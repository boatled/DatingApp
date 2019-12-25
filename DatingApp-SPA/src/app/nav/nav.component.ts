import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};

  constructor(private authService: AuthService) {}

  ngOnInit() {}

  login() {
    // As the login method is returning observable, 'subscribe' the method.
    // In the subscribe method, we are using the overload
    // 'next' => If the login is successful
    // 'error' (Error Handlng) => Login failed
    this.authService.login(this.model).subscribe(next => {
      console.log('Logged in successfully');
    }, error => {
      console.log('Failed to Login');
    });
  }

  logout() {
    localStorage.removeItem('token');
    console.log('Logged Out');
  }

  loggedIn() {
    const token = localStorage.getItem('token');
    return !!token;
  }
}
