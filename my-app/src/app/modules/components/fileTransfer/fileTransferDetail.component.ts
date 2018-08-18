import { Component, OnInit, Input, OnChanges } from '@angular/core';
import { Location } from '@angular/common';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { FileTransfer } from '../../../shared/models/fileTransfer';
import { FileTransferService } from '../../../core/services/fileTransfer.service';
import { MasterService } from '../../../core/services/master.service';

@Component({
    selector: 'app-fileTransferDetail',
    templateUrl: './fileTransferDetail.component.html',
    styleUrls: ['./fileTransferDetail.component.css']
})

export class FileTransferDetailComponent implements OnInit {

    FileTransferId: string;

    newFileTransfer: boolean;

    selectedFileTransfer: FileTransfer;

    queueTypes: any[];

    queueType: any;

    fileTransferForm: FormGroup;

    constructor(private fileTransferService: FileTransferService, private masterService: MasterService,  private location: Location, private router: Router, private route: ActivatedRoute, private fb: FormBuilder) {
        this.route.params.subscribe(res => {
            if (res['id']) {
                this.FileTransferId = res.id;
                this.newFileTransfer = false;
            }
            else {
                this.newFileTransfer = true;
            }
        });
    }

    ngOnInit() {
        if (this.newFileTransfer) {
            this.selectedFileTransfer = new FileTransfer();
            this.masterService.getQueueTypes().subscribe(x=> this.queueTypes = x);
         }
        else {
            this.getFileTransfer();
        }
        this.fileTransferForm = this.fb.group({
            'name': new FormControl('', Validators.required),
            'queueType': new FormControl('', Validators.required),
            'host': new FormControl('', Validators.required),
            'userName': new FormControl('', Validators.required),
            'password': new FormControl('', Validators.required),
            'port': new FormControl('', Validators.required),
            'status': new FormControl('')
        });
        this.fileTransferForm.get('queueType').valueChanges.subscribe(
            (queueType:any)=> {
                if(queueType.QueueTypeId === 4)
                {
                    this.fileTransferForm.get('host').setValidators(null);
                    this.fileTransferForm.get('userName').setValidators(null);
                    this.fileTransferForm.get('password').setValidators(null);
                    this.fileTransferForm.get('port').setValidators(null);
                }
                else
                {
                    this.fileTransferForm.get('host').setValidators([Validators.required]);
                    this.fileTransferForm.get('userName').setValidators([Validators.required]);
                    this.fileTransferForm.get('password').setValidators([Validators.required]);
                    this.fileTransferForm.get('port').setValidators([Validators.required]);
                }
                this.fileTransferForm.get('host').updateValueAndValidity();
                this.fileTransferForm.get('userName').updateValueAndValidity();
                this.fileTransferForm.get('password').updateValueAndValidity();
                this.fileTransferForm.get('port').updateValueAndValidity();
            }
        )
    }

    getFileTransfer(): void {        
        this.masterService.getQueueTypes().subscribe(c=> {this.queueTypes = c ;                
            this.fileTransferService.getFileTrasnfer(this.FileTransferId)
                .subscribe(x => { 
                    this.selectedFileTransfer = x;
                    this.queueType = this.queueTypes.find(s=> s.QueueTypeId == this.selectedFileTransfer.QueueTypeId);
                  });});
    }   

    save() {
        if (this.newFileTransfer) {
            this.selectedFileTransfer.QueueTypeId = this.queueType.QueueTypeId;
            this.fileTransferService.addFileTransfer(this.selectedFileTransfer).subscribe(x => { this.selectedFileTransfer = null; this.router.navigate(['/fileTransfers']); });
        } else {
            this.selectedFileTransfer.QueueTypeId = this.queueType.QueueTypeId;
            this.fileTransferService.updateFileTransfer(this.selectedFileTransfer).subscribe(x => { this.selectedFileTransfer = null; this.router.navigate(['/fileTransfers']); });
        }
    }

    cancel() {
        this.selectedFileTransfer = null;
        this.location.back();
    }
}
