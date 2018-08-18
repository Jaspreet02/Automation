import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { User } from '../../../shared/models/user';
import { UserService } from '../../../core/services/user.service';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})

export class UserComponent implements OnInit {

  Users: User[];

  _total: number;

  pageNumber: number = 0;

  pageSize: number = 5;

  sortF: string = 'CreatedAt';

  sortO: string ;

  selectedUser: User;

  constructor(private router: Router, private userService: UserService) { }

  ngOnInit() {
    this.getUsers();
  }

  showDialogToAdd() {
    this.router.navigate(['/user']);
  }

  onSelect(): void {
    this.router.navigate(['/user/' + this.selectedUser.Id]);
  }

  getUsers(): void {
    this.userService
      .getUsers(this.pageNumber, this.pageSize,this.sortF,this.sortO == '1' ? 'asc' : 'desc')
      .subscribe(x => (this.Users = x.Result, this._total = x.Count));
  }

  paginate(event) {
    this.pageNumber = event.page;
    this.pageSize = event.rows;
    this.getUsers();
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
    this.getUsers();
  }
}
