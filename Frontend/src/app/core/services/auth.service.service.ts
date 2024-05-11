import { Injectable } from '@angular/core';
import { HttpHeaders } from '@angular/common/http';


@Injectable({
  providedIn: 'root'
})

export class AuthService {
  private isLogged: boolean = false;
  authBasicHeader = new HttpHeaders({
    contentType: 'application/json',
    responseType: 'text'
  });

  authJwtHeader = new HttpHeaders({
    contentType: 'application/json',
    responseType: 'text'
  });

  setJwtLoginStatus(logValue: boolean, jwtToken: string='') {
    this.isLogged = logValue;
    if (logValue) {
      localStorage.setItem('jwtToken', jwtToken);
      this.authJwtHeader = this.authJwtHeader.set(
        'Authorization',
        'Bearer ' + jwtToken
      );
      console.log(this.authJwtHeader)
    } else {
      localStorage.removeItem('jwtToken');
      this.authJwtHeader= new HttpHeaders({
        contentType: 'application/json',
        responseType: 'text'
      });
    }
  }

  getLoginStatus() {
    return this.isLogged;
  }


}
