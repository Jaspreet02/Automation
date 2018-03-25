import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable } from 'rxjs/Observable';
import { of } from 'rxjs/observable/of';

import { User } from './user';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable()

export class UserService {

  private userUrl = 'http://127.0.0.1:8001/api/User';  // URL to web api

  constructor( private http: HttpClient) {}

  getUsers(pageNumber: number, pageSize: number): Observable<PagedResponse<User>> {
   // return of(Users);
   return this.http.get<PagedResponse<User>>(this.userUrl + '/Get?pageNumber=' + pageNumber + '&pageSize=' + pageSize);
  }

    /** POST: add a new user to the server */
    getUser (id: string): Observable<User> {
      return this.http.get<User>(this.userUrl + '/Get/' + id);
    }

  /** POST: add a new user to the server */
  addUser (user: User): Observable<User> {
    const url = `${this.userUrl}/Post?roleName=admin`;
    return this.http.post<User>(url, user, httpOptions);
  }

  /** DELETE: delete the user from the server */
  deleteUser (user: User | number): Observable<User> {
    const id = typeof user === 'number' ? user : user.Id;
    const url = `${this.userUrl}/Delete/${id}`;

    return this.http.delete<User>(url, httpOptions);
  }

  /** PUT: update the user on the server */
  updateUser (user: User): Observable<any> {
    const url = `${this.userUrl}/Put/${user.Id}`;
    return this.http.put(url, user, httpOptions);
  }

}

export interface PagedResponse<T> {
  Count: number;
  Result: T[];
}
