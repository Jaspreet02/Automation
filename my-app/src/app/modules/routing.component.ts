import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { UserComponent } from './components/user/user.component';
import { UserDetailComponent } from './components/user/userDetail.component';
import { ClientComponent } from './components/client/client.component';
import { ClientDetailComponent } from './components/client/clientDetail.component';
import { ApplicationComponent } from './components/application/application.component';
import { ApplicationDetailComponent } from './components/application/applicationDetail.component';
import { ApplicationFileComponent } from './components/application/applicationFile.component';
import { FileTransferComponent } from './components/fileTransfer/fileTransfer.component';
import { FileTransferDetailComponent } from './components/fileTransfer/fileTransferDetail.component';
import { ComponentExeComponent } from './components/componentExe/componentExe.component';
import { ComponentExeDetailComponent } from './components/componentExe/componentExeDetail.component';
import { ApplicationComponentComponent } from './components/applicationComponent/applicationComponent.component';
import { ApplicationComponentDetailComponent } from './components/applicationComponent/applicationComponentDetail.component';
import { ComponentInputLocationComponent } from './components/applicationComponent/componentInputLocation.component';
import { ComponentOutputLocationComponent } from './components/applicationComponent/componentOutputLocation.component';
import { RunDetailComponent } from './components/Dashboard/runDetail.component';
import { UploadFileComponent } from './components/uploadFile/uploadFile.component';
import { UploadFileDetailComponent } from './components/uploadFile/uploadFileDetail.component';
import { AdminHeaderComponent } from '../core/header/admin-header/admin-header.component';
import { SuperHeaderComponent } from '../core/header/super-header/super-header.component';
import { UserHeaderComponent } from '../core/header/user-header/user-header.component';
import { UpdatePasswordComponent } from './components/user/updatePassword.component';

const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent }, 
  { path: 'superAdmin', component : SuperHeaderComponent,
  children : [
    { path: 'runDetails', component: RunDetailComponent},
    { path: 'users', component: UserComponent },
    { path: 'user/:id', component: UserDetailComponent },
    { path: 'user', component: UserDetailComponent },
    { path: 'clients', component: ClientComponent },
    { path: 'client/:id', component: ClientDetailComponent },
    { path: 'client', component: ClientDetailComponent },
    { path: 'applications', component: ApplicationComponent },
    { path: 'application/:id', component: ApplicationDetailComponent },
    { path: 'application', component: ApplicationDetailComponent },
    { path: 'applicationFile/:id', component: ApplicationFileComponent },
    { path: 'fileTransfers', component: FileTransferComponent },
    { path: 'fileTransfer/:id', component: FileTransferDetailComponent },
    { path: 'fileTransfer', component: FileTransferDetailComponent }, 
    { path: 'applicationComponents', component: ApplicationComponentComponent },
    { path: 'applicationComponent/:id', component: ApplicationComponentDetailComponent },
    { path: 'applicationComponent', component: ApplicationComponentDetailComponent },
    { path: 'inputLocations', component: ComponentInputLocationComponent},
    { path: 'outputLocations', component: ComponentOutputLocationComponent},
    { path: 'uploadFiles', component: UploadFileComponent},
    { path: 'uploadFile/:id', component: UploadFileDetailComponent },
    { path: 'uploadFile', component: UploadFileDetailComponent },
    { path: 'componentExes', component: ComponentExeComponent }, 
    { path: 'componentExe/:id', component: ComponentExeDetailComponent }, 
    { path: 'componentExe', component: ComponentExeDetailComponent },
    { path: 'changePassword', component: UpdatePasswordComponent }
  ]},
  { path: 'admin', component : AdminHeaderComponent,
    children : [
      { path: 'runDetails', component: RunDetailComponent},
      { path: 'users', component: UserComponent },
      { path: 'user/:id', component: UserDetailComponent },
      { path: 'user', component: UserDetailComponent },
      { path: 'clients', component: ClientComponent },
      { path: 'client/:id', component: ClientDetailComponent },
      { path: 'client', component: ClientDetailComponent },
      { path: 'applications', component: ApplicationComponent },
      { path: 'application/:id', component: ApplicationDetailComponent },
      { path: 'application', component: ApplicationDetailComponent },
      { path: 'applicationFile/:id', component: ApplicationFileComponent },
      { path: 'fileTransfers', component: FileTransferComponent },
      { path: 'fileTransfer/:id', component: FileTransferDetailComponent },
      { path: 'fileTransfer', component: FileTransferDetailComponent }, 
      { path: 'applicationComponents', component: ApplicationComponentComponent },
      { path: 'applicationComponent/:id', component: ApplicationComponentDetailComponent },
      { path: 'applicationComponent', component: ApplicationComponentDetailComponent },
      { path: 'inputLocations', component: ComponentInputLocationComponent},
      { path: 'outputLocations', component: ComponentOutputLocationComponent},
      { path: 'uploadFiles', component: UploadFileComponent},
      { path: 'uploadFile/:id', component: UploadFileDetailComponent },
      { path: 'uploadFile', component: UploadFileDetailComponent },
      { path: 'changePassword', component: UpdatePasswordComponent }
    ]},
    { path: 'user', component : UserHeaderComponent,
      children : [
        { path: 'runDetails', component: RunDetailComponent},
        { path: 'changePassword', component: UpdatePasswordComponent }
      ]}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class RoutingModule { }