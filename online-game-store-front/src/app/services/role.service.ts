import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Role } from '../classes/auth/role';

@Injectable({
  providedIn: 'root',
})
export class RoleService {

  private apiUrl: string = environment.apiUrl + "api/role";

  constructor( 
    private httpClient: HttpClient
    ) { }

  getRoles(): Observable<Role[]> {
    return this.httpClient.get<Role[]>(this.apiUrl);
  }
}