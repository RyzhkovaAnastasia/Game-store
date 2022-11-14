import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Shipper } from '../classes/shipper';

@Injectable({
  providedIn: 'root',
})
export class ShipperService {

  private apiUrl: string = environment.apiUrl + "api/shipper";

  constructor( 
    private httpClient: HttpClient
    ) { }

  getShippers(): Observable<Shipper[]> {
    return this.httpClient.get<Shipper[]>(this.apiUrl);
  }

  getShipper(id: string): Observable<Shipper> {
    return this.httpClient.get<Shipper>(this.apiUrl + `/${id}`);
  }
}