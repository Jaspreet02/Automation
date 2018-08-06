import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';

import { Application } from './application';
import { ApplicationFile } from './applicationFile';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable()

export class ApplicationService {

  private ApplicationUrl = 'http://localhost:5100/api/Application';  // URL to web api

  constructor(private http: HttpClient) { }

  getApplications(pageNumber: number, pageSize: number, sortField: string, sortOrder: string, fetchAll: boolean = false): Observable<PagedResponse<Application>> {
    // return of(Users);
    return this.http.get<PagedResponse<Application>>(this.ApplicationUrl + '/Get?pageNumber=' + pageNumber + '&pageSize=' + pageSize + '&sortField=' + sortField + '&sortOrder=' + sortOrder + '&fetchAll=' + fetchAll);
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

  getApplicationFiles(applicationId: number): Observable<ApplicationFile[]> {
    // return of(Users);
    return this.http.get<ApplicationFile[]>(this.ApplicationUrl + '/ApplicationFiles?applicationId=' + applicationId);
  }

  addApplicationFile(applicationFile: ApplicationFile): Observable<ApplicationFile> {
    const url = `${this.ApplicationUrl}/AddApplicationFile`;
    return this.http.post<ApplicationFile>(url, applicationFile, httpOptions);
  }

  deleteApplicationFile(applicationFile: ApplicationFile | number): Observable<ApplicationFile> {
    const id = typeof applicationFile === 'number' ? applicationFile : applicationFile.ApplicationFileId;
    const url = `${this.ApplicationUrl}/DeleteApplicationFile?applicationFileId=${id}`;
    return this.http.delete<ApplicationFile>(url, httpOptions);
  }

}

export interface PagedResponse<T> {
  Count: number;
  Result: T[];
}
