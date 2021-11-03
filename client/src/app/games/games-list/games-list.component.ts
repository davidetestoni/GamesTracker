import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { GameInfo } from 'src/app/_models/game-info';
import { Pagination } from 'src/app/_models/pagination';
import { BusyService } from 'src/app/_services/busy.service';
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
  lastQuery: string = '';
  model: GameSearch = { 
    query: ''
  };

  constructor(private gamesService: GamesService, private activatedRoute: ActivatedRoute,
    private router: Router, private busyService: BusyService) { }

  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe(params => {
      this.model.query = params.query || this.model.query;
      this.pageNumber = params.pageNumber || this.pageNumber;
      this.pageSize = params.pageSize || this.pageSize;
      this.search();
    });
  }

  search() {
    this.searchGames(this.model.query);
  }

  searchGames(query: string) {

    // Fixes a bug where it would endlessly loop through pages 1 and 2 and freeze the browser
    if (this.busyService.isBusy()) {
      return;
    }

    if (query !== this.lastQuery) {
      this.pageNumber = 1;
    }

    let queryParams = null;

    if (query) {
      this.gamesService.searchGames(query, this.pageNumber, this.pageSize).subscribe(games => {
        this.games = games.result;
        this.pagination = games.pagination;
        this.lastQuery = query;
      });
      queryParams = {
        query: query,
        pageNumber: this.pageNumber,
        pageSize: this.pageSize
      };
    }
    else {
      this.games = [];
      this.pagination = {
        currentPage: 1,
        itemsPerPage: 12,
        totalItems: 0,
        totalPages: 1
      };
    }

    // Update the URL
    this.router.navigate([], 
      {
        relativeTo: this.activatedRoute,
        queryParams: queryParams,
        queryParamsHandling: 'merge'
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
