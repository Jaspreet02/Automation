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
import { FileTransfer } from '../fileTransfer';
import { FileTransferService } from '../fileTransfer.service';

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

    fileTransfers: FileTransfer[];

    fileTransfer: FileTransfer;

    applicationform: FormGroup;

    constructor(private applicationService: ApplicationService, private clientService: ClientService, private fileTransferService: FileTransferService,
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

    }

    ngOnInit() {
        if (this.newApplication) {
            this.selectedApplication = new Application();
            this.clientService.getClients(0,0,'CreatedAt','desc',true).subscribe(c=> this.clients = c.Result);            
            this.fileTransferService.getFileTransfers(0,0,'CreatedAt','desc',true).subscribe(c=> this.fileTransfers = c.Result);                
        }
        else {
            this.getApplication();
        }
        this.applicationform = this.fb.group({
            'client': new FormControl('', Validators.required),
            'fileTransfer': new FormControl('', Validators.required),
            'name': new FormControl('', Validators.required),
            'code': new FormControl('', Validators.required),
            'hotFolder': new FormControl('', Validators.required),
            'archivePath': new FormControl('', Validators.required),
            'archiveFileName': new FormControl(''),
            'isArchive': new FormControl(''),
            'inputPath': new FormControl('', Validators.required),
            'isFileMove': new FormControl(''),
            'isBatch': new FormControl(''),
            'status': new FormControl('')
        });
        this.applicationform.get('isArchive').valueChanges.subscribe(
            (isArchive:boolean)=> {
                if(isArchive)
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
        this.fileTransferService.getFileTransfers(0,0,'CreatedAt','desc',true).subscribe(x=> { this.fileTransfers = x.Result;       
        this.clientService.getClients(0,0,'CreatedAt','desc',true).subscribe(c=> {this.clients = c.Result ;                
        this.applicationService.getApplication(this.ApplicationId)
            .subscribe(x => { 
                this.selectedApplication = x;
                this.client = this.clients.find(f=> f.ClientId == this.selectedApplication.ClientId);
               this.fileTransfer = this.fileTransfers.find(f=> f.FileTransferSettingId == this.selectedApplication.FileTransferSettingId);
            });});});
     }   

    save() {
        if (this.newApplication) {
            this.selectedApplication.ClientId = this.client.ClientId;
            this.selectedApplication.FileTransferSettingId = this.fileTransfer.FileTransferSettingId;
            this.applicationService.addApplication(this.selectedApplication).subscribe(x => { this.selectedApplication = null; this.router.navigate(['/applications']); });
        } else {
            this.selectedApplication.ClientId = this.client.ClientId;
            this.selectedApplication.FileTransferSettingId = this.fileTransfer.FileTransferSettingId;
             this.applicationService.updateApplication(this.selectedApplication).subscribe(x => { this.selectedApplication = null; this.router.navigate(['/applications']); });
        }
    }

    cancel() {
        this.selectedApplication = null;
        this.location.back();
    }

}
