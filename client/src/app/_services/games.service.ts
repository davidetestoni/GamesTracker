import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { GameDetails } from '../_models/game-details';
import { GameInfo } from '../_models/game-info';

@Injectable({
  providedIn: 'root'
})
export class GamesService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  searchGames(query: string) {
    return this.http.post<GameInfo[]>(this.baseUrl + 'games/search', {
      query: query,
      pageSize: 12,
      pageNumber: 1
    });
  }

  getGame(id: number) {
    return this.http.get<GameDetails>(this.baseUrl + 'games/' + id);
  }
}
