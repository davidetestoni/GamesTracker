import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GameDetailsComponent } from './games/game-details/game-details.component';
import { GamesListComponent } from './games/games-list/games-list.component';
import { HomeComponent } from './home/home.component';
import { LibraryComponent } from './library/library.component';
import { AuthGuard } from './_guards/auth.guard';

const routes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      { path: 'games', component: GamesListComponent },
      { path: 'games/:id', component: GameDetailsComponent },
      { path: 'library', component: LibraryComponent },
    ]
  },
  { path: '**', component: HomeComponent, pathMatch: 'full' } // Wildcard route
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
