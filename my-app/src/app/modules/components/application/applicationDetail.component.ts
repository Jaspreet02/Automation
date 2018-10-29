import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { Validators, FormControl, FormGroup, FormBuilder, ValidatorFn, AbstractControl } from '@angular/forms';
import { Application } from '../../../shared/models/application';
import { Client } from '../../../shared/models/client';
import { FileTransfer } from '../../../shared/models/fileTransfer';
import { ApplicationService } from '../../../core/services/application.service';
import { ClientService } from '../../../core/services/client.service';
import { FileTransferService } from '../../../core/services/fileTransfer.service';

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

    existingTags = [];

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
            'code': new FormControl('', Validators.compose([Validators.required, Validators.minLength(3), Validators.maxLength(3), this.validCode()])),
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

            this.applicationService.CodeExistList(parseInt(this.ApplicationId)).subscribe((res) => {
                this.existingTags = res;
            });
     }   

    save() {
        if (this.newApplication) {
            this.selectedApplication.ClientId = this.client.ClientId;
            this.selectedApplication.FileTransferSettingId = this.fileTransfer.FileTransferSettingId;
            this.applicationService.addApplication(this.selectedApplication).subscribe(() => { this.selectedApplication = null; this.router.navigate(['/' + localStorage.getItem('role') + '/applications']); });
        } else {
            this.selectedApplication.ClientId = this.client.ClientId;
            this.selectedApplication.FileTransferSettingId = this.fileTransfer.FileTransferSettingId;
             this.applicationService.updateApplication(this.selectedApplication).subscribe(() => { this.selectedApplication = null; this.router.navigate(['/' + localStorage.getItem('role') + '/applications']); });
        }
    }

    cancel() {
        this.selectedApplication = null;
        this.location.back();
    }

    private validCode(): ValidatorFn {

        return (control: AbstractControl): { [key: string]: boolean } => {

            if (control.value.length == 3) {
                if (this.existingTags.includes(control.value)) {
                    return { 'validCode': true };
                }
                else {
                    return null;
                }
            }
            else {
                return null;
            }
        }
    }

}
