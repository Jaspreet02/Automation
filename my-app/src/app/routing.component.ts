import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { UserComponent } from './user/user.component';
import { UserDetailComponent } from './user/userDetail.component'
import { LoginComponent } from './login/login.component';
import { ClientComponent } from './client/client.component';
import { ClientDetailComponent } from './client/clientDetail.component';
import { ApplicationComponent } from './application/application.component';
import { ApplicationDetailComponent } from './application/applicationDetail.component';
import { ApplicationFileComponent } from './application/applicationFile.component';
import { FileTransferComponent } from './fileTransfer/fileTransfer.component';
import { FileTransferDetailComponent } from './fileTransfer/fileTransferDetail.component';
import { ComponentExeComponent } from './componentExe/componentExe.component';
import { ComponentExeDetailComponent } from './componentExe/componentExeDetail.component';

const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'users', component: UserComponent },
  { path: 'user/:id', component: UserDetailComponent },
  { path: 'user', component: UserDetailComponent },
  { path: 'clients', component: ClientComponent },
  { path: 'client/:id', component: ClientDetailComponent },
  { path: 'client', component: ClientDetailComponent },
  { path: 'applications', component: ApplicationComponent },
  { path: 'application/:id', component: ApplicationDetailComponent },
  { path: 'applicationFile/:id', component: ApplicationFileComponent },
  { path: 'application', component: ApplicationDetailComponent },
  { path: 'fileTransfers', component: FileTransferComponent },
  { path: 'fileTransfer/:id', component: FileTransferDetailComponent },
  { path: 'fileTransfer', component: FileTransferDetailComponent },  
  { path: 'componentExes', component: ComponentExeComponent }, 
  { path: 'componentExe/:id', component: ComponentExeDetailComponent }, 
  { path: 'componentExe', component: ComponentExeDetailComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class RoutingModule { }