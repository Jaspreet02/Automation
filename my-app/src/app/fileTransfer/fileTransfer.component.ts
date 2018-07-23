import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FileTransfer } from '../fileTransfer';
import { FileTransferService } from '../fileTransfer.service';
import { MasterService } from '../master.service';

@Component({
  selector: 'app-fileTransfer',
  templateUrl: './fileTransfer.component.html',
  styleUrls: ['./fileTransfer.component.css']
})

export class FileTransferComponent implements OnInit {

  FileTransfers: FileTransfer[];

  QueueTypes: any[];

  _total: number;

  pageNumber: number = 0;

  pageSize: number = 5;

  sortF: string = 'CreatedAt';

  sortO: string;

  newUser: boolean;

  selectedFileTransfer: FileTransfer;


  constructor(private router: Router, private fileTransferService: FileTransferService,private masterService: MasterService) { }

  ngOnInit() {
    this.getFileTransfers();
  }

  GetQueueTypebyId(id) {
   return this.QueueTypes.find(x=> x.QueueTypeId == id).Status;
  };

  showDialogToAdd() {
    this.newUser = true;
    this.selectedFileTransfer = new FileTransfer();
    this.router.navigate(['/fileTransfer']);
  }

  delete() {
    this.fileTransferService.deleteFileTrasnfer(this.selectedFileTransfer).subscribe();
    const index = this.findSelectedUserIndex();
    //this.FileTransfers = this.FileTransfers.filter((i) => i !== index);
    this.selectedFileTransfer = null;
  }

  findSelectedUserIndex(): number {
    return this.FileTransfers.indexOf(this.selectedFileTransfer);
  }

  onSelect(): void {
    this.newUser = false;
    this.router.navigate(['/fileTransfer/' + this.selectedFileTransfer.FileTransferSettingId]);
  }

  getFileTransfers(): void {
    this.masterService.getQueueTypes().subscribe(c=> { this.QueueTypes = c ;
        this.fileTransferService
          .getFileTransfers(this.pageNumber, this.pageSize, this.sortF, this.sortO == '1' ? 'asc' : 'desc')
          .subscribe(x => (this.FileTransfers = x.Result, this._total = x.Count));});
  }

  paginate(event) {
    this.pageNumber = event.page;
    this.pageSize = event.rows;
    this.getFileTransfers();
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
    this.getFileTransfers();
  }
}
