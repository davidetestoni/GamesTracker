import { Component, OnInit } from '@angular/core';
import { GameDetails } from 'src/app/_models/game-details';

@Component({
  selector: 'app-game-details',
  templateUrl: './game-details.component.html',
  styleUrls: ['./game-details.component.css']
})
export class GameDetailsComponent implements OnInit {
  game: GameDetails | undefined = undefined;

  constructor() { }

  ngOnInit(): void {
  }

}
