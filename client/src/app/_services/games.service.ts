import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { GameInfo } from '../_models/game-info';
import { User } from '../_models/user';

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
      },
      headers: new HttpHeaders({
        Authorization: this.getAuthHeader()
      })
    };
    return this.http.get<GameInfo[]>(this.baseUrl + 'games/search', httpOptions);
  }

  getGame(id: number) {
    const httpOptions = {
      headers: new HttpHeaders({
        Authorization: this.getAuthHeader()
      })
    };
    return this.http.get<GameInfo>(this.baseUrl + 'games/' + id, httpOptions);
  }

  private getAuthHeader(): string {
    const userJson = localStorage.getItem('user');
    const user: User | undefined = userJson !== null ? JSON.parse(userJson) : undefined;
    if (user) {
      return 'Bearer ' + user.token;
    }
    return '';
  }
}
