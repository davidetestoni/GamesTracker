import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { GameInfo } from 'src/app/_models/game-info';
import { LibraryService } from 'src/app/_services/library.service';

@Component({
  selector: 'app-game-card',
  templateUrl: './game-card.component.html',
  styleUrls: ['./game-card.component.css']
})
export class GameCardComponent implements OnInit {
  @Input() game: GameInfo | undefined = undefined;
  
  constructor(private libraryService: LibraryService, private toastr: ToastrService) { }

  ngOnInit(): void {
  }

  addToLibrary(game: GameInfo | undefined) {
    if (game) {
      this.libraryService.addGame(game.id).subscribe(libraryGame => {
        this.toastr.success(`${libraryGame.name} was added to your library`);
      });
    }
  }

}
