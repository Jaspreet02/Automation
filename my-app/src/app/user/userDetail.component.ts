import { Component, OnInit } from '@angular/core';
import { User } from '../user';
import { UserService } from '../user.service';
import { Location } from '@angular/common';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { AppPage } from '../../../e2e/app.po';
import { NumberValueAccessor } from '@angular/forms/src/directives/number_value_accessor';
import { flattenStyles } from '@angular/platform-browser/src/dom/dom_renderer';
import { timingSafeEqual } from 'crypto';
import { GenericBrowserDomAdapter } from '@angular/platform-browser/src/browser/generic_browser_adapter';

@Component({
  selector: 'app-userDetail',
  templateUrl: './userDetail.component.html',
  styleUrls: ['./userDetail.component.css']
})

export class UserDetailComponent implements OnInit {

  userId: string;

  newUser: boolean;

  selectedUser: User;

  genders: SelectedItem[];

  gender: SelectedItem;

  constructor(private userService: UserService,
    private location: Location, private router: Router, private route: ActivatedRoute) {
    this.route.params.subscribe(res => {
      if (res['id']) {
        this.userId = res.id;
        this.newUser = false;
      }
      else {
        this.newUser = true;
      }
    });

    this.genders =[
      { Label:'Male', Value:'M' },
      { Label:'Female', Value:'F' },
      { Label:'Other', Value:'O' }
    ];

  }

  ngOnInit() {
    if (this.newUser) {
      this.selectedUser = new User();
    }
    else {
      this.getUser();
    }
  }

  getUser(): void {
    this.userService.getUser(this.userId)
      .subscribe(x => { this.selectedUser = x ;  this.gender = this.genders.find(g=> g.Value == this.selectedUser.Gender) });
  }

  save() {
    if (this.newUser) {
      this.selectedUser.Gender = this.gender.Value;
      this.userService.addUser(this.selectedUser).subscribe(x=> { this.selectedUser = null; this.router.navigate(['/users']); });
    } else {
      this.selectedUser.Gender = this.gender.Value;
      this.userService.updateUser(this.selectedUser).subscribe(x=> { this.selectedUser = null; this.router.navigate(['/users']); });
    }  
  }

  cancel() {
    this.selectedUser = null;
    this.location.back();
  }

}

interface SelectedItem {
  Label: string;
  Value: string
}
