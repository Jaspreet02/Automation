import { Component, OnInit } from '@angular/core';
import { User } from '../user';
import { UserService } from '../user.service';
import { Router } from '@angular/router';
import { AppPage } from '../../../e2e/app.po';
import { NumberValueAccessor } from '@angular/forms/src/directives/number_value_accessor';
import { flattenStyles } from '@angular/platform-browser/src/dom/dom_renderer';
import { timingSafeEqual } from 'crypto';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})

export class UserComponent implements OnInit {

  Users: User[];

  _total: number;

  sortF: string;

  sortO: string;

  newUser: boolean;

  selectedUser: User;

  constructor(private router: Router, private userService: UserService) { }

  ngOnInit() {
    this.getUsers(0, 2);
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

  getUsers(pagenumber: number, pageSize: number): void {
    this.userService
      .getUsers(pagenumber, pageSize)
      .subscribe(x => (this.Users = x.Result, this._total = x.Count));
  }

  paginate(event) {
    this.getUsers(event.page, event.rows);
    // event.first = Index of the first record
    // event.rows = Number of rows to display in new page
    // event.page = Index of the new page
    // event.pageCount = Total number of pages
  }

  changeSort(event) {
    if (!event.order) {
      this.sortF = 'year';
    } else {
      this.sortF = event.field;
    }
  }
}
