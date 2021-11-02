import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { GameInfo } from '../_models/game-info';

@Injectable({
  providedIn: 'root'
})
export class GamesService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  searchGames(query: string) {
    const httpOptions = {
      params: {
        query: query
      }
    };
    return this.http.get<GameInfo[]>(this.baseUrl + 'games/search', httpOptions);
  }

  getGame(id: number) {
    return this.http.get<GameInfo>(this.baseUrl + 'games/' + id);
  }
}
