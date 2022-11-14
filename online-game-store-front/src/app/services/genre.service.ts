import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Genre } from '../classes/genre';

@Injectable({
  providedIn: 'root',
})
export class GenreService {

  private apiUrl: string = environment.apiUrl + "api/genre";
  
  
  constructor( 
    private httpClient: HttpClient
    ) { }

  getGenres(): Observable<Genre[]> {
    return this.httpClient.get<Genre[]>(this.apiUrl);
  }

  getGenre(id: string): Observable<Genre> {
    return this.httpClient.get<Genre>(this.apiUrl + `/${id}`);
  }

  createGenre(genre : Genre): Observable<Genre> {
    return this.httpClient.post<Genre>(this.apiUrl, genre);
  }

  editGenre(genre : Genre): Observable<Genre> {
    return this.httpClient.put<Genre>(this.apiUrl, genre);
  }

  deleteGenre(id : string): Observable<Genre> {
    return this.httpClient.delete<Genre>(this.apiUrl + `/${id}`);
  }
}