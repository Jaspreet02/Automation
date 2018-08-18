import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { ComponentInputLocation } from '../../../shared/models/componentInputLocation';
import { ApplicationComponentService } from '../../../core/services/applicationComponent.service';
@Component({
  selector: 'app-componentInputLocation',
  templateUrl: './componentInputLocation.component.html',
  styleUrls: ['./componentInputLocation.component.css']
})

export class ComponentInputLocationComponent implements OnInit {

  ComponentInputLocations: ComponentInputLocation[];

  ComponentInputLocation: ComponentInputLocation;

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
      .inputLocations(this.ApplicationId, this.ComponentId)
      .subscribe(x => (this.ComponentInputLocations = x));
  }

  showDialogToAdd() {
    this.newEntity = true;
    this.ComponentInputLocation = new ComponentInputLocation();
    this.ComponentInputLocation.ApplicationId = this.ApplicationId;
    this.ComponentInputLocation.ComponentId = this.ComponentId;
    this.displayDialog = true;
  }

  save() {
    let cars = [...this.ComponentInputLocations];

    this.service
      .addInputLocation(this.ComponentInputLocation)
      .subscribe(x => {
        if (this.newEntity)
          cars.push(this.ComponentInputLocation);
        else
          cars[this.ComponentInputLocations.indexOf(this.ComponentInputLocation)] = this.ComponentInputLocation;
        this.ComponentInputLocations = cars;
        this.ComponentInputLocation = null;
        this.displayDialog = false;
      });
  }

  delete() {
    this.service
      .deleteInputLocation(this.ComponentInputLocation)
      .subscribe(x => {
        let index = this.ComponentInputLocations.indexOf(this.ComponentInputLocation);
        this.ComponentInputLocations = this.ComponentInputLocations.filter((val, i) => i != index);
        this.ComponentInputLocation = null;
        this.displayDialog = false;
      });
  }

  onSelect() {
    this.newEntity = false;
    this.displayDialog = true;
  }

}
