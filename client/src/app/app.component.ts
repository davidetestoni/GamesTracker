import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

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
  constructor(private http: HttpClient) {}
  
  // This is like OnInitialized in blazor
  ngOnInit(): void {
    this.getUsers();
  }

  getUsers() {
    this.http.get('https://localhost:5001/api/users').subscribe(response => {
      this.users = response;
    }, error => {
      console.log(error);
    });
  }
}
