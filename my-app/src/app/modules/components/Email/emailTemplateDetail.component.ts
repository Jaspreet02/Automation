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

    emailTokens: any[];

    clients: Client[];

    client: Client;

    applications: Application[];

    application: Application;

    components: ComponentExe[];

    component: ComponentExe;

    emailToken: any;

    text: string;
    
    loading: boolean;

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
            this.masterService.getEmailTokens().subscribe(x=> {this.emailTokens = x;
                this.emailToken = this.emailTokens.indexOf(0)});             
         }
        else {
            this.getEmailTransfer();
        }
        this.emailTemplateForm = this.fb.group({
            'client': new FormControl(''),
            'application': new FormControl(''),
            'component': new FormControl(''),
            'subject': new FormControl('', Validators.required),
            'body': new FormControl('', Validators.required),
            'emailToken': new FormControl('',Validators.required),
            'emailToIds': new FormControl('',Validators.compose([Validators.required,Validators.email])),
            'emailCcIds': new FormControl('',Validators.email),
            'timeInterval': new FormControl(''),
            'status': new FormControl('')
        });
    }

    getEmailTransfer(): void {
        this.loading = true;
             this.masterService.getEmailTokens().subscribe(c => {
                    this.emailTokens = c;
                    this.emailTemplateService.getEmailTemplate(this.EmailTemplateId)
                        .subscribe(x => {                            
                             this.selectedEmailTemplate = x ;
                             this.emailToken = this.emailTokens.find(x=> x.Keyword == this.selectedEmailTemplate.EmailToken);
                             this.selectedEmailTemplate.ClientId > 0 ? 
                             this.clientService.getClients(0, 0, 'CreatedAt', 'desc', true).subscribe(c => {
                                 this.clients = c.Result;
                                 this.client = this.clients.find(x=> x.ClientId == this.selectedEmailTemplate.ClientId);
                                 this.selectedEmailTemplate.ApplicationId > 0 ?
                                   this.applicationService.getApplicationbyClientId(this.selectedEmailTemplate.ClientId).subscribe(c => {
                                    this.applications = c;
                                    this.application = this.applications.find(x=> x.ApplicationId == this.selectedEmailTemplate.ApplicationId);
                               this.selectedEmailTemplate.ApplicationComponentId > 0 ?
                                this.componentService.applicationComponent(this.selectedEmailTemplate.ApplicationId).subscribe(c=> {
                                   this.components = c;
                                   this.component = this.components.find(x=> x.ComponentId == this.selectedEmailTemplate.ApplicationComponentId);
                                }) : null;
                                }) : null;
                                }) : null ;
                              this.loading = false});
                });
    }

    getApplications() {
        this.application = null;
        this.component = null;
        this.applicationService.getApplicationbyClientId(this.client.ClientId).subscribe(c => this.applications = c);
    }

    getComponents() {
        this.componentService.applicationComponent(this.application.ApplicationId).subscribe(c => this.components = c);
    }

    save() {
        this.selectedEmailTemplate.EmailFromSmtpId = 1;
        this.selectedEmailTemplate.EmailToken = this.emailToken.Keyword;
        this.selectedEmailTemplate.ClientId =this.client != null ? this.client.ClientId : 0;
        this.selectedEmailTemplate.ApplicationId =this.application != null ? this.application.ApplicationId : 0;
        this.selectedEmailTemplate.ApplicationComponentId = this.component!= null ? this.component.ComponentId : 0;
        if (this.newEmailTemplate) {
            this.emailTemplateService.addEmailTemplate(this.selectedEmailTemplate).subscribe(x => { this.selectedEmailTemplate = null; this.router.navigate(['/' + localStorage.getItem('role') + '/emails']); });
        } else {
            this.emailTemplateService.updateEmailTemplate(this.selectedEmailTemplate).subscribe(x => { this.selectedEmailTemplate = null; this.router.navigate(['/' + localStorage.getItem('role') + '/emails']); });
        }
    }

    cancel() {
        this.selectedEmailTemplate = null;
        this.location.back();
    }
}
