import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';

import { FileTransfer } from './fileTransfer';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable()

export class FileTransferService {

  private FileTransferUrl = 'http://localhost:5100/api/FileTransfer';  // URL to web api

  constructor(private http: HttpClient) { }

  getFileTransfers(pageNumber: number, pageSize: number, sortField: string, sortOrder: string, fetchAll: boolean = false): Observable<PagedResponse<FileTransfer>> {
    // return of(Users);
    return this.http.get<PagedResponse<FileTransfer>>(this.FileTransferUrl + '/Get?pageNumber=' + pageNumber + '&pageSize=' + pageSize + '&sortField=' + sortField + '&sortOrder=' + sortOrder + '&fetchAll=' + fetchAll);
  }

  /** POST: add a new user to the server */
  getFileTrasnfer(id: string): Observable<FileTransfer> {
    return this.http.get<FileTransfer>(this.FileTransferUrl + '/Get/' + id);
  }

  /** POST: add a new user to the server */
  addFileTransfer(fileTransfer: FileTransfer): Observable<FileTransfer> {
    const url = `${this.FileTransferUrl}/Post`;
    return this.http.post<FileTransfer>(url, fileTransfer, httpOptions);
  }

  /** DELETE: delete the user from the server */
  deleteFileTrasnfer(application: FileTransfer | number): Observable<FileTransfer> {
    const id = typeof application === 'number' ? application : application.FileTransferSettingId;
    const url = `${this.FileTransferUrl}/Delete/${id}`;

    return this.http.delete<FileTransfer>(url, httpOptions);
  }

  /** PUT: update the user on the server */
  updateFileTransfer(application: FileTransfer): Observable<any> {
    const url = `${this.FileTransferUrl}/Put/${application.FileTransferSettingId}`;
    return this.http.put(url, application, httpOptions);
  }

}

export interface PagedResponse<T> {
  Count: number;
  Result: T[];
}
