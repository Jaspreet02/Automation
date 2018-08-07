import { Component, OnInit } from '@angular/core';
import { Application } from '../application';
import { ApplicationService } from '../application.service';
import { Router } from '@angular/router';
import { Client } from '../client';
import { ClientService } from '../client.service';
import { RunDetail } from '../runDetail';
import { RunDetailService } from '../runDetail.service';
import { MasterService } from '../master.service';

@Component({
  selector: 'app-runDetail',
  templateUrl: './runDetail.component.html',
  styleUrls: ['./runDetail.component.css']
})

export class RunDetailComponent implements OnInit {

  RunDetails: RunDetail[];

  Applications: Application[];

  Clients: Client[];

  RunNumberStatuses: any[];

  _total: number;

  pageNumber: number = 0;

  pageSize: number = 5;

  sortF: string = 'CreatedAt';

  sortO: string;

  selectedRunDetail: RunDetail;


  constructor(private router: Router, private masterService: MasterService,private runDetailService: RunDetailService, private applicationService: ApplicationService, private clientService: ClientService) { }

  ngOnInit() {
    this.masterService.getRunNumerStatus().subscribe(x=> { this.RunNumberStatuses = x ;
      this.applicationService
        .getApplications(this.pageNumber, this.pageSize, this.sortF, this.sortO == '1' ? 'asc' : 'desc',true)
        .subscribe(x => { this.Applications = x.Result ; this.getRunDetails() });});    
  }

  GetApplicationbyId(id) {
   return this.Applications.find(x=> x.ApplicationId == id).Name;
  };

  GetRunNumberStatusbyId(id) {
    return this.RunNumberStatuses[id];
   };


  onSelect(): void {
  }

  getRunDetails(): void {
        this.runDetailService
          .getRunDetails(0,0,-1,this.pageNumber, this.pageSize, this.sortF, this.sortO == '1' ? 'asc' : 'desc')
          .subscribe(x => (this.RunDetails = x.Result, this._total = x.Count));
  }

  paginate(event) {
    this.pageNumber = event.page;
    this.pageSize = event.rows;
    this.getRunDetails();
    // event.first = Index of the first record
    // event.rows = Number of rows to display in new page
    // event.page = Index of the new page
    // event.pageCount = Total number of pages
  }

  changeSort(event) {
    if (!event.order) {
      this.sortF = 'CreatedAt';
      this.sortO = '-1';
    } else {
      this.sortF = event.field;
      this.sortO = event.order;
    }
    this.getRunDetails();
  }
}
