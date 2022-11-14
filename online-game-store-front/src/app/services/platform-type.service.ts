import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { PlatformType } from '../classes/platformType';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class PlatformTypeService {

  private apiUrl: string = environment.apiUrl + "api/platformType";

  constructor( 
    private httpClient: HttpClient
    ) { }

  getPlatformTypes(): Observable<PlatformType[]> {
    return this.httpClient.get<PlatformType[]>(this.apiUrl);
  }

  getPlatformType(id: string): Observable<PlatformType> {
    return this.httpClient.get<PlatformType>(this.apiUrl + `/${id}`);
  }

  createPlatformType(platformType : PlatformType): Observable<PlatformType> {
    return this.httpClient.post<PlatformType>(this.apiUrl, platformType);
  }

  editPlatformType(platformType : PlatformType): Observable<PlatformType> {
    return this.httpClient.put<PlatformType>(this.apiUrl, platformType);
  }

  deletePlatformType(id : string): Observable<PlatformType> {
    return this.httpClient.delete<PlatformType>(this.apiUrl + `/${id}`);
  }
}