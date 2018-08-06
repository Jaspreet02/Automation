import { Component, OnInit } from '@angular/core';
import { ApplicationComponent } from '../applicationComponent';
import { ApplicationComponentService } from '../applicationComponent.service';
import { Router, ActivatedRoute } from '@angular/router';
import { ComponentExe } from '../componentExe';
import { ComponentExeService } from '../componentExe.service';

@Component({
  selector: 'app-applicationComponent',
  templateUrl: './applicationComponent.component.html',
  styleUrls: ['./applicationComponent.component.css']
})

export class ApplicationComponentComponent implements OnInit {

  components: ComponentExe[];

  ApplicationComponents: ApplicationComponent[];

  ApplicationId: number;

  _total: number;

  pageNumber: number = 0;

  pageSize: number = 5;

  sortF: string = 'CreatedAt';

  sortO: string;

  selectedApplicationComponent: ApplicationComponent;

  constructor(private router: Router, private route: ActivatedRoute, private applicationComponentService: ApplicationComponentService, private componentExeService: ComponentExeService) {
    this.route.queryParams.subscribe(res => {
        this.ApplicationId = res['applicationId'];
    });
  }

  ngOnInit() {
    this.getApplicationComponents();
  }

  GetComponentbyId(id) {
    return this.components.find(x=> x.ComponentId == id).Name;
   };

  getApplicationComponents(): void {
    this.componentExeService.getComponentExes(0, 0, 'CreatedAt', 'desc', true).subscribe(c => {
    this.components = c.Result;
      this.applicationComponentService
        .getApplicationComponents(this.ApplicationId, this.pageNumber, this.pageSize, this.sortF, this.sortO == '1' ? 'asc' : 'desc')
        .subscribe(x => (this.ApplicationComponents = x.Result, this._total = x.Count));
    });
  }

  showDialogToAdd() {
    this.router.navigate(['/applicationComponent',{applicationId:this.ApplicationId,maxValue:this._total + 1}]);
  }

  onSelect(): void {
    this.router.navigate(['/applicationComponent/' + this.selectedApplicationComponent.ApplicationComponentId, {applicationId: this.ApplicationId,maxValue:this._total + 1}]);
  }

  paginate(event) {
    this.pageNumber = event.page;
    this.pageSize = event.rows;
    this.getApplicationComponents();
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
    this.getApplicationComponents();
  }
}
