import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import { UploadFile } from '../../shared/models/uploadFile';


const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable()

export class UploadFileService {

  private Url = 'http://127.0.0.1:8001/api/UploadFile';  // URL to web api

  constructor(private http: HttpClient) { }

  gets(pageNumber: number, pageSize: number, sortField: string, sortOrder: string, fetchAll: boolean = false): Observable<PagedResponse<UploadFile>> {
    // return of(Users);
    return this.http.get<PagedResponse<UploadFile>>(this.Url + '/Get?pageNumber=' + pageNumber + '&pageSize=' + pageSize + '&sortField=' + sortField + '&sortOrder=' + sortOrder + '&fetchAll=' + fetchAll);
  }

  /** POST: add a new user to the server */
  get(id: string): Observable<UploadFile> {
    return this.http.get<UploadFile>(this.Url + '/Get/' + id);
  }

  /** POST: add a new user to the server */
  add(fileTransfer: UploadFile): Observable<UploadFile> {
    const url = `${this.Url}/Post`;
    return this.http.post<UploadFile>(url, fileTransfer, httpOptions);
  }

  /** DELETE: delete the user from the server */
  delete(application: UploadFile | number): Observable<UploadFile> {
    const id = typeof application === 'number' ? application : application.UploadFileId;
    const url = `${this.Url}/Delete/${id}`;

    return this.http.delete<UploadFile>(url, httpOptions);
  }

  /** PUT: update the user on the server */
  update(application: UploadFile): Observable<any> {
    const url = `${this.Url}/Put/${application.UploadFileId}`;
    return this.http.put(url, application, httpOptions);
  }

}

export interface PagedResponse<T> {
  Count: number;
  Result: T[];
}
