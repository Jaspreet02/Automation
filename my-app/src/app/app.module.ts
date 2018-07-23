import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule  } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import {DataTableModule, SharedModule,InputMaskModule, InputTextModule, PaginatorModule, DialogModule, ButtonModule,PanelModule  } from 'primeng/primeng';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { UserComponent } from './user/user.component';
import { UserDetailComponent } from './user/userDetail.component';
import { LoginComponent } from './login/login.component';
import {  ClientComponent } from './client/client.component';
import { ClientDetailComponent } from './client/clientDetail.component';
import { ApplicationComponent } from './application/application.component';
import { ApplicationDetailComponent } from './application/applicationDetail.component';
import { FileTransferComponent } from './fileTransfer/fileTransfer.component';
import { FileTransferDetailComponent } from './fileTransfer/fileTransferDetail.component';
import { UserService } from './user.service';
import  { ClientService } from './client.service';
import { ApplicationService } from './application.service';
import { FileTransferService } from './fileTransfer.service';
import { MasterService } from './master.service'

import { AppHeaderComponent } from './_layout/app-header/app-header.component';
import { AdminHeaderComponent } from './_layout/admin-header/admin-header.component';
import { UserHeaderComponent } from './_layout/user-header/user-header.component';
import { SuperHeaderComponent } from './_layout/super-header/super-header.component';

import { RoutingModule  } from './routing.component';
import { AuthGuard } from './auth/auth.guard';
import { AuthInterceptor } from './auth/auth.interceptor';

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
    FileTransferComponent,
    FileTransferDetailComponent,
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
    PanelModule ,
    BrowserAnimationsModule
  ],
  providers: [UserService,ClientService,ApplicationService,FileTransferService,MasterService,AuthGuard,,
    {
      provide : HTTP_INTERCEPTORS,
      useClass : AuthInterceptor,
      multi : true
    }],
  bootstrap: [AppComponent]
})
export class AppModule { }
