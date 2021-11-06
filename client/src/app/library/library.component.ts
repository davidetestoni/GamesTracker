import { Component, OnInit, Pipe } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { LibraryGameInfo } from '../_models/library-game-info';
import { AccountService } from '../_services/account.service';
import { LibraryService } from '../_services/library.service';

@Component({
  selector: 'app-library',
  templateUrl: './library.component.html',
  styleUrls: ['./library.component.css']
})
export class LibraryComponent implements OnInit {
  games: LibraryGameInfo[] = [];
  username: string = '';

  constructor(private libraryService: LibraryService, private route: ActivatedRoute,
    public accountService: AccountService) { }

  ngOnInit(): void {
    this.loadLibrary();
  }

  loadLibrary() {
    const username = this.route.snapshot.paramMap.get('username');
    if (username) {
      this.username = username;
      this.libraryService.getLibrary(username).subscribe(games => {
        this.games = games;
      });
    }
  }

}
