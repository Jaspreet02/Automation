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
  { path: 'componentExe', component: ComponentExeDetailComponent },
  { path: 'applicationComponents', component: ApplicationComponentComponent },
  { path: 'applicationComponent/:id', component: ApplicationComponentDetailComponent },
  { path: 'applicationComponent', component: ApplicationComponentDetailComponent },
  { path: 'inputLocations', component: ComponentInputLocationComponent},
  { path: 'outputLocations', component: ComponentOutputLocationComponent},
  { path: 'runDetails', component: RunDetailComponent},
  { path: 'uploadFiles', component: UploadFileComponent},
  { path: 'uploadFile/:id', component: UploadFileDetailComponent },
  { path: 'uploadFile', component: UploadFileDetailComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class RoutingModule { }