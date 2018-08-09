import { Component, OnInit } from '@angular/core';
import { Application } from '../application';
import { ApplicationService } from '../application.service';
import { Router } from '@angular/router';
import { Client } from '../client';
import { ClientService } from '../client.service';
import { RunDetail } from '../runDetail';
import { RunDetailService } from '../runDetail.service';
import { MasterService } from '../master.service';
import { SelectItem } from 'primeng/api';
import { SelectItemGroup } from 'primeng/api';

@Component({
  selector: 'app-runDetail',
  templateUrl: './runDetail.component.html',
  styleUrls: ['./runDetail.component.css']
})

export class RunDetailComponent implements OnInit {

  RunDetails: RunDetail[];

  Applications: Application[];

  Clients: Client[];

  RunNumberStatuses: RunNumber[];

  RunNumberStatus: RunNumber;

  _total: number;

  pageNumber: number = 0;

  pageSize: number = 5;

  sortF: string = 'CreatedAt';

  sortO: string;

  yearFilter: Date;

  cols: any[];

  selectedApplication: number;

  groupedApplications: SelectItemGroup[];

  selectedRunDetail: RunDetail;

  constructor(private router: Router, private masterService: MasterService, private runDetailService: RunDetailService, private applicationService: ApplicationService, private clientService: ClientService) {
    this.RunNumberStatuses = [
      { Status: 'All', Id: -1 },
      { Status: 'Running', Id: 1 },
      { Status: 'Error', Id: 2 },
      { Status: 'Complete', Id: 3 }
    ];
  }

  ngOnInit() {
    this.clientService.getClients(this.pageNumber, this.pageSize, this.sortF, this.sortO == '1' ? 'asc' : 'desc', true)
      .subscribe(x => {
        this.Clients = x.Result;
        this.applicationService
          .getApplications(this.pageNumber, this.pageSize, this.sortF, this.sortO == '1' ? 'asc' : 'desc', true)
          .subscribe(x => { this.Applications = x.Result; this.BindDropdown(); this.getRunDetails() });
      });

    this.cols = [
      { field: 'ApplicationId', header: 'Application' },
      { field: 'RunNumber', header: 'RunNumber' },
      { field: 'RunNumberStatusId', header: 'Status' },
      { field: 'CreatedAt', header: 'Download At' }
    ];
  }

  BindDropdown() {
    let result = [];
    for (let index = 0; index < this.Clients.length; index++) {
      let subItem = [];
      let tempApplication = this.Applications.filter(x => x.ClientId == this.Clients[index].ClientId);
      if(tempApplication.length == 0)
      {
        continue;
      }
      for (let subIndex = 0; subIndex < tempApplication.length; subIndex++) {
        subItem[subIndex] = { label: tempApplication[subIndex].Name, value: tempApplication[subIndex].ApplicationId }
      }
      result[index] =
        {
          label: this.Clients[index].Name, value: this.Clients[index].Code,
          items: subItem
        };
    }
    this.groupedApplications = result;
  }

  GetApplicationbyId(id) {
    return this.Applications.find(x => x.ApplicationId == id).Name;
  };

  GetRunNumberStatusbyId(id) {
    if (id == 0) {
      id = 1;
    }
    return this.RunNumberStatuses.find(x => x.Id == id).Status;
  };


  onSelect(): void {
  }

  getRunDetails(): void {
    this.runDetailService
      .getRunDetails(0, this.selectedApplication != null ? this.selectedApplication : 0, this.RunNumberStatus != null ? this.RunNumberStatus.Id : -1, this.pageNumber, this.pageSize, this.sortF, this.sortO == '1' ? 'asc' : 'desc')
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

interface RunNumber {
  Status: string;
  Id: number;
}