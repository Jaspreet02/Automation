import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import { Client } from '../../shared/models/client';


const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable()

export class ClientService {

  private ClientUrl = 'http://127.0.0.1:8001/api/Client';  // URL to web api

  constructor(private http: HttpClient) { }

  getClients(pageNumber: number, pageSize: number, sortField: string, sortOrder: string, fetchAll: boolean = false): Observable<PagedResponse<Client>> {
    // return of(Users);
    return this.http.get<PagedResponse<Client>>(this.ClientUrl + '/Get?pageNumber=' + pageNumber + '&pageSize=' + pageSize + '&sortField=' + sortField + '&sortOrder=' + sortOrder + '&fetchAll=' + fetchAll);
  }

  /** POST: add a new user to the server */
  getClient(id: string): Observable<Client> {
    return this.http.get<Client>(this.ClientUrl + '/Get/' + id);
  }

  /** POST: add a new user to the server */
  addClient(client: Client): Observable<Client> {
    const url = `${this.ClientUrl}/Post`;
    return this.http.post<Client>(url, client, httpOptions);
  }

  /** DELETE: delete the user from the server */
  deleteClient(client: Client | number): Observable<Client> {
    const id = typeof client === 'number' ? client : client.ClientId;
    const url = `${this.ClientUrl}/Delete/${id}`;

    return this.http.delete<Client>(url, httpOptions);
  }

  /** PUT: update the user on the server */
  updateClient(client: Client): Observable<any> {
    const url = `${this.ClientUrl}/Put/${client.ClientId}`;
    return this.http.put(url, client, httpOptions);
  }

  isNameExist(name: string): Observable<any>{
    return this.http.get<Client>(this.ClientUrl + '/IsNameExist?name=' + name);
  }

}

export interface PagedResponse<T> {
  Count: number;
  Result: T[];
}
