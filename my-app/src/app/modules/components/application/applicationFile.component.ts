import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { ApplicationFile } from '../../../shared/models/applicationFile';
import { ApplicationService } from '../../../core/services/application.service';

@Component({
  selector: 'app-applicationFile',
  templateUrl: './applicationFile.component.html',
  styleUrls: ['./applicationFile.component.css']
})

export class ApplicationFileComponent implements OnInit {

  ApplicationFiles: ApplicationFile[];

  ApplicationFile: ApplicationFile;

  ApplicationId: number;

  displayDialog: boolean;

  newApplicationFile: boolean;

  constructor(private router: Router, private route: ActivatedRoute, private applicationService: ApplicationService) {
    this.route.params.subscribe(res => {
      if (res['id']) {
        this.ApplicationId = res.id;
      }
    });
  }

  ngOnInit() {
    this.applicationService
      .getApplicationFiles(this.ApplicationId)
      .subscribe(x => (this.ApplicationFiles = x));
  }

  showDialogToAdd() {
    this.newApplicationFile = true;
    this.ApplicationFile = new ApplicationFile();
    this.ApplicationFile.ApplicationId = this.ApplicationId;
    this.displayDialog = true;
  }

  save() {
    let cars = [...this.ApplicationFiles];

    this.applicationService
      .addApplicationFile(this.ApplicationFile)
      .subscribe(x => {
        if (this.newApplicationFile)
          cars.push(this.ApplicationFile);
        else
          cars[this.ApplicationFiles.indexOf(this.ApplicationFile)] = this.ApplicationFile;
        this.ApplicationFiles = cars;
        this.ApplicationFile = null;
        this.displayDialog = false;
      });
  }

  delete() {
    this.applicationService
      .deleteApplicationFile(this.ApplicationFile)
      .subscribe(x => {
        let index = this.ApplicationFiles.indexOf(this.ApplicationFile);
        this.ApplicationFiles = this.ApplicationFiles.filter((val, i) => i != index);
        this.ApplicationFile = null;
        this.displayDialog = false;
      });
  }

  onSelect() {
    this.newApplicationFile = false;
    this.displayDialog = true;
  }

}
