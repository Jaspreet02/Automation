import  { NgModule  } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { UserComponent   } from './user/user.component';
import { UserDetailComponent } from './user/userDetail.component'
import { LoginComponent } from './login/login.component';
import { ClientComponent } from './client/client.component';
import { ClientDetailComponent } from './client/clientDetail.component';
import { ApplicationComponent } from './application/application.component';
import { ApplicationDetailComponent } from './application/applicationDetail.component';

const routes: Routes = [
    { path: '', redirectTo: '/login', pathMatch: 'full' },
    { path: 'login', component: LoginComponent},
    { path: 'users', component: UserComponent },
    { path: 'user/:id', component: UserDetailComponent },
    { path: 'user', component: UserDetailComponent },
    { path: 'clients', component: ClientComponent },
    { path: 'client/:id', component: ClientDetailComponent },
    {path: 'client', component: ClientDetailComponent},
    { path: 'applications', component: ApplicationComponent},
    { path: 'application/:id', component: ApplicationDetailComponent },
    {path: 'application', component: ApplicationDetailComponent}
  ];

  @NgModule({
    imports: [ RouterModule.forRoot(routes) ],
    exports: [ RouterModule ]
  })
  
  export class RoutingModule {}