import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GameDetails } from 'src/app/_models/game-details';
import { GamesService } from 'src/app/_services/games.service';

@Component({
  selector: 'app-game-details',
  templateUrl: './game-details.component.html',
  styleUrls: ['./game-details.component.css']
})
export class GameDetailsComponent implements OnInit {
  game: GameDetails | undefined = undefined;

  constructor(private gamesService: GamesService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.loadGame();
  }

  loadGame() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.gamesService.getGame(parseInt(id)).subscribe(game => this.game = game);
    }
  }

}
