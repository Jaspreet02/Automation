import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { Validators, FormControl, FormGroup, FormBuilder, AbstractControl, ValidatorFn } from '@angular/forms';
import { ChangePasswordBindingModel } from '../../../shared/models/changePasswordBindingModel';
import { UserService } from '../../../core/services/user.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-updatePassword',
  templateUrl: './updatePassword.component.html',
  styleUrls: ['./updatePassword.component.css']
})

export class UpdatePasswordComponent implements OnInit {

  selectedPassword: ChangePasswordBindingModel;

  errorMessage : string;

  isError : boolean = false;

  passwordform: FormGroup;

  constructor(private router : Router,private userService: UserService, private fb: FormBuilder, private location: Location) {
  }

  ngOnInit() {
    this.selectedPassword = new ChangePasswordBindingModel();
    this.passwordform = this.fb.group({
      'oldpassword': new FormControl('', Validators.required),
      'newpassword': new FormControl('', Validators.compose([Validators.required, Validators.minLength(8)])),
      'confirmpassword': new FormControl('', Validators.compose([Validators.required, this.passwordConfirming()])),
    });
  }

  private passwordConfirming(): ValidatorFn {

    return (control: AbstractControl): { [key: string]: boolean } => {
      if (control.value != "") {
        if (control.value !== this.selectedPassword.NewPassword) {
          return { 'passwordMatch': true };
        }
        else {
          return null;
        }
      }
      else {
        return null;
      }
    }
  }

  save() {
    this.userService.changePassword(this.selectedPassword).subscribe(() => { 
      this.selectedPassword = null;    
      localStorage.removeItem('userToken');
      localStorage.removeItem('role');
      this.router.navigate(['/login']); 
    },
    (err : HttpErrorResponse)=>{
      this.errorMessage = "Error Message";
      this.isError = true;
    });
  }

  cancel() {
    this.location.back();
  }
}
