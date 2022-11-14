import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Comment } from '../classes/comment';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class CommentService {

  private apiUrl: string = environment.apiUrl + "api/comment";

  constructor( 
    private httpClient: HttpClient
    ) { }

  getComments(gamekey: string): Observable<Comment[]> {
    return this.httpClient.get<Comment[]>(this.apiUrl + `/${gamekey}`);
  }

  getComment(gamekey: string, id: string): Observable<Comment> {
    return this.httpClient.get<Comment>(this.apiUrl + `/${gamekey}` + `/${id}`);
  }

  createComment(comment : Comment): Observable<Comment> {
    return this.httpClient.post<Comment>(this.apiUrl, comment);
  }

  editComment(comment : Comment): Observable<Comment> {
    return this.httpClient.put<Comment>(this.apiUrl, comment);
  }

  deleteComment(id : string): Observable<Comment> {
    return this.httpClient.delete<Comment>(this.apiUrl + `/${id}`);
  }
}