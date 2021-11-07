import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  navbarExpanded: boolean = false;

  // We need it to be public to access it in the template
  constructor(public accountService: AccountService, private router: Router,
    private toastr: ToastrService) {}

  ngOnInit(): void {
  }

  login() {
    this.accountService.login(this.model).subscribe(response => {
      this.router.navigateByUrl('/library');
    });
  }

  resetPassword() {
    this.router.navigateByUrl('/reset-password');
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }

  toggleNavbar() {
    this.navbarExpanded = !this.navbarExpanded;
  }

}
