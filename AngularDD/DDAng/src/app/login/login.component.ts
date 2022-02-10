import { Component, OnInit } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(public auth: AuthService) { }

  logIn(){
    this.auth.loginWithRedirect();
  }

  logOut(){
    this.auth.logout();
  }

  loggedIn: boolean = false;


  ngOnInit(): void {
    this.auth.isAuthenticated$.subscribe((isLoggedIn) =>
    {
      this.loggedIn = isLoggedIn;
      console.log(isLoggedIn);
    })
    
  }

}
