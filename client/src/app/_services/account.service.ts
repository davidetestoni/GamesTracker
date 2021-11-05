import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';

// Can be injected in other components or services via DI
@Injectable({
  providedIn: 'root' // It's a singleton
})
export class AccountService {
  baseUrl = environment.apiUrl;

  // It's a type of observable that keeps emitting the same object
  // the buffer has length 1 since we only have 1 thing to store.
  // We use an observable so that the auth guard can subscribe to this
  private currentUserSource = new ReplaySubject<User>(1);

  // By convention observables have a $ at the end
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient) { }

  login(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      // Anything inside this is an rxjs operator, it's basically like passing a delegate
      map((user: User) => {
        if (user) {
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource.next(user); // Set the next value of the replay subject
        }
      })
    );
  }

  requestPasswordReset(model: any) {
    return this.http.post(this.baseUrl + 'account/request-password-reset', model);
  }

  resetPassword(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/reset-password', model).pipe(
      map((user: User) => {
        if (user) {
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource.next(user);
        }
      })
    );
  }

  register(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/register', model).pipe(
      map((user: User) => {
        if (user) {
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource.next(user);
        }
      })
    );
  }

  setCurrentUser(user: User | undefined) {
    this.currentUserSource.next(user); // Set the next value of the replay subject
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(undefined);
  }
}
