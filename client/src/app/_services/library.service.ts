import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { LibraryGameInfo, UserGameStatus } from '../_models/library-game-info';

@Injectable({
  providedIn: 'root'
})
export class LibraryService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getGame(id: number) {
    return this.http.get<LibraryGameInfo | null>(this.baseUrl + 'library/games/' + id);
  }

  getLibrary(username: string) {
    return this.http.get<LibraryGameInfo[]>(this.baseUrl + 'library/' + username);
  }

  addGame(id: number) {
    // TODO: Let the user choose status etc. when adding the game to the library (show a modal)
    return this.http.post<LibraryGameInfo>(this.baseUrl + 'library/add-game', {
      id: id,
      status: UserGameStatus.Playing,
      finishedOn: null,
      userRating: null
    });
  }

  removeGame(id: number) {
    return this.http.delete(this.baseUrl + 'library/remove-game/' + id);
  }

  updateGame(game: LibraryGameInfo) {
    return this.http.put(this.baseUrl + 'library/update-game', {
      id: game.id,
      status: game.status,
      finishedOn: game.finishedOn,
      userRating: game.userRating
    });
  }
}
