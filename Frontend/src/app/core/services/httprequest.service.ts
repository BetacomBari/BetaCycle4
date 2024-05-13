import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable } from 'rxjs';
import { Credientals } from '../../shared/models/credentials';
import { User } from '../../shared/models/user';
import { AuthService } from './auth.service';


@Injectable({
  providedIn: 'root'
})
export class HttprequestService {
  token: string = "niente";

  constructor(private http: HttpClient, private auth: AuthService) {}

  loginPostJwt(credentials: Credientals): Observable<any>
  {
    return this.http.post(`https://localhost:7165/Login`, credentials, {observe: 'response'})
  }

  postUser(user: User): Observable<any> {
    return this.http.post(`https://localhost:7165/api/Customers`, user)
  }

  getCustomer(): Observable<any> {  
    this.token = String(localStorage.getItem('jwtToken'));
    this.auth.setJwtLoginStatus(true, this.token)
    console.log(this.token);

    console.log(this.auth.authJwtHeader);
    
    return this.http.get(`https://localhost:7165/api/Customers`, {headers: this.auth.authJwtHeader} )
  }

  getCustomerByID(id:number): Observable<any> {
    return this.http.get(`https://localhost:7165/api/Customers/${id}`)
  }

  getProduct(): Observable<any> {
    return this.http.get(`https://localhost:7165/api/Products`)
  }

  getProductByID(id:number): Observable<any> {
    return this.http.get(`https://localhost:7165/api/Products/${id}`)
  }
}
