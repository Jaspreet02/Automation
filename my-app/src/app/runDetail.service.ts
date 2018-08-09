import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import { RunDetail } from './runDetail';


const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable()

export class RunDetailService {

  private Url = 'http://127.0.0.1:8001/api/RunDetail';  // URL to web api

  constructor(private http: HttpClient) { }

  getRunDetails(clientId: number,appId: number,status: number,pageNumber: number, pageSize: number, sortField: string, sortOrder: string, fetchAll: boolean = false): Observable<PagedResponse<RunDetail>> {
    // return of(Users);
    return this.http.get<PagedResponse<RunDetail>>(this.Url + '/Get?clientId=' + clientId + '&appId=' + appId + '&status=' + status  + '&pageNumber=' + pageNumber + '&pageSize=' + pageSize + '&sortField=' + sortField + '&sortOrder=' + sortOrder + '&fetchAll=' + fetchAll);
  }
}

export interface PagedResponse<T> {
  Count: number;
  Result: T[];
}
