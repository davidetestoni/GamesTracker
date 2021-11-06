import { Component, Input, OnInit } from '@angular/core';
import { LibraryGameInfo } from 'src/app/_models/library-game-info';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-library-list',
  templateUrl: './library-list.component.html',
  styleUrls: ['./library-list.component.css']
})
export class LibraryListComponent implements OnInit {
  @Input() games: LibraryGameInfo[] = [];
  @Input() name: string = 'Library list';
  @Input() owner: string | undefined = undefined; // The username of the owner of this library
  
  constructor(public accountService: AccountService) { }

  ngOnInit(): void {
  }

  removeFromList(id: number) {
    this.games = this.games.filter(g => g.id !== id);
  }

}
