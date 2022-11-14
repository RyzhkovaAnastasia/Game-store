import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Game } from '../classes/game';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { GameCacheService } from './game-cache.service';
import { GameFilterComponents } from '../classes/gameFilterComponents';
import { FilteredGameModel } from '../classes/filteredGameModel';

@Injectable({
  providedIn: 'root',
})
export class GameService {

  private apiUrl: string = environment.apiUrl + "api/game";

  constructor(
    private httpClient: HttpClient,
    private gameCacheService: GameCacheService
  ) { }

  getGameNumber(): Observable<number> {
    let gameNumber = this.gameCacheService.getValue();
    if(gameNumber == null)
    {
      let response = this.httpClient.get<number>(this.apiUrl + '/gameNumber');
      this.gameCacheService.setValue(response);
      return response;
    }
      return gameNumber;
  }

  getGames(): Observable<FilteredGameModel> {
    return this.httpClient.get<FilteredGameModel>(this.apiUrl);
  }

  filterGames(filter: string): Observable<FilteredGameModel> {
    return this.httpClient.get<FilteredGameModel>(this.apiUrl + '/filter/' + filter);
  }

  getGame(gamekey: string): Observable<Game> {
    return this.httpClient.get<Game>(this.apiUrl + `/${gamekey}`);
  }

  downloadGame(gamekey: string): Observable<HttpResponse<Blob>> {
    return this.httpClient.get(this.apiUrl + `/${gamekey}/download`,
      {
        responseType: 'blob',
        observe: 'response'
      });
  }

  createGame(game: Game): Observable<Game> {
    return this.httpClient.post<Game>(this.apiUrl, game);
  }

  editGame(game: Game): Observable<Game> {
    return this.httpClient.put<Game>(this.apiUrl, game);
  }

  editGameViewNumber(gameKey: string): Observable<Game> {
    return this.httpClient.put<Game>(this.apiUrl + `/incviewnumber/${gameKey}`, "");
  }

  deleteGame(id: string): Observable<any> {
    return this.httpClient.delete(this.apiUrl + `/${id}`);
  }
}