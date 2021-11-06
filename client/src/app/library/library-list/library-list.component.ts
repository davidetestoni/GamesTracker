import { Component, Input, OnInit } from '@angular/core';
import { LibraryGameInfo } from 'src/app/_models/library-game-info';

@Component({
  selector: 'app-library-list',
  templateUrl: './library-list.component.html',
  styleUrls: ['./library-list.component.css']
})
export class LibraryListComponent implements OnInit {
  @Input() games: LibraryGameInfo[] = [];
  @Input() name: string = 'Library list';
  
  constructor() { }

  ngOnInit(): void {
  }

  removeFromList(id: number) {
    this.games = this.games.filter(g => g.id !== id);
  }

}
