import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NavComponent } from './nav/nav.component';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { GamesListComponent } from './games/games-list/games-list.component';
import { GameDetailsComponent } from './games/game-details/game-details.component';
import { LibraryComponent } from './library/library.component';
import { SharedModule } from './_modules/shared.module';
import { TestErrorsComponent } from './errors/test-errors/test-errors.component';
import { ErrorInterceptor } from './_interceptors/error.interceptor';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { GameCardComponent } from './games/game-card/game-card.component';
import { JwtInterceptor } from './_interceptors/jwt.interceptor';
import { TextInputComponent } from './_forms/text-input/text-input.component';
import { LoadingInterceptor } from './_interceptors/loading.interceptor';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { LibraryListComponent } from './library/library-list/library-list.component';
import { FilterPipe } from './_pipes/filter-pipe';

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    HomeComponent,
    RegisterComponent,
    GamesListComponent,
    GameDetailsComponent,
    LibraryComponent,
    TestErrorsComponent,
    NotFoundComponent,
    ServerErrorComponent,
    GameCardComponent,
    TextInputComponent,
    ResetPasswordComponent,
    LibraryListComponent,
    FilterPipe
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule
  ],
  providers: [
    // multi: true adds the interceptor to the other ones, we don't want to replace them
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
