import { Component, OnInit} from '@angular/core';
import { Location } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { User } from '../../../shared/models/user';
import { UserService } from '../../../core/services/user.service';

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

  userform: FormGroup;

  roles = ['superAdmin', 'admin', 'user'];
  
  constructor(private userService: UserService,
    private location: Location, private router: Router, private route: ActivatedRoute, private fb: FormBuilder) {
    this.route.params.subscribe(res => {
      if (res['id']) {
        this.userId = res.id;
        this.newUser = false;
      }
      else {
        this.newUser = true;
      }
    });

    this.genders = [
      { Label: 'Male', Value: 'M' },
      { Label: 'Female', Value: 'F' },
      { Label: 'Other', Value: 'O' }
    ];

  }

  ngOnInit() {
    if (this.newUser) {
      this.selectedUser = new User();
    }
    else {
      this.getUser();
    }
    this.userform = this.fb.group({
      'firstname': new FormControl('', Validators.required),
      'lastname': new FormControl('', Validators.required),
      'phonenumber': new FormControl('', Validators.required),
      'email': new FormControl('', Validators.compose([Validators.required,Validators.email])),
      'gender': new FormControl('', Validators.required),
      'status': new FormControl('')
    });
  }

  getUser(): void {
    this.userService.getUser(this.userId)
      .subscribe(x => {
        this.selectedUser = x;
        this.gender = this.genders.find(g => g.Value == this.selectedUser.Gender);
      });
  }

  save() {
    if (this.newUser) {
      const role = this.roles[this.roles.indexOf(localStorage.getItem('role')) + 1];
      this.selectedUser.Gender = this.gender.Value;
      this.userService.addUser(this.selectedUser, role).subscribe(() => { this.selectedUser = null; this.router.navigate(['/' + localStorage.getItem('role') + '/users']); });
    } else {
      this.selectedUser.Gender = this.gender.Value;
      this.userService.updateUser(this.selectedUser).subscribe(() => { this.selectedUser = null; this.router.navigate(['/' + localStorage.getItem('role') + '/users']); });
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
