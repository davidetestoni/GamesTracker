import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { GameDetails } from 'src/app/_models/game-details';
import { GamesService } from 'src/app/_services/games.service';

@Component({
  selector: 'app-game-details',
  templateUrl: './game-details.component.html',
  styleUrls: ['./game-details.component.css']
})
export class GameDetailsComponent implements OnInit {
  game: GameDetails | undefined = undefined;
  galleryOptions: NgxGalleryOptions[] = [];
  galleryImages: NgxGalleryImage[] = [];

  constructor(private gamesService: GamesService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.loadGame();
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

}
