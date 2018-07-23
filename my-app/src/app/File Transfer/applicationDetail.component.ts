import { Component, OnInit, Input, OnChanges } from '@angular/core';
import { Application } from '../application';
import { ApplicationService } from '../application.service';
import { Location } from '@angular/common';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { AppPage } from '../../../e2e/app.po';
import { NumberValueAccessor } from '@angular/forms/src/directives/number_value_accessor';
import { flattenStyles } from '@angular/platform-browser/src/dom/dom_renderer';
import { timingSafeEqual } from 'crypto';
import { GenericBrowserDomAdapter } from '@angular/platform-browser/src/browser/generic_browser_adapter';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { Client } from '../client';
import { ClientService } from '../client.service';

@Component({
    selector: 'app-applicationDetail',
    templateUrl: './applicationDetail.component.html',
    styleUrls: ['./applicationDetail.component.css']
})

export class ApplicationDetailComponent implements OnInit {

    ApplicationId: string;

    newApplication: boolean;

    selectedApplication: Application;

    clients: Client[];

    client: Client;

    proofFormats: SelectedItem[];

    proofFormat: SelectedItem;

    statuses: SelectedItem[];

    status: SelectedItem;

    isArchive: SelectedItem;

    isFileMove: SelectedItem;

    isBatch: SelectedItem;

    applicationform: FormGroup;

    constructor(private applicationService: ApplicationService, private clientService: ClientService,
        private location: Location, private router: Router, private route: ActivatedRoute, private fb: FormBuilder) {
        this.route.params.subscribe(res => {
            if (res['id']) {
                this.ApplicationId = res.id;
                this.newApplication = false;
            }
            else {
                this.newApplication = true;
            }

        });

        this.statuses = [
            { Label: 'True', Value: 'true' },
            { Label: 'False', Value: 'false' }
        ];

    }

    ngOnInit() {
        if (this.newApplication) {
            this.selectedApplication = new Application();
            this.isArchive = this.statuses.find(s => s.Value == 'false');
            this.clientService.getClients(0,0,'CreatedAt','desc',true).subscribe(c=> this.clients = c.Result);                
        }
        else {
            this.getApplication();
        }
        this.applicationform = this.fb.group({
            'client': new FormControl('', Validators.required),
            'name': new FormControl('', Validators.required),
            'code': new FormControl('', Validators.required),
            'hotFolder': new FormControl('', Validators.required),
            'archivePath': new FormControl('', Validators.required),
            'archiveFileName': new FormControl(''),
            'isArchive': new FormControl('', Validators.required),
            'inputPath': new FormControl('', Validators.required),
            'isFileMove': new FormControl('', Validators.required),
            'isBatch': new FormControl('', Validators.required),
            'status': new FormControl('', Validators.required)
        });
        this.applicationform.get('isArchive').valueChanges.subscribe(
            (isArchive:SelectedItem)=> {
                if(isArchive.Value === 'true')
                {
                    this.applicationform.get('archivePath').setValidators([Validators.required]);
                    this.applicationform.get('archiveFileName').setValidators([Validators.required]);
                }
                else
                {
                    this.applicationform.get('archivePath').setValidators(null);
                    this.applicationform.get('archiveFileName').setValidators(null);
                }
                this.applicationform.get('archivePath').updateValueAndValidity();
                this.applicationform.get('archiveFileName').updateValueAndValidity();
            }
        )
    }

    getApplication(): void {        
        this.clientService.getClients(0,0,'CreatedAt','desc',true).subscribe(c=> {this.clients = c.Result ;                
        this.applicationService.getApplication(this.ApplicationId)
            .subscribe(x => { 
                this.selectedApplication = x;
                this.client = this.clients.find(f=> f.ClientId == this.selectedApplication.ClientId);
                this.status = this.statuses.find(s => s.Value == this.selectedApplication.Status.toString());
                this.isArchive = this.statuses.find(s => s.Value == this.selectedApplication.IsArchive.toString());
                this.isFileMove = this.statuses.find(s => s.Value == this.selectedApplication.IsFileMove.toString());
                this.isBatch = this.statuses.find(s => s.Value == this.selectedApplication.IsBatch.toString())
            });});
    }   

    save() {
        if (this.newApplication) {
            this.selectedApplication.ClientId = this.client.ClientId;
            this.selectedApplication.FileTransferSettingId = 1;
            this.selectedApplication.Status = (this.status.Value == 'true');
            this.selectedApplication.IsArchive = (this.isArchive.Value == 'true');
            this.selectedApplication.IsFileMove = (this.isFileMove.Value == 'true');
            this.selectedApplication.IsBatch = (this.isBatch.Value == 'true');
            this.applicationService.addApplication(this.selectedApplication).subscribe(x => { this.selectedApplication = null; this.router.navigate(['/applications']); });
        } else {
            this.selectedApplication.ClientId = this.client.ClientId;
            this.selectedApplication.FileTransferSettingId = 1;
            this.selectedApplication.Status = (this.status.Value == 'true');
            this.selectedApplication.IsArchive = (this.isArchive.Value == 'true');
            this.selectedApplication.IsFileMove = (this.isFileMove.Value == 'true');
            this.selectedApplication.IsBatch = (this.isBatch.Value == 'true');
            this.applicationService.updateApplication(this.selectedApplication).subscribe(x => { this.selectedApplication = null; this.router.navigate(['/applications']); });
        }
    }

    cancel() {
        this.selectedApplication = null;
        this.location.back();
    }

}

interface SelectedItem {
    Label: string;
    Value: string
}
