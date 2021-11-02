import { Component, Input, OnInit } from '@angular/core';
import { GameInfo } from 'src/app/_models/game-info';

@Component({
  selector: 'app-game-card',
  templateUrl: './game-card.component.html',
  styleUrls: ['./game-card.component.css']
})
export class GameCardComponent implements OnInit {
  @Input() game: GameInfo | undefined = undefined;
  
  constructor() { }

  ngOnInit(): void {
  }

}
