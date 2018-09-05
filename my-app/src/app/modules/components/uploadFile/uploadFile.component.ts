import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ConfirmationService } from 'primeng/api';
import { UploadFile } from '../../../shared/models/uploadFile';
import { UploadFileService } from '../../../core/services/uploadFile.service';

@Component({
  selector: 'app-uploadFile',
  templateUrl: './uploadFile.component.html',
  styleUrls: ['./uploadFile.component.css'],
  providers: [ConfirmationService]
})

export class UploadFileComponent implements OnInit {

  UploadFiles: UploadFile[];

  _total: number;

  pageNumber: number = 0;

  pageSize: number = 5;

  sortF: string = 'CreatedAt';

  sortO: string;

  selectedUploadFile: UploadFile;


  constructor(private router: Router,private confirmationService: ConfirmationService, private service: UploadFileService) { }

  ngOnInit() {    
    this.gets();
  }

  showDialogToAdd() {
    this.router.navigate(['/uploadFile']);
  }

  onSelect(): void {
    this.router.navigate(['/uploadFile/' + this.selectedUploadFile.UploadFileId]);
  }

  gets(): void {
        this.service
          .gets(this.pageNumber, this.pageSize, this.sortF, this.sortO == '1' ? 'asc' : 'desc')
          .subscribe(x => (this.UploadFiles = x.Result, this._total = x.Count));
  }

  paginate(event) {
    this.pageNumber = event.page;
    this.pageSize = event.rows;
    this.gets();
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
    this.gets();
  }

  delete(fileId, index) {
    this.confirmationService.confirm({
      message: 'Do you want to delete this record?',
      header: 'Delete',
      icon: 'fa fa-info-circle',
      accept: () => {
        this.service.delete(fileId).subscribe(x =>
          this.UploadFiles = this.UploadFiles.filter((val, i) => i != index)
        );
      },
      reject: () => {
      }
    });
  }
}
