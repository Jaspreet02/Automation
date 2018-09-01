import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Client } from '../../../shared/models/client';
import { ClientService } from '../../../core/services/client.service';
import { ConfirmationService } from 'primeng/api';
import { Alert } from '../../../../../node_modules/@types/selenium-webdriver';

@Component({
  selector: 'app-client',
  templateUrl: './client.component.html',
  styleUrls: ['./client.component.css'],
  providers: [ConfirmationService]
})

export class ClientComponent implements OnInit {

  Clients: Client[];

  _total: number;

  pageNumber: number = 0;

  pageSize: number = 5;

  sortF: string = 'CreatedAt';

  sortO: string;

  selectedClient: Client;


  constructor(private router: Router,private confirmationService: ConfirmationService, private clientService: ClientService) { }

  ngOnInit() {
    this.getClients();
  }

  showDialogToAdd() {
    this.router.navigate(['/client']);
  }

  onSelect(): void {
    this.router.navigate(['/client/' + this.selectedClient.ClientId]);
  }

  getClients(): void {
    this.clientService
      .getClients(this.pageNumber, this.pageSize, this.sortF, this.sortO == '1' ? 'asc' : 'desc')
      .subscribe(x => (this.Clients = x.Result, this._total = x.Count));
  }

  paginate(event) {
    this.pageNumber = event.page;
    this.pageSize = event.rows;
    this.getClients();
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
    this.getClients();
  }

  delete(clientId, index) {
    this.confirmationService.confirm({
      message: 'Do you want to delete this record?',
      header: 'Delete',
      icon: 'fa fa-info-circle',
      accept: () => {
        this.clientService.deleteClient(clientId).subscribe(x =>
          this.Clients = this.Clients.filter((val, i) => i != index)
        );
      },
      reject: () => {
      }
    });
  }
}
