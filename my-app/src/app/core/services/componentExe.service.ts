import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import { ComponentExe } from '../../shared/models/componentExe';
import { TriggerandStatusFile } from '../../shared/models/TriggerandStatusFile';


const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable()

export class ComponentExeService {

  private ComponentExeUrl = 'http://127.0.0.1:8001/api/Component';  // URL to web api

  constructor(private http: HttpClient) { }

  getComponentExes(pageNumber: number, pageSize: number, sortField: string, sortOrder: string, fetchAll: boolean = false): Observable<PagedResponse<ComponentExe>> {
    // return of(Users);
    return this.http.get<PagedResponse<ComponentExe>>(this.ComponentExeUrl + '/Get?pageNumber=' + pageNumber + '&pageSize=' + pageSize + '&sortField=' + sortField + '&sortOrder=' + sortOrder + '&fetchAll=' + fetchAll);
  }

  /** POST: add a new user to the server */
  getComponentExe(id: string): Observable<ComponentExe> {
    return this.http.get<ComponentExe>(this.ComponentExeUrl + '/Get/' + id);
  }

  /** POST: add a new user to the server */
  addComponentExe(componentExe: ComponentExe): Observable<ComponentExe> {
    const url = `${this.ComponentExeUrl}/Post`;
    return this.http.post<ComponentExe>(url, componentExe, httpOptions);
  }

  /** DELETE: delete the user from the server */
  deleteComponentExe(componentExe: ComponentExe | number): Observable<ComponentExe> {
    const id = typeof componentExe === 'number' ? componentExe : componentExe.ComponentId;
    const url = `${this.ComponentExeUrl}/Delete/${id}`;

    return this.http.delete<ComponentExe>(url, httpOptions);
  }

  /** PUT: update the user on the server */
  updateComponentExe(componentExe: ComponentExe): Observable<any> {
    const url = `${this.ComponentExeUrl}/Put/${componentExe.ComponentId}`;
    return this.http.put(url, componentExe, httpOptions);
  }

  addTriggerStatusFile(triggerStatusFile: TriggerandStatusFile): Observable<TriggerandStatusFile> {
    const url = `${this.ComponentExeUrl}/AddUpdateTriggerandStatusFile`;
    return this.http.post<TriggerandStatusFile>(url, triggerStatusFile, httpOptions);
  }

  applicationComponent(applicationId: number): Observable<ComponentExe[]> {
    // return of(Users);
    return this.http.get<ComponentExe[]>(this.ComponentExeUrl + '/ApplicationComponents?applicationId=' + applicationId);
  }
}

export interface PagedResponse<T> {
  Count: number;
  Result: T[];
}
