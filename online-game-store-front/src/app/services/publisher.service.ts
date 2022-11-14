import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

import { HttpClient } from '@angular/common/http';
import { Publisher } from '../classes/publisher';

@Injectable({
  providedIn: 'root',
})
export class PublisherService {

  private apiUrl: string = environment.apiUrl + "api/publisher";

  constructor( 
    private httpClient: HttpClient
    ) { }

  getPublishers(): Observable<Publisher[]> {
    return this.httpClient.get<Publisher[]>(this.apiUrl);
  }

  getPublisherByCompanyName(companyName: string): Observable<Publisher> {
    return this.httpClient.get<Publisher>(this.apiUrl + `/${companyName}`);
  }

  createPublisher(publisher : Publisher): Observable<Publisher> {
    return this.httpClient.post<Publisher>(this.apiUrl, publisher);
  }

  editPublisher(publisher : Publisher): Observable<Publisher> {
    return this.httpClient.put<Publisher>(this.apiUrl, publisher);
  }

  deletePublisher(id : string): Observable<Publisher> {
    return this.httpClient.delete<Publisher>(this.apiUrl + `/${id}`);
  }
}