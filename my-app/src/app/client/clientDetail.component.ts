import { Component, OnInit, Input, OnChanges } from '@angular/core';
import { Client } from '../client';
import { ClientService } from '../client.service';
import { Location } from '@angular/common';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { AppPage } from '../../../e2e/app.po';
import { NumberValueAccessor } from '@angular/forms/src/directives/number_value_accessor';
import { flattenStyles } from '@angular/platform-browser/src/dom/dom_renderer';
import { timingSafeEqual } from 'crypto';
import { GenericBrowserDomAdapter } from '@angular/platform-browser/src/browser/generic_browser_adapter';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';

@Component({
    selector: 'app-clientDetail',
    templateUrl: './clientDetail.component.html',
    styleUrls: ['./clientDetail.component.css']
})

export class ClientDetailComponent implements OnInit {

    clientId: string;

    newClient: boolean;

    selectedClient: Client;

    proofFormats: SelectedItem[];

    proofFormat: SelectedItem;

    clientform: FormGroup;

    constructor(private clientService: ClientService,
        private location: Location, private router: Router, private route: ActivatedRoute, private fb: FormBuilder) {
        this.route.params.subscribe(res => {
            if (res['id']) {
                this.clientId = res.id;
                this.newClient = false;
            }
            else {
                this.newClient = true;
            }
        });

        this.proofFormats = [
            { Label: 'NONE', Value: null },
            { Label: 'ZIP', Value: 'ZIP' },
            { Label: 'PGP', Value: 'PGP' }
        ];
    }

    ngOnInit() {
        if (this.newClient) {
            this.selectedClient = new Client();
        }
        else {
            this.getClient();
        }
        this.clientform = this.fb.group({
            'name': new FormControl('', Validators.required),
            'code': new FormControl('', Validators.required),
            'contact': new FormControl('', Validators.required),
            'emailAddress': new FormControl('', Validators.compose([Validators.required, Validators.email])),
            'proofFormat': new FormControl('', Validators.required),
            'proofPassword': new FormControl(''),
            'proofName':new FormControl(''),
            'status': new FormControl('')
        });
    }

    getClient(): void {
        this.clientService.getClient(this.clientId)
            .subscribe(x => {
                this.selectedClient = x;
                this.proofFormat = this.proofFormats.find(g => g.Value == this.selectedClient.ProofFormat);
             });
    }

    save() {
        if (this.newClient) {
            this.selectedClient.ProofFormat = this.proofFormat.Value;
            this.clientService.addClient(this.selectedClient).subscribe(x => { this.selectedClient = null; this.router.navigate(['/clients']); });
        } else {
            this.selectedClient.ProofFormat = this.proofFormat.Value;
            this.clientService.updateClient(this.selectedClient).subscribe(x => { this.selectedClient = null; this.router.navigate(['/clients']); });
        }
    }

    cancel() {
        this.selectedClient = null;
        this.location.back();
    }

}

interface SelectedItem {
    Label: string;
    Value: string
}
