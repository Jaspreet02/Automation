import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SelectItem } from 'primeng/api';
import { SelectItemGroup } from 'primeng/api';
import { RunDetail } from '../../../shared/models/runDetail';
import { ComponentExe } from '../../../shared/models/componentExe';
import { RunComponentStatus } from '../../../shared/models/runComponentStatus';
import { Application } from '../../../shared/models/application';
import { Client } from '../../../shared/models/client';
import { ComponentExeService } from '../../../core/services/componentExe.service';
import { MasterService } from '../../../core/services/master.service';
import { RunDetailService } from '../../../core/services/runDetail.service';
import { ApplicationService } from '../../../core/services/application.service';
import { ClientService } from '../../../core/services/client.service';

@Component({
  selector: 'app-runDetail',
  templateUrl: './runDetail.component.html',
  styleUrls: ['./runDetail.component.css']
})

export class RunDetailComponent implements OnInit {

  RunDetails: RunDetail[];

  ComponentExes: ComponentExe[];

  RunComponentStatuses: RunComponentStatus[];

  Applications: Application[];

  Clients: Client[];

  RunNumberStatuses: RunNumber[];

  ComponentStatuses: RunNumber[];

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

  loading: boolean;

  constructor(private router: Router, private componentService: ComponentExeService, private masterService: MasterService, private runDetailService: RunDetailService, private applicationService: ApplicationService, private clientService: ClientService) {

    this.RunNumberStatuses = [
      { Status: 'All', Id: -1 },
      { Status: 'Running', Id: 1 },
      { Status: 'Error', Id: 2 },
      { Status: 'Complete', Id: 3 }
    ];

    this.ComponentStatuses = [
      { Status: 'Ready', Id: 0 },
      { Status: 'Running', Id: 1 },
      { Status: 'Error', Id: 2 },
      { Status: 'Complete', Id: 3 },
      { Status: 'Optional', Id: 4 }
    ];

    setInterval(() => this.onRefreshPage(), 50000);
  }

  ngOnInit() {

    this.loading = true;

    this.clientService.getClients(this.pageNumber, this.pageSize, this.sortF, this.sortO == '1' ? 'asc' : 'desc', true)
      .subscribe(x => {
        this.Clients = x.Result;
        this.applicationService
          .getApplications(this.pageNumber, this.pageSize, this.sortF, this.sortO == '1' ? 'asc' : 'desc', true)
          .subscribe(x => { this.Applications = x.Result; this.BindDropdown(); this.getRunDetails() });
      });

    this.componentService.getComponentExes(0, 0, "CreatedAt", "desc", true)
      .subscribe(x => this.ComponentExes = x.Result);

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
      let tempApplication = this.Applications.filter(x => x.ClientId == this.Clients[index].ClientId);
      let subItem = [];
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

  GetComponentbyId(id) {
    return this.ComponentExes.find(x => x.ComponentId == id).Name;
  };

  GetRunNumberStatusbyId(id) {
    if (id == 0) {
      id = 1;
    }
    return this.RunNumberStatuses.find(x => x.Id == id).Status;
  };

  GetComponentStatusbyId(id) {
    return this.ComponentStatuses.find(x => x.Id == id).Status;
  };


  onRefreshPage(): void {
    this.runDetailService.getCount(this.selectedApplication != null ? this.selectedApplication : 0, this.RunNumberStatus != null ? this.RunNumberStatus.Id : -1)
      .subscribe(x => {
        if (this._total < x) {
          this.runDetailService
            .getRunDetails(0, this.selectedApplication != null ? this.selectedApplication : 0, this.RunNumberStatus != null ? this.RunNumberStatus.Id : -1, this.pageNumber, this.pageSize, this.sortF, this.sortO == '1' ? 'asc' : 'desc')
            .subscribe(x => (this.RunDetails = x.Result, this._total = x.Count));
        }
      });
  }

  getRunDetails(): void {
    this.loading = true;
    this.runDetailService
      .getRunDetails(0, this.selectedApplication != null ? this.selectedApplication : 0, this.RunNumberStatus != null ? this.RunNumberStatus.Id : -1, this.pageNumber, this.pageSize, this.sortF, this.sortO == '1' ? 'asc' : 'desc')
      .subscribe(x => (this.RunDetails = x.Result, this._total = x.Count, this.loading = false));
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