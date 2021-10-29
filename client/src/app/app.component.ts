import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { User } from './_models/user';
import { AccountService } from './_services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

// OnInit is a lifecycle interface
export class AppComponent implements OnInit {
  title = 'Games Tracker';
  users: any;

  // Use dependency injection to get an http client
  constructor(private accountService: AccountService) {}
  
  // This is like OnInitialized in blazor
  ngOnInit(): void {
    this.setCurrentUser();
  }

  setCurrentUser() {
    const userJson = localStorage.getItem('user');
    const user: User | undefined = userJson !== null ? JSON.parse(userJson) : undefined;
    this.accountService.setCurrentUser(user);
  }

}
