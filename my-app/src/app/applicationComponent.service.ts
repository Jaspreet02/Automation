import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import { ApplicationComponent } from './ApplicationComponent';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable()

export class ApplicationComponentService {

  private ApplicationComponentUrl = 'http://localhost:5100/api/AppComponent';  // URL to web api

  constructor(private http: HttpClient) { }

  getApplicationComponents(applicationId: number,pageNumber: number, pageSize: number, sortField: string, sortOrder: string, fetchAll: boolean = false): Observable<PagedResponse<ApplicationComponent>> {
    // return of(Users);
    return this.http.get<PagedResponse<ApplicationComponent>>(this.ApplicationComponentUrl + '/GetApplication?applicationId=' + applicationId +'&pageNumber=' + pageNumber + '&pageSize=' + pageSize + '&sortField=' + sortField + '&sortOrder=' + sortOrder + '&fetchAll=' + fetchAll);
  }

  /** POST: add a new user to the server */
  getApplicationComponent(id: string): Observable<ApplicationComponent> {
    return this.http.get<ApplicationComponent>(this.ApplicationComponentUrl + '/Get/' + id);
  }

  /** POST: add a new user to the server */
  addApplicationComponent(componentExe: ApplicationComponent): Observable<ApplicationComponent> {
    const url = `${this.ApplicationComponentUrl}/Post`;
    return this.http.post<ApplicationComponent>(url, componentExe, httpOptions);
  }

  /** DELETE: delete the user from the server */
  deleteApplicationComponent(componentExe: ApplicationComponent | number): Observable<ApplicationComponent> {
    const id = typeof componentExe === 'number' ? componentExe : componentExe.ApplicationComponentId;
    const url = `${this.ApplicationComponentUrl}/Delete/${id}`;

    return this.http.delete<ApplicationComponent>(url, httpOptions);
  }

  /** PUT: update the user on the server */
  updateApplicationComponent(componentExe: ApplicationComponent): Observable<any> {
    const url = `${this.ApplicationComponentUrl}/Put/${componentExe.ApplicationComponentId}`;
    return this.http.put(url, componentExe, httpOptions);
  }
}

export interface PagedResponse<T> {
  Count: number;
  Result: T[];
}
