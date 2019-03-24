import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { EmailTemplate } from '../../../shared/models/emailTemplate';
import { EmailTemplateService } from '../../../core/services/emailTemplate.service';
import { MasterService } from '../../../core/services/master.service';
import { ConfirmationService } from 'primeng/api';

@Component({
  selector: 'app-emailTemplate',
  templateUrl: './emailTemplate.component.html',
  styleUrls: ['./emailTemplate.component.css'],
  providers: [ConfirmationService]
})

export class EmailTemplateComponent implements OnInit {

  emailTemplates: EmailTemplate[];

  QueueTypes: any[];

  _total: number;

  pageNumber: number = 0;

  pageSize: number = 5;

  sortF: string = 'CreatedAt';

  sortO: string;

  selectedEmailTemplate: EmailTemplate;


  constructor(private router: Router,private confirmationService: ConfirmationService, private emailTemplateService: EmailTemplateService,private masterService: MasterService) { }

  ngOnInit() {    
    this.masterService.getQueueTypes().subscribe(c=> { this.QueueTypes = c ;
    this.getEmailTemplates() });
  }

  GetQueueTypebyId(id) {
   return this.QueueTypes.find(x=> x.QueueTypeId == id).Status;
  };

  showDialogToAdd() {
    this.router.navigate(['/' + localStorage.getItem('role') + '/email']);
  }

  onSelect(): void {
    this.router.navigate(['/' + localStorage.getItem('role') + '/email/' + this.selectedEmailTemplate.EmailTemplateId]);
  }

  getEmailTemplates(): void {
        this.emailTemplateService
          .getEmailTemplates(this.pageNumber, this.pageSize, this.sortF, this.sortO == '1' ? 'asc' : 'desc')
          .subscribe(x => (this.emailTemplates = x.Result, this._total = x.Count));
  }

  paginate(event) {
    this.pageNumber = event.page;
    this.pageSize = event.rows;
    this.getEmailTemplates();
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
    this.getEmailTemplates();
  }

  delete(fileId, index) {
    this.confirmationService.confirm({
      message: 'Do you want to delete this record?',
      header: 'Delete',
      icon: 'fa fa-info-circle',
      accept: () => {
        this.emailTemplateService.deleteEmailTemplate(fileId).subscribe(x =>
          this.emailTemplates = this.emailTemplates.filter((val, i) => i != index)
        );
      },
      reject: () => {
      }
    });
  }
}
