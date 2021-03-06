import { Component, OnInit } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import { UserService } from '../../../core/services/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  errorMessage : string;
  isLoginError : boolean = false;
  constructor(private userService : UserService,private router : Router) { }

  ngOnInit() {
  }

  OnSubmit(userName,password){
     this.userService.userAuthentication(userName,password).subscribe((data : any)=>{
      localStorage.setItem('userToken',data.access_token);
      localStorage.setItem('role',data.role);
      this.router.navigate(['/' + data.role + '/runDetails']);
    },
    (err : HttpErrorResponse)=>{
      this.errorMessage = err.error.error_description;
      this.isLoginError = true;
    });
  }
}
