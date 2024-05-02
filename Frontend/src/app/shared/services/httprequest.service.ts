import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Credientals } from '../models/credentials';
import { User } from '../models/user';


@Injectable({
  providedIn: 'root'
})
export class HttprequestService {

  constructor(private http: HttpClient) { }

  loginPost(credentials: Credientals): Observable<any>
  {
    return this.http.post(`https://localhost:7165/Login`, credentials, {observe: 'response'})
  }

  postUser(user: User): Observable<any> {
    return this.http.post(`https://localhost:7165/api/Customers`, user)
  }
}
