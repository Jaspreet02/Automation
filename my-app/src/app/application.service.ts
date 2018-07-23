import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';

import { Application } from './application';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable()

export class ApplicationService {

  private ApplicationUrl = 'http://127.0.0.1:8001/api/Application';  // URL to web api

  constructor(private http: HttpClient) { }

  getApplications(pageNumber: number, pageSize: number, sortField: string, sortOrder: string): Observable<PagedResponse<Application>> {
    // return of(Users);
    return this.http.get<PagedResponse<Application>>(this.ApplicationUrl + '/Get?pageNumber=' + pageNumber + '&pageSize=' + pageSize + '&sortField=' + sortField + '&sortOrder=' + sortOrder);
  }

  /** POST: add a new user to the server */
  getApplication(id: string): Observable<Application> {
    return this.http.get<Application>(this.ApplicationUrl + '/Get/' + id);
  }

  /** POST: add a new user to the server */
  addApplication(application: Application): Observable<Application> {
    const url = `${this.ApplicationUrl}/Post`;
    return this.http.post<Application>(url, application, httpOptions);
  }

  /** DELETE: delete the user from the server */
  deleteApplication(application: Application | number): Observable<Application> {
    const id = typeof application === 'number' ? application : application.ApplicationId;
    const url = `${this.ApplicationUrl}/Delete/${id}`;

    return this.http.delete<Application>(url, httpOptions);
  }

  /** PUT: update the user on the server */
  updateApplication(application: Application): Observable<any> {
    const url = `${this.ApplicationUrl}/Put/${application.ApplicationId}`;
    return this.http.put(url, application, httpOptions);
  }

}

export interface PagedResponse<T> {
  Count: number;
  Result: T[];
}
