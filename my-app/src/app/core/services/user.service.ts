import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import { User } from '../../shared/models/user';


const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable()

export class UserService {

  private userUrl = 'http://127.0.0.1:8001/api/User';  // URL to web api

  constructor( private http: HttpClient) {}

  userAuthentication(userName: string, password: string):Observable<any>  {
    var data = "username=" + userName + "&password=" + password + "&grant_type=password";
    var reqHeader = new HttpHeaders({ 'Content-Type': 'application/x-www-urlencoded', 'No-Auth':'True'});
    return this.http.post('http://127.0.0.1:8001/token', data, { headers: reqHeader });
  }

  private serializeObj(obj) {
    var result = [];
    for (var property in obj)
        result.push(encodeURIComponent(property) + "=" + encodeURIComponent(obj[property]));

    return result.join("&");
}

  getUsers(pageNumber: number, pageSize: number,sortField: string, sortOrder: string): Observable<PagedResponse<User>> {
   // return of(Users);
   return this.http.get<PagedResponse<User>>(this.userUrl + '/Get?pageNumber=' + pageNumber + '&pageSize=' + pageSize + '&sortField=' + sortField + '&sortOrder=' + sortOrder);
  }

    /** POST: add a new user to the server */
    getUser (id: string): Observable<User> {
      return this.http.get<User>(this.userUrl + '/Get/' + id);
    }

  /** POST: add a new user to the server */
  addUser (user: User, role : string): Observable<User> {
    const url = `${this.userUrl}/Post?roleName=` + role;
    return this.http.post<User>(url, user, httpOptions);
  }

  /** DELETE: delete the user from the server */
  deleteUser (user: User | string): Observable<User> {
    const id = typeof user === 'string' ? user : user.Id;
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
