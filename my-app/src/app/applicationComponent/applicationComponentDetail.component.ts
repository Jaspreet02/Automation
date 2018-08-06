import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { ApplicationComponent } from '../applicationComponent';
import { ApplicationComponentService } from '../applicationComponent.service';
import { ComponentExe } from '../componentExe';
import { ComponentExeService } from '../componentExe.service';

@Component({
    selector: 'app-applicationComponentDetail',
    templateUrl: './applicationComponentDetail.component.html',
    styleUrls: ['./applicationComponentDetail.component.css']
})

export class ApplicationComponentDetailComponent implements OnInit {

    components: ComponentExe[];

    component: ComponentExe;

    maximunValue : number;

    ApplicationComponentId: string;

    ApplicationId: string;

    newApplicationComponent: boolean;

    selectedApplicationComponent: ApplicationComponent;

    applicationComponentform: FormGroup;

    constructor(private componentExeService: ComponentExeService, private applicationComponentService: ApplicationComponentService, private location: Location, private router: Router, private route: ActivatedRoute, private fb: FormBuilder) {
        this.route.params.subscribe(res => {
            if (res['id']) {
                this.ApplicationComponentId = res.id;
                this.ApplicationId = res['applicationId'];
                this.maximunValue = res['maxValue'];
                this.newApplicationComponent = false;
            }
            else {
                this.newApplicationComponent = true;
                this.ApplicationId = res['applicationId'];
                this.maximunValue = res['maxValue'];
            }
        });
    }

    ngOnInit() {
        if (this.newApplicationComponent) {
            this.selectedApplicationComponent = new ApplicationComponent();
            this.componentExeService.getComponentExes(0, 0, 'CreatedAt', 'desc', true).subscribe(c => this.components = c.Result);
        }
        else {
            this.getApplicationComponent();
        }

        this.applicationComponentform = this.fb.group({
            'component': new FormControl('', Validators.required),
            'order': new FormControl('', Validators.required),
            'isOptional': new FormControl(''),
            'status': new FormControl('')
        });
    }

    getApplicationComponent(): void {
        this.componentExeService.getComponentExes(0, 0, 'CreatedAt', 'desc', true).subscribe(c => {
        this.components = c.Result;
            this.applicationComponentService.getApplicationComponent(this.ApplicationComponentId)
                .subscribe(x => {
                this.selectedApplicationComponent = x;
                    this.component = this.components.find(c => c.ComponentId == x.ComponentId)
                }
                )
        });
    }

    save() {
        this.selectedApplicationComponent.ApplicationId = parseInt(this.ApplicationId);
        this.selectedApplicationComponent.ComponentId = this.component.ComponentId;
        if (this.newApplicationComponent) {
            this.applicationComponentService.addApplicationComponent(this.selectedApplicationComponent).subscribe(() => { this.selectedApplicationComponent = null;this.location.back() });
        } else {
            this.applicationComponentService.updateApplicationComponent(this.selectedApplicationComponent).subscribe(() => { this.selectedApplicationComponent = null; this.location.back() });
        }
    }

    cancel() {
        this.selectedApplicationComponent = null;
        this.location.back();
    }
}
