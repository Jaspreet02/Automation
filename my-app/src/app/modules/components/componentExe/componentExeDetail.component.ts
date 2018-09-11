import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { ComponentExe } from '../../../shared/models/componentExe';
import { ComponentExeService } from '../../../core/services/componentExe.service';

@Component({
    selector: 'app-componentExeDetail',
    templateUrl: './componentExeDetail.component.html',
    styleUrls: ['./componentExeDetail.component.css']
})

export class ComponentExeDetailComponent implements OnInit {

    ComponentId: string;

    newComponent: boolean;

    selectedComponentExe: ComponentExe;

    componentExeform: FormGroup;

    constructor(private componentService: ComponentExeService, private location: Location, private router: Router, private route: ActivatedRoute, private fb: FormBuilder) {
        this.route.params.subscribe(res => {
            if (res['id']) {
                this.ComponentId = res.id;
                this.newComponent = false;
            }
            else {
                this.newComponent = true;
            }
        });
    }

    ngOnInit() {
        if (this.newComponent) {
            this.selectedComponentExe = new ComponentExe();
        }
        else {
            this.getComponentExe();
        }

        this.componentExeform = this.fb.group({
            'name': new FormControl('', Validators.required),
            'shortName': new FormControl('', Validators.required),
            'componentExe': new FormControl('', Validators.required),
            'detail': new FormControl('', Validators.required),            
            'status': new FormControl('')
        });
    }

    getComponentExe(): void { 
        this.componentService.getComponentExe(this.ComponentId)
            .subscribe(x => this.selectedComponentExe = x);
     }   

    save() {
        if (this.newComponent) {
             this.componentService.addComponentExe(this.selectedComponentExe).subscribe(() => { this.selectedComponentExe = null; this.router.navigate(['/' + localStorage.getItem('role') + '/componentExes']); });
        } else {
            this.componentService.updateComponentExe(this.selectedComponentExe).subscribe(() => { this.selectedComponentExe = null; this.router.navigate(['/' + localStorage.getItem('role') + '/componentExes']); });
        }
    }

    cancel() {
        this.selectedComponentExe = null;
        this.location.back();
    }
}
