import { Component, OnInit } from '@angular/core';
import { Application } from '../application';
import { ApplicationService } from '../application.service';
import { Router } from '@angular/router';
import { Client } from '../client';
import { ClientService } from '../client.service';

@Component({
  selector: 'app-application',
  templateUrl: './application.component.html',
  styleUrls: ['./application.component.css']
})

export class ApplicationComponent implements OnInit {

  Applications: Application[];

  Clients: Client[];

  _total: number;

  pageNumber: number = 0;

  pageSize: number = 5;

  sortF: string = 'CreatedAt';

  sortO: string;

  newUser: boolean;

  selectedApplication: Application;


  constructor(private router: Router, private applicationService: ApplicationService, private clientService: ClientService) { }

  ngOnInit() {
    this.getApplications();
  }

  GetClientbyId(id) {
   return this.Clients.find(x=> x.ClientId == id).Name;
  };

  showDialogToAdd() {
    this.newUser = true;
    this.selectedApplication = new Application();
    this.router.navigate(['/application']);
  }

  delete() {
    this.applicationService.deleteApplication(this.selectedApplication).subscribe();
    const index = this.findSelectedUserIndex();
    this.Applications = this.Applications.filter((val, i) => i !== index);
    this.selectedApplication = null;
  }

  findSelectedUserIndex(): number {
    return this.Applications.indexOf(this.selectedApplication);
  }

  onSelect(): void {
    this.newUser = false;
    this.router.navigate(['/application/' + this.selectedApplication.ApplicationId]);
  }

  getApplications(): void {
    this.clientService
      .getClients(this.pageNumber, this.pageSize, this.sortF, this.sortO == '1' ? 'asc' : 'desc',true)
      .subscribe(x => { this.Clients = x.Result ;
        this.applicationService
          .getApplications(this.pageNumber, this.pageSize, this.sortF, this.sortO == '1' ? 'asc' : 'desc')
          .subscribe(x => (this.Applications = x.Result, this._total = x.Count));});
  }

  paginate(event) {
    this.pageNumber = event.page;
    this.pageSize = event.rows;
    this.getApplications();
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
    this.getApplications();
  }
}
