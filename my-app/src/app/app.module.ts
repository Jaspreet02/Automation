import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import {DataTableModule, SharedModule, InputTextModule, PaginatorModule, DialogModule, ButtonModule } from 'primeng/primeng';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { UserComponent } from './user/user.component';
import { UserDetailComponent } from './user/userDetail.component';
import { UserService } from './user.service';
import { importType } from '@angular/compiler/src/output/output_ast';

import { RoutingModule  } from './routing.component';

@NgModule({
  declarations: [
    AppComponent,
    UserComponent,
    UserDetailComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    RoutingModule,
    HttpClientModule,
    DataTableModule,
    SharedModule,
    PaginatorModule,
    DialogModule,
    InputTextModule,
    ButtonModule,
    BrowserAnimationsModule
  ],
  providers: [UserService],
  bootstrap: [AppComponent]
})
export class AppModule { }
