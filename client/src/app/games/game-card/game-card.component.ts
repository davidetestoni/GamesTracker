import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
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
  @Input() isLibraryGame: boolean = false; // If true, the add to list button will become a remove from list button
  @Output() removedFromLibrary = new EventEmitter();
  
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

  removeFromLibrary(game: GameInfo | undefined) {
    if (game) {
      this.libraryService.removeGame(game.id).subscribe(() => {
        this.removedFromLibrary.emit(game.id);
        this.toastr.info(`${game.name} was removed from your library`);
      });
    }
  }

}
