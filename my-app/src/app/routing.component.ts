import  { NgModule  } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { UserComponent   } from './user/user.component';
import { UserDetailComponent } from './user/userDetail.component'

const routes: Routes = [
    { path: '', redirectTo: '/users', pathMatch: 'full' },
    { path: 'users', component: UserComponent },
    { path: 'user/:id', component: UserDetailComponent },
    { path: 'user', component: UserDetailComponent }
  ];

  @NgModule({
    imports: [ RouterModule.forRoot(routes) ],
    exports: [ RouterModule ]
  })
  
  export class RoutingModule {}