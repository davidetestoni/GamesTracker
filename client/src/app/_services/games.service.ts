import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { GameDetails } from '../_models/game-details';
import { GameInfo } from '../_models/game-info';
import { PaginatedResults } from '../_models/pagination';

@Injectable({
  providedIn: 'root'
})
export class GamesService {
  baseUrl = environment.apiUrl;
  paginatedResult: PaginatedResults<GameInfo[]> = new PaginatedResults<GameInfo[]>();

  constructor(private http: HttpClient) { }

  searchGames(query: string, page?: number, itemsPerPage?: number) {
    let params = new HttpParams();
    if (page !== undefined && itemsPerPage !== undefined) {
      params = params.append('query', query);
      params = params.append('pageNumber', page?.toString());
      params = params.append('pageSize', itemsPerPage?.toString());
    }
    return this.http.get<GameInfo[]>(this.baseUrl + 'games/search', 
      { observe: 'response', params }).pipe(map(response => {
        if (response.body) {
          this.paginatedResult.result = response.body;
        }
        const pagination = response.headers.get('Pagination');
        if (pagination !== null) {
          this.paginatedResult.pagination = JSON.parse(pagination);
        }
        return this.paginatedResult;
      }));
  }

  getGame(id: number) {
    return this.http.get<GameDetails>(this.baseUrl + 'games/' + id);
  }
}
