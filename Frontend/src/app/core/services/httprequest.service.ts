import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable, map } from 'rxjs';

import { User } from '../../shared/models/user';
import { AuthService } from './auth.service';

import { CustomerRegister } from '../../shared/models/CustomerRegister';
import { Credentials } from '../../shared/models/credentials';




@Injectable({
  providedIn: 'root'
})
export class HttprequestService {
  get(arg0: string) {
    throw new Error('Method not implemented.');
  }
  token: string = "niente";
  id:number = -1;
  constructor(private http: HttpClient, private auth: AuthService) {}

  loginPostJwt(credentials: Credentials): Observable<any>
  {
    return this.http.post(`https://localhost:7165/Login`, credentials, {observe: 'response'})
  }

  register(CustomerRegister: CustomerRegister): Observable<any>
  {
    return this.http.post(`https://localhost:7165/Register`, CustomerRegister,  {observe: 'response'})
  }

  getCustomer(): Observable<any> {  
    this.token = String(localStorage.getItem('jwtToken'));
    this.auth.setJwtLoginStatus(true, this.token)
    return this.http.get(`https://localhost:7165/api/Customers`, {headers: this.auth.authJwtHeader} )
  }

  getCustomerByID(id:number): Observable<any> {
    return this.http.get(`https://localhost:7165/api/Customers/${id}`)
  }

  getProduct(): Observable<any> {
    return this.http.get(`https://localhost:7165/api/Products`)
  }

  getLast12Product(): Observable<any> {
    return this.http.get(`https://localhost:7165/api/Products/GetProductsByPage`)
  }

  getProductByID(id:number): Observable<any> {
    return this.http.get(`https://localhost:7165/api/Products/${id}`)
  }

  getRecommandations(product_id: number): Observable<any> {
    return this.http.get(`http://127.0.0.1:5000/api/recommendations/${product_id}`)
  } 

  getRecommandations2(): Observable<any> {
    return this.http.get(`http://127.0.0.1:5000/recom/`)
  } 

  goToChat():Observable<any>{
    return this.http.get(`http://127.0.0.1:5000/`)
  }


  getCartProducts(customerId: number[]): Observable<any> {
    return this.http.get(`https://localhost:7165/api/ShoppingCarts/${customerId}`)
  }

  getProductsForCart(productId: number): Observable<any>{
    return this.http.get(`https://localhost:7165/api/Products/${productId}`)
  }

  getIdFromEmail(email: string): Observable<any>{
    return this.http.get(`https://localhost:7165/api/ShoppingCarts/${email}/1`)
  }
  
}
