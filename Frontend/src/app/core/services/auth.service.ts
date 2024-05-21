import { Injectable } from '@angular/core';
import { HttpHeaders } from '@angular/common/http';
import { LoginComponent } from '../login/login.component';


@Injectable({
  providedIn: 'root'
})

export class AuthService {
  private isLogged: boolean = false;

  authJwtHeader = new HttpHeaders({
    contentType: 'application/json',
    responseType: 'text'
  });
  


  setJwtLoginStatus(logValue: boolean, jwtToken: string) {
    this.isLogged = logValue;
    if (logValue) {
      localStorage.removeItem("jwtToken");
      localStorage.setItem('jwtToken', jwtToken);
      this.authJwtHeader = this.authJwtHeader.set(
        'Authorization',
        'Bearer ' + jwtToken
      );

    } else {
      localStorage.removeItem('jwtToken');
      this.authJwtHeader= new HttpHeaders({
        contentType: 'application/json',
        responseType: 'text'
      });
    }
  }

  isLoggedIn(): boolean {
    return localStorage.getItem('JwtToken') !== null;
  }
}
