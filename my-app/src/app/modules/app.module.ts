import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { DataTableModule, SharedModule, InputMaskModule, InputTextModule, PaginatorModule, DialogModule, ButtonModule, PanelModule, InputSwitchModule,ConfirmDialogModule } from 'primeng/primeng';
import {TableModule} from 'primeng/table';
import {SliderModule} from 'primeng/slider';
import {MultiSelectModule} from 'primeng/multiselect';
import {SpinnerModule} from 'primeng/spinner';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { UserComponent } from './components/user/user.component';
import { UserDetailComponent } from './components/user/userDetail.component';
import { LoginComponent } from './components/login/login.component';
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

import { UserService } from '../core/services/user.service';
import { ClientService } from '../core/services/client.service';
import { ApplicationService } from '../core/services/application.service';
import { FileTransferService } from '../core/services/fileTransfer.service';
import { ComponentExeService } from '../core/services/componentExe.service';
import { ApplicationComponentService } from '../core/services/applicationComponent.service';
import { RunDetailService } from '../core/services/runDetail.service';
import { MasterService } from '../core/services/master.service';
import { UploadFileService } from '../core/services/uploadFile.service';

import { AppHeaderComponent } from '../core/header/app-header/app-header.component';
import { AdminHeaderComponent } from '../core/header/admin-header/admin-header.component';
import { UserHeaderComponent } from '../core/header/user-header/user-header.component';
import { SuperHeaderComponent } from '../core/header/super-header/super-header.component';

import { RoutingModule } from './routing.component';
import { AuthGuard } from '../core/guards/auth.guard';
import { AuthInterceptor } from '../core/guards/auth.interceptor';

@NgModule({
  declarations: [
    AppComponent,
    UserComponent,
    UserDetailComponent,
    LoginComponent,
    ClientComponent,
    ClientDetailComponent,
    ApplicationComponent,
    ApplicationDetailComponent,
    ApplicationFileComponent,
    FileTransferComponent,
    FileTransferDetailComponent,
    ComponentExeComponent,
    ComponentExeDetailComponent,
    ApplicationComponentComponent,
    ApplicationComponentDetailComponent,
    ComponentInputLocationComponent,
    ComponentOutputLocationComponent,
    RunDetailComponent,
    UploadFileComponent,
    UploadFileDetailComponent,
    AppHeaderComponent,
    AdminHeaderComponent,
    UserHeaderComponent,
    SuperHeaderComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    RoutingModule,
    HttpClientModule,
    DataTableModule,
    SharedModule,
    PaginatorModule,
    DialogModule,
    InputMaskModule,
    InputTextModule,
    ButtonModule,
    PanelModule,
    SpinnerModule,
    BrowserAnimationsModule,
    InputSwitchModule,
    TableModule,
    MultiSelectModule,
    SliderModule,
    ConfirmDialogModule
  ],
  providers: [RunDetailService,UserService, ClientService, ApplicationService, FileTransferService,ComponentExeService,ApplicationComponentService, MasterService, UploadFileService, AuthGuard,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }],
  bootstrap: [AppComponent]
})
export class AppModule { }
