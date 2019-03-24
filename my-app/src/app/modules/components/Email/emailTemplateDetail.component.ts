import { Component, OnInit, Input, OnChanges } from '@angular/core';
import { Location } from '@angular/common';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { EmailTemplate } from '../../../shared/models/emailTemplate';
import { EmailTemplateService } from '../../../core/services/emailTemplate.service';
import { MasterService } from '../../../core/services/master.service';
import { Client } from '../../../shared/models/client';
import { ApplicationService } from '../../../core/services/application.service';
import { ClientService } from '../../../core/services/client.service';
import { Application } from '../../../shared/models/application';
import { ComponentExe } from '../../../shared/models/componentExe';
import { ComponentExeService } from '../../../core/services/componentExe.service';

@Component({
    selector: 'app-emailTemplateDetail',
    templateUrl: './emailTemplateDetail.component.html',
    styleUrls: ['./emailTemplateDetail.component.css']
})

export class EmailTemplateDetailComponent implements OnInit {

    EmailTemplateId: string;

    newEmailTemplate: boolean;

    selectedEmailTemplate: EmailTemplate;

    queueTypes: any[];

    clients: Client[];

    client: Client;

    applications: Application[];

    application: Application;

    components: ComponentExe[];

    component: ComponentExe;

    queueType: any;

    text: string;

    emailTemplateForm: FormGroup;

    constructor(private componentService: ComponentExeService, private applicationService: ApplicationService, private clientService: ClientService, private emailTemplateService: EmailTemplateService, private masterService: MasterService, private location: Location, private router: Router, private route: ActivatedRoute, private fb: FormBuilder) {
        this.route.params.subscribe(res => {
            if (res['id']) {
                this.EmailTemplateId = res.id;
                this.newEmailTemplate = false;
            }
            else {
                this.newEmailTemplate = true;
            }
        });
    }

    ngOnInit() {
        if (this.newEmailTemplate) {
            this.selectedEmailTemplate = new EmailTemplate();
            this.clientService.getClients(0, 0, 'CreatedAt', 'desc', true).subscribe(c => this.clients = c.Result);
            this.applicationService.getApplications(0, 0, 'CreatedAt', 'desc', true).subscribe(c => this.applications = c.Result);
            this.componentService.getComponentExes(0, 0, 'CreatedAt', 'desc', true).subscribe(c => this.components = c.Result);
        }
        else {
            this.getEmailTransfer();
        }
        this.emailTemplateForm = this.fb.group({
            'client': new FormControl('', Validators.required),
            'application': new FormControl('', Validators.required),
            'component': new FormControl('', Validators.required),
            'subject': new FormControl('', Validators.required),
            'body': new FormControl('', Validators.required),
            'emailToken': new FormControl('', Validators.required),
            'emailToIds': new FormControl('', Validators.required),
            'emailCcIds': new FormControl('', Validators.required),
            'timeInterval': new FormControl('', Validators.required),
            'status': new FormControl('')
        });
    }

    getEmailTransfer(): void {
        this.clientService.getClients(0, 0, 'CreatedAt', 'desc', true).subscribe(c => {
            this.clients = c.Result;
            this.applicationService.getApplications(0, 0, 'CreatedAt', 'desc', true).subscribe(c => {
                this.applications = c.Result;
                this.componentService.getComponentExes(0, 0, 'CreatedAt', 'desc', true).subscribe(c => {
                    this.components = c.Result;
                    this.masterService.getQueueTypes().subscribe(c => {
                        this.queueTypes = c;
                        this.emailTemplateService.getEmailTemplate(this.EmailTemplateId)
                            .subscribe(x => {
                                this.selectedEmailTemplate = x
                            });
                    });
                });
            });
        })
    }

    save() {
        if (this.newEmailTemplate) {
            this.emailTemplateService.addEmailTemplate(this.selectedEmailTemplate).subscribe(x => { this.selectedEmailTemplate = null; this.router.navigate(['/' + localStorage.getItem('role') + '/fileTransfers']); });
        } else {
            this.emailTemplateService.updateEmailTemplate(this.selectedEmailTemplate).subscribe(x => { this.selectedEmailTemplate = null; this.router.navigate(['/' + localStorage.getItem('role') + '/fileTransfers']); });
        }
    }

    cancel() {
        this.selectedEmailTemplate = null;
        this.location.back();
    }
}
