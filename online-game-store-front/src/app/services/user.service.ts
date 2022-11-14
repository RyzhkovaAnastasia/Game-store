import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { CommentBanDuration } from '../enums/commentBanDuration';
import { HttpClient } from '@angular/common/http';
import { Login } from 'src/app/classes/auth/login';
import { Register } from 'src/app/classes/auth/register';
import { tap } from 'rxjs';
import { Token } from '../classes/auth/token';
import { User } from '../classes/auth/user';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root',
})

export class UserService {
  public user: User;
  private apiUrl: string = environment.apiUrl + "api/user";
  private accessTokenKey: string = 'ACCESS_TOKEN_KEY';

  constructor( 
    private httpClient: HttpClient,
    private jwtHelper: JwtHelperService
    ) { 
      this.user = new User();
      if(this.user.id == '' || this.user == null)
      this.decodeUserToken();
    }

    public getAllUsers(): Observable<User[]>{
      return this.httpClient.get<User[]>(this.apiUrl);
    }

    public getUserByName(username: string): Observable<User>{
      return this.httpClient.get<User>(this.apiUrl + `/${username}`);
    }

    public editUser(user: User): Observable<User>{
      return this.httpClient.post<User>(this.apiUrl, user);
    }

  banUserComments(duration: CommentBanDuration): Observable<CommentBanDuration> {
    return this.httpClient.post<CommentBanDuration>(this.apiUrl + "/comments/ban", JSON.stringify(duration));
  }

  signIn(login: Login): Observable<Token> {
    return this.httpClient.post<Token>(this.apiUrl + "/auth/signin", login, { withCredentials: true })
    .pipe(
      tap(
      info => {
        this.initStorageInfo(info);
        this.decodeUserToken();
      }));
  }

  signUp(register: Register): Observable<Token> {
    return this.httpClient.post<Token>(this.apiUrl + "/auth/signup", register, { withCredentials: true })
    .pipe(
      tap(
      info => {
        this.initStorageInfo(info);
        this.decodeUserToken();
      }));
  }

  signOut(): void {
    localStorage.removeItem(this.accessTokenKey);
  }

  private decodeUserToken(): User {
      let token = localStorage.getItem(this.accessTokenKey);
      if(token !== null){
      let tokenInfo = this.jwtHelper.decodeToken(token);
      this.user.id = tokenInfo.sub;
      this.user.username = tokenInfo.unique_name;
      this.user.email = tokenInfo.email;
      this.user.role = tokenInfo.role;
      this.user.publisherId = tokenInfo.publisherId;
      return this.user;
    }
    return new User();
  }

  private initStorageInfo(token: Token) {
    localStorage.setItem(this.accessTokenKey, token.accessTokenKey);
  }

  public get isAuthenticated(): boolean {
    const token = localStorage.getItem(this.accessTokenKey);
    return token !== null && !this.jwtHelper.isTokenExpired(token);
  }

  public get getUser(): User {
     return this.decodeUserToken();
  }
}