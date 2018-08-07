import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import { ApplicationComponent } from './ApplicationComponent';
import { ComponentInputLocation } from './componentInputLocation';
import { ComponentOutputLocation } from './componentOutputLocation';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable()

export class ApplicationComponentService {

  private ApplicationComponentUrl = 'http://localhost:5100/api/AppComponent';  // URL to web api

  constructor(private http: HttpClient) { }

  getApplicationComponents(applicationId: number, pageNumber: number, pageSize: number, sortField: string, sortOrder: string, fetchAll: boolean = false): Observable<PagedResponse<ApplicationComponent>> {
    // return of(Users);
    return this.http.get<PagedResponse<ApplicationComponent>>(this.ApplicationComponentUrl + '/GetApplication?applicationId=' + applicationId + '&pageNumber=' + pageNumber + '&pageSize=' + pageSize + '&sortField=' + sortField + '&sortOrder=' + sortOrder + '&fetchAll=' + fetchAll);
  }

  /** POST: add a new user to the server */
  getApplicationComponent(id: string): Observable<ApplicationComponent> {
    return this.http.get<ApplicationComponent>(this.ApplicationComponentUrl + '/Get/' + id);
  }

  /** POST: add a new user to the server */
  addApplicationComponent(componentExe: ApplicationComponent): Observable<ApplicationComponent> {
    const url = `${this.ApplicationComponentUrl}/Post`;
    return this.http.post<ApplicationComponent>(url, componentExe, httpOptions);
  }

  /** DELETE: delete the user from the server */
  deleteApplicationComponent(componentExe: ApplicationComponent | number): Observable<ApplicationComponent> {
    const id = typeof componentExe === 'number' ? componentExe : componentExe.ApplicationComponentId;
    const url = `${this.ApplicationComponentUrl}/Delete/${id}`;

    return this.http.delete<ApplicationComponent>(url, httpOptions);
  }

  /** PUT: update the user on the server */
  updateApplicationComponent(componentExe: ApplicationComponent): Observable<any> {
    const url = `${this.ApplicationComponentUrl}/Put/${componentExe.ApplicationComponentId}`;
    return this.http.put(url, componentExe, httpOptions);
  }

  inputLocations(applicationId: number, componentId: number): Observable<ComponentInputLocation[]> {
    // return of(Users);
    return this.http.get<ComponentInputLocation[]>(this.ApplicationComponentUrl + '/InputLocations?appId=' + applicationId + '&compId=' + componentId);
  }

  /** POST: add a new user to the server */
  addInputLocation(entity: ComponentInputLocation): Observable<ComponentInputLocation> {
    const url = `${this.ApplicationComponentUrl}/AddInputLocation`;
    return this.http.post<ComponentInputLocation>(url, entity, httpOptions);
  }

  /** DELETE: delete the user from the server */
  deleteInputLocation(entity: ComponentInputLocation | number): Observable<ComponentInputLocation> {
    const id = typeof entity === 'number' ? entity : entity.ComponentInputLocationId;
    const url = `${this.ApplicationComponentUrl}/DeleteInputLocation/${id}`;

    return this.http.delete<ComponentInputLocation>(url, httpOptions);
  }

  outputLocations(applicationId: number, componentId: number): Observable<ComponentOutputLocation[]> {
    // return of(Users);
    return this.http.get<ComponentOutputLocation[]>(this.ApplicationComponentUrl + '/OutputLocations?appId=' + applicationId + '&compId=' + componentId);
  }

  /** POST: add a new user to the server */
  addOutputLocation(entity: ComponentOutputLocation): Observable<ComponentOutputLocation> {
    const url = `${this.ApplicationComponentUrl}/AddOutputLocation`;
    return this.http.post<ComponentOutputLocation>(url, entity, httpOptions);
  }

  /** DELETE: delete the user from the server */
  deleteOutputLocation(entity: ComponentOutputLocation | number): Observable<ComponentOutputLocation> {
    const id = typeof entity === 'number' ? entity : entity.ComponentOutputLocationId;
    const url = `${this.ApplicationComponentUrl}/DeleteOutputLocation/${id}`;

    return this.http.delete<ComponentOutputLocation>(url, httpOptions);
  }
}

export interface PagedResponse<T> {
  Count: number;
  Result: T[];
}
