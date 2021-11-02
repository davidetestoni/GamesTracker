import { Component, OnInit } from '@angular/core';
import { GameInfo } from 'src/app/_models/game-info';
import { GamesService } from 'src/app/_services/games.service';

@Component({
  selector: 'app-games-list',
  templateUrl: './games-list.component.html',
  styleUrls: ['./games-list.component.css']
})
export class GamesListComponent implements OnInit {
  games: GameInfo[] = [];
  model: GameSearch = { 
    query: ''
  };

  constructor(private gamesService: GamesService) { }

  ngOnInit(): void {
    this.searchGames('Wolfenstein');
  }

  search() {
    this.searchGames(this.model.query);
  }

  searchGames(query: string) {
    this.gamesService.searchGames(query).subscribe(games => {
      this.games = games;
    });
  }

}

interface GameSearch {
  query: string;
}
