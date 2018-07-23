import { Component, OnInit } from '@angular/core';
import { User } from '../user';
import { UserService } from '../user.service';
import { Router } from '@angular/router';

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

  newUser: boolean;

  selectedUser: User;

  cols: any[];

  constructor(private router: Router, private userService: UserService) { }

  ngOnInit() {
    this.getUsers();
    this.cols = [
      { field: 'FirstName', header: 'First Name' },
      { field: 'LastName', header: 'Last Name' },
      { field: 'PhoneNumber', header: 'Phone Number' },
      { field: 'Gender', header: 'Gender' },
      {field: 'Email', header: 'Email'},
      {field: 'Status', header: 'Status'}
  ];
  }

  showDialogToAdd() {
    this.newUser = true;
    this.selectedUser = new User();
    this.router.navigate(['/user']);
  }

  save() {
    if (this.newUser) {
      this.userService.addUser(this.selectedUser).subscribe(user => this.Users.push(user));
    } else {
      this.userService.updateUser(this.selectedUser).subscribe();
      this.Users[this.findSelectedUserIndex()] = this.selectedUser;
    }
    this.selectedUser = null;
  }

  delete() {
    this.userService.deleteUser(this.selectedUser).subscribe();
    const index = this.findSelectedUserIndex();
    this.Users = this.Users.filter((val, i) => i !== index);
    this.selectedUser = null;
  }

  findSelectedUserIndex(): number {
    return this.Users.indexOf(this.selectedUser);
  }

  onSelect(): void {
    this.newUser = false;
    this.router.navigate(['/user/' + this.selectedUser.Id]);
  }

  getUsers(): void {
    debugger;
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
