import { Component, OnInit } from '@angular/core';
import { GameInfo } from 'src/app/_models/game-info';
import { Pagination } from 'src/app/_models/pagination';
import { GamesService } from 'src/app/_services/games.service';

@Component({
  selector: 'app-games-list',
  templateUrl: './games-list.component.html',
  styleUrls: ['./games-list.component.css']
})
export class GamesListComponent implements OnInit {
  games: GameInfo[] = [];
  pagination: Pagination = {
    currentPage: 1,
    itemsPerPage: 12,
    totalItems: 0,
    totalPages: 1
  };
  pageNumber: number = 1;
  pageSize: number = 12;
  model: GameSearch = { 
    query: ''
  };

  constructor(private gamesService: GamesService) { }

  ngOnInit(): void {
    // TODO: Remove this in prod
    // -------------------------
    this.model.query = 'Wolfenstein';
    this.search();
    // -------------------------
  }

  search() {
    this.searchGames(this.model.query);
  }

  searchGames(query: string) {
    this.gamesService.searchGames(query, this.pageNumber, this.pageSize).subscribe(games => {
      this.games = games.result;
      this.pagination = games.pagination;
    });
  }

  pageChanged(event: any) {
    this.pageNumber = event.page;
    this.search();
  }

}

interface GameSearch {
  query: string;
}
