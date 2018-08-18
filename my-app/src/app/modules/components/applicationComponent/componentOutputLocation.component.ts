import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { ComponentOutputLocation } from '../../../shared/models/componentOutputLocation';
import { ApplicationComponentService } from '../../../core/services/applicationComponent.service';

@Component({
  selector: 'app-componentOutputLocation',
  templateUrl: './componentOutputLocation.component.html',
  styleUrls: ['./componentOutputLocation.component.css']
})

export class ComponentOutputLocationComponent implements OnInit {

  ComponentOutputLocations: ComponentOutputLocation[];

  ComponentOutputLocation: ComponentOutputLocation;

  ApplicationId: number;

  ComponentId: number;

  displayDialog: boolean;

  newEntity: boolean;

  constructor(private router: Router, private route: ActivatedRoute, private service: ApplicationComponentService) {
    this.route.queryParams.subscribe(res => {
      this.ApplicationId = res['applicationId'];
      this.ComponentId = res['componentId'];
    });
  }

  ngOnInit() {
    this.service
      .outputLocations(this.ApplicationId, this.ComponentId)
      .subscribe(x => (this.ComponentOutputLocations = x));
  }

  showDialogToAdd() {
    this.newEntity = true;
    this.ComponentOutputLocation = new ComponentOutputLocation();
    this.ComponentOutputLocation.ApplicationId = this.ApplicationId;
    this.ComponentOutputLocation.ComponentId = this.ComponentId;
    this.displayDialog = true;
  }

  save() {
    let cars = [...this.ComponentOutputLocations];

    this.service
      .addOutputLocation(this.ComponentOutputLocation)
      .subscribe(x => {
        if (this.newEntity)
          cars.push(this.ComponentOutputLocation);
        else
          cars[this.ComponentOutputLocations.indexOf(this.ComponentOutputLocation)] = this.ComponentOutputLocation;
        this.ComponentOutputLocations = cars;
        this.ComponentOutputLocation = null;
        this.displayDialog = false;
      });
  }

  delete() {
    this.service
      .deleteOutputLocation(this.ComponentOutputLocation)
      .subscribe(x => {
        let index = this.ComponentOutputLocations.indexOf(this.ComponentOutputLocation);
        this.ComponentOutputLocations = this.ComponentOutputLocations.filter((val, i) => i != index);
        this.ComponentOutputLocation = null;
        this.displayDialog = false;
      });
  }

  onSelect() {
    this.newEntity = false;
    this.displayDialog = true;
  }

}
