import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { ToastrService } from 'ngx-toastr';
import { GameDetails } from 'src/app/_models/game-details';
import { LibraryGameInfo } from 'src/app/_models/library-game-info';
import { GamesService } from 'src/app/_services/games.service';
import { LibraryService } from 'src/app/_services/library.service';

@Component({
  selector: 'app-game-details',
  templateUrl: './game-details.component.html',
  styleUrls: ['./game-details.component.css']
})
export class GameDetailsComponent implements OnInit {
  game: GameDetails | undefined = undefined;
  libraryGame: LibraryGameInfo | undefined = undefined;
  galleryOptions: NgxGalleryOptions[] = [];
  galleryImages: NgxGalleryImage[] = [];

  constructor(private gamesService: GamesService, private libraryService: LibraryService,
    private route: ActivatedRoute, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.loadGame();
    this.loadLibraryGame();
    this.galleryOptions = [
      {
        fullWidth: true,
        height: '800px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false
      }
    ];
  }

  getScreenshots(): NgxGalleryImage[] {
    if (this.game) {
      const imageUrls = [];
      for (const screenshot of this.game.screenshots) {
        imageUrls.push({
          small: screenshot?.mediumUrl,
          medium: screenshot?.hugeUrl,
          big: screenshot?.hugeUrl
        });
      }
      return imageUrls;
    }
    return [];
  }

  loadGame() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.gamesService.getGame(parseInt(id)).subscribe(game => {
        this.game = game;
        this.galleryImages = this.getScreenshots();
      });
    }
  }

  loadLibraryGame() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.libraryService.getGame(parseInt(id)).subscribe(libraryGame => {
        if (libraryGame) {
          this.libraryGame = libraryGame;
        }
      });
    }
  }

  addToLibrary() {
    if (this.game) {
      this.libraryService.addGame(this.game.id).subscribe(libraryGame => {
        this.toastr.success(`${libraryGame.name} was added to your library`);
        this.libraryGame = libraryGame;
      });
    }
  }

  removeFromLibrary() {
    if (this.libraryGame) {
      this.libraryService.removeGame(this.libraryGame.id).subscribe(() => {
        this.toastr.info(`${this.libraryGame?.name} was removed from your library`);
        this.libraryGame = undefined;
      });
    }
  }

}
