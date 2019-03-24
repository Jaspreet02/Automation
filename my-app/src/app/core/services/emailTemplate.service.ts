import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import { EmailTemplate } from '../../shared/models/emailTemplate';


const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable()

export class EmailTemplateService {

  private Url = 'http://127.0.0.1:8001/api/EmailTemplate';  // URL to web api

  constructor(private http: HttpClient) { }

  getEmailTemplates(pageNumber: number, pageSize: number, sortField: string, sortOrder: string, fetchAll: boolean = false): Observable<PagedResponse<EmailTemplate>> {
    // return of(Users);
    return this.http.get<PagedResponse<EmailTemplate>>(this.Url + '/Get?pageNumber=' + pageNumber + '&pageSize=' + pageSize + '&sortField=' + sortField + '&sortOrder=' + sortOrder + '&fetchAll=' + fetchAll);
  }

  /** POST: add a new user to the server */
  getEmailTemplate(id: string): Observable<EmailTemplate> {
    return this.http.get<EmailTemplate>(this.Url + '/Get/' + id);
  }

  /** POST: add a new user to the server */
  addEmailTemplate(emailTemplate: EmailTemplate): Observable<EmailTemplate> {
    const url = `${this.Url}/Post`;
    return this.http.post<EmailTemplate>(url, emailTemplate, httpOptions);
  }

  /** DELETE: delete the user from the server */
  deleteEmailTemplate(application: EmailTemplate | number): Observable<EmailTemplate> {
    const id = typeof application === 'number' ? application : application.EmailTemplateId;
    const url = `${this.Url}/Delete/${id}`;

    return this.http.delete<EmailTemplate>(url, httpOptions);
  }

  /** PUT: update the user on the server */
  updateEmailTemplate(application: EmailTemplate): Observable<any> {
    const url = `${this.Url}/Put/${application.EmailTemplateId}`;
    return this.http.put(url, application, httpOptions);
  }

}

export interface PagedResponse<T> {
  Count: number;
  Result: T[];
}
