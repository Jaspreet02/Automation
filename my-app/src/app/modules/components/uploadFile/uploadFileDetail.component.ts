import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { ComponentExe } from '../../../shared/models/componentExe';
import { ComponentExeService } from '../../../core/services/componentExe.service';
import { UploadFile } from '../../../shared/models/uploadFile';
import { UploadFileService } from '../../../core/services/uploadFile.service';
import { Application } from '../../../shared/models/application';
import { ApplicationService } from '../../../core/services/application.service';
import { FileTransfer } from '../../../shared/models/fileTransfer';
import { FileTransferService } from '../../../core/services/fileTransfer.service';

@Component({
    selector: 'app-uploadFileDetail',
    templateUrl: './uploadFileDetail.component.html',
    styleUrls: ['./uploadFileDetail.component.css']
})

export class UploadFileDetailComponent implements OnInit {

    components: ComponentExe[];

    component: ComponentExe;

    applications: Application[];

    application: Application;

    fileTransfers: FileTransfer[];

    archiveFileTransfer: FileTransfer;

    moveFileTransfer: FileTransfer;

    UploadFileId: string;

    newUploadFile: boolean;

    selectedUploadFile: UploadFile;

    uploadFileform: FormGroup;

    constructor(private componentExeService: ComponentExeService, private fileTransferService: FileTransferService, private applicationService: ApplicationService, private service: UploadFileService, private location: Location, private router: Router, private route: ActivatedRoute, private fb: FormBuilder) {
        this.route.params.subscribe(res => {
            if (res['id']) {
                this.UploadFileId = res.id;
                this.newUploadFile = false;
            }
            else {
                this.newUploadFile = true;
            }
        });
    }

    ngOnInit() {
        if (this.newUploadFile) {
            this.selectedUploadFile = new UploadFile();
            this.applicationService.getApplications(0, 0, 'CreatedAt', 'desc', true).subscribe(c => this.applications = c.Result);
            this.componentExeService.getComponentExes(0, 0, 'CreatedAt', 'desc', true).subscribe(c => this.components = c.Result);
            this.fileTransferService.getFileTransfers(0,0,'CreatedAt','desc',true).subscribe(c=> this.fileTransfers = c.Result);
        }
        else {
            this.get();
        }

        this.uploadFileform = this.fb.group({
            'application': new FormControl('', Validators.required),
            'component': new FormControl('', Validators.required),
            'name': new FormControl('', Validators.required),
            'fileInputPath': new FormControl('', Validators.required),
            'inputFileMask': new FormControl('', Validators.required),
            'archiveOutputPath': new FormControl('', Validators.required),
            'archiveFileTransferSettingId': new FormControl('', Validators.required),
            'archiveFileExpression': new FormControl(''),
            'isArchiveOutputRequired': new FormControl(''),
            'moveFilePath': new FormControl('', Validators.required),
            'moveFileExpression': new FormControl(''),
            'isMoveFileRequired': new FormControl(''),
            'moveFileTransferSettingId': new FormControl('', Validators.required),
            'status': new FormControl('')
        });
        this.uploadFileform.get('isMoveFileRequired').valueChanges.subscribe(
            (isMoveFileRequired: boolean) => {
                if (isMoveFileRequired) {
                    this.uploadFileform.get('moveFilePath').setValidators([Validators.required]);
                    this.uploadFileform.get('moveFileExpression').setValidators([Validators.required]);
                    this.uploadFileform.get('moveFileTransferSettingId').setValidators([Validators.required]);
                }
                else {
                    this.uploadFileform.get('moveFilePath').setValidators(null);
                    this.uploadFileform.get('moveFileExpression').setValidators(null);
                    this.uploadFileform.get('moveFileTransferSettingId').setValidators(null);
                }
                this.uploadFileform.get('moveFilePath').updateValueAndValidity();
                this.uploadFileform.get('moveFileExpression').updateValueAndValidity();
                this.uploadFileform.get('moveFileTransferSettingId').updateValueAndValidity();
            }
        )
        this.uploadFileform.get('isArchiveOutputRequired').valueChanges.subscribe(
            (isArchiveOutputRequired: boolean) => {
                if (isArchiveOutputRequired) {
                    this.uploadFileform.get('archiveOutputPath').setValidators([Validators.required]);
                    this.uploadFileform.get('archiveFileExpression').setValidators([Validators.required]);
                    this.uploadFileform.get('archiveFileTransferSettingId').setValidators([Validators.required]);
                }
                else {
                    this.uploadFileform.get('archiveOutputPath').setValidators(null);
                    this.uploadFileform.get('archiveFileExpression').setValidators(null);
                    this.uploadFileform.get('archiveFileTransferSettingId').setValidators(null);
                }
                this.uploadFileform.get('archiveOutputPath').updateValueAndValidity();
                this.uploadFileform.get('archiveFileExpression').updateValueAndValidity();
                this.uploadFileform.get('archiveFileTransferSettingId').updateValueAndValidity();
            }
        )
    }

    get(): void {
        this.fileTransferService.getFileTransfers(0, 0, 'CreatedAt', 'desc', true).subscribe(x => {
        this.fileTransfers = x.Result;
            this.applicationService.getApplications(0, 0, 'CreatedAt', 'desc', true).subscribe(c => {
            this.applications = c.Result;
                this.componentExeService.getComponentExes(0, 0, 'CreatedAt', 'desc', true).subscribe(c => {
                    this.components = c.Result;
                    this.service.get(this.UploadFileId)
                        .subscribe(x => {
                            this.selectedUploadFile = x;
                            this.component = this.components.find(c => c.ComponentId == x.ComponentId);
                            this.application = this.applications.find(c => c.ApplicationId == x.ApplicationId);
                            this.archiveFileTransfer = this.fileTransfers.find(f => f.FileTransferSettingId == this.selectedUploadFile.ArchiveFileTransferSettingId)
                            this.moveFileTransfer = this.fileTransfers.find(f => f.FileTransferSettingId == this.selectedUploadFile.MoveFileTransferSettingId)
                        })
                });
            });
        });
    }

    save() {
        this.selectedUploadFile.ComponentId = this.component.ComponentId;
        this.selectedUploadFile.ApplicationId = this.application.ApplicationId;
        if (this.selectedUploadFile.IsArchiveOutputRequired) {
            this.selectedUploadFile.ArchiveFileTransferSettingId = this.archiveFileTransfer.FileTransferSettingId;
        }
        else{
            this.selectedUploadFile.ArchiveFileExpression = null;
            this.selectedUploadFile.ArchiveFileTransferSettingId = null;
            this.selectedUploadFile.ArchiveOutputPath = null;
        }
        if (this.selectedUploadFile.IsMoveFileRequired) {
            this.selectedUploadFile.MoveFileTransferSettingId = this.moveFileTransfer.FileTransferSettingId;
        }
        else{
            this.selectedUploadFile.MoveFileExpression = null;
            this.selectedUploadFile.MoveFileTransferSettingId = null;
            this.selectedUploadFile.MoveFilePath = null;
        }
        if (this.newUploadFile) {
            this.service.add(this.selectedUploadFile).subscribe(() => { this.selectedUploadFile = null; this.location.back() });
        } else {
            this.service.update(this.selectedUploadFile).subscribe(() => { this.selectedUploadFile = null; this.location.back() });
        }
    }

    cancel() {
        this.selectedUploadFile = null;
        this.location.back();
    }
}
