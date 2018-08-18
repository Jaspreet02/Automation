import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ComponentExe } from '../../../shared/models/componentExe';
import { TriggerandStatusFile } from '../../../shared/models/TriggerandStatusFile';
import { ComponentExeService } from '../../../core/services/componentExe.service';

@Component({
  selector: 'app-componentExe',
  templateUrl: './componentExe.component.html',
  styleUrls:['./componentExe.component.css']
})

export class ComponentExeComponent implements OnInit {

  ComponentExes: ComponentExe[];

  _total: number;

  pageNumber: number = 0;

  pageSize: number = 5;

  sortF: string = 'CreatedAt';

  sortO: string;

  displayDialog: boolean;

  selectedComponentExe: ComponentExe;

  triggerStatusFile: TriggerandStatusFile;

  constructor(private router: Router, private componentExeService: ComponentExeService) { }

  ngOnInit() {
    this.getComponentExes();
  }

  getComponentExes(): void {
    this.componentExeService
      .getComponentExes(this.pageNumber, this.pageSize, this.sortF, this.sortO == '1' ? 'asc' : 'desc')
      .subscribe(x => (this.ComponentExes = x.Result, this._total = x.Count));
  }

  showDialogToAdd() {
    this.selectedComponentExe = new ComponentExe();
    this.router.navigate(['/componentExe']);
  }

  showDialogToAddTriggerFile(id, item) {
    if (item == null) {
      this.triggerStatusFile = new TriggerandStatusFile();
      this.triggerStatusFile.ComponentId = id;
    }
    else {
      this.triggerStatusFile = item;
    }
    this.displayDialog = true;
  }

  save() {
    this.componentExeService.addTriggerStatusFile(this.triggerStatusFile)
      .subscribe(x => {
        this.triggerStatusFile = null;
        this.displayDialog = false;
      });
  }

  onSelect(): void {
    this.router.navigate(['/componentExe/' + this.selectedComponentExe.ComponentId]);
  }

  paginate(event) {
    this.pageNumber = event.page;
    this.pageSize = event.rows;
    this.getComponentExes();
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
    this.getComponentExes();
  }
}
