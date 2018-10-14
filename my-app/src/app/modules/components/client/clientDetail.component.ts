import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { Client } from '../../../shared/models/client';
import { ClientService } from '../../../core/services/client.service';
import { CodeValidator } from '../../../shared/directives/code.directive';

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

    constructor(private clientService: ClientService,public Validator: CodeValidator,
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
            this.proofFormat = this.proofFormats.find(g => g.Value == null);
        }
        else {
            this.getClient();
        }
        this.clientform = this.fb.group({
            'name': new FormControl('', Validators.required),
            'code': new FormControl('', Validators.compose([Validators.required,Validators.minLength(3),Validators.maxLength(3)]),this.Validator.checkUsername.bind(this.Validator)),
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
            this.clientService.addClient(this.selectedClient).subscribe(() => { this.selectedClient = null; this.router.navigate(['/' + localStorage.getItem('role') + '/clients']); });
        } else {
            this.selectedClient.ProofFormat = this.proofFormat.Value;
            this.clientService.updateClient(this.selectedClient).subscribe(() => { this.selectedClient = null; this.router.navigate(['/' + localStorage.getItem('role') + '/clients']); });
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
