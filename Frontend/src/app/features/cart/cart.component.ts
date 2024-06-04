import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NavbarComponent } from '../../core/navbar/navbar.component';
import { FooterComponent } from '../../core/footer/footer.component';
import { HttprequestService } from '../../core/services/httprequest.service';
import { Product } from '../../shared/models/product';

/////// NON TOCCARE KANE
@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [FormsModule, CommonModule, NavbarComponent, FooterComponent],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css'
})
export class CartComponent {
  cartProducts: Product[] = []
  cartList: any[] = []
  userId: number[] = []
  jwtToken: string | null = "";
  totalPrice: number = 0;

  constructor(private http: HttprequestService) {
    this.jwtToken = localStorage.getItem("jwtToken")
  }

  ngOnInit() {
    this.getDecodeToken();

    this.http.getCartProducts(this.userId).subscribe({
      next: (products) => {
        products.forEach((element: any) => {
          this.cartList.push(element)
          console.log(element)
        });

        this.cartList.forEach((element: any) => {

          this.http.getProductByID(element.productId).subscribe({
            next: ((product_name: any) => {
              this.cartProducts.push(product_name);
              this.totalPrice += product_name.listPrice        
            })
          })
        })
        console.log(this.cartProducts);
      }})
      console.log("Carrello ritirato con successo");
  }

  decodeBase64Url(str: string): string {
    // Replace non-URL-safe characters with URL-safe equivalents
    return decodeURIComponent(atob(str.replace(/-/g, '+').replace(/_/g, '/')));
  } parseJson(str: string): any {
    try {
      return JSON.parse(str);
    } catch (error) {
      throw new Error('Invalid JWT payload');
    }
  }
  splitToken(token: string | null): string[] {
    if (!token || token.length < 3) {
      throw new Error('Invalid JWT token');
    }
    return token.split('.');
  }

  // FUNZIONI PER LA DECODIFICA DEL TOKEN

  getDecodeToken() {
    try {
      const parts = this.splitToken(this.jwtToken);
      const decodedHeader = this.decodeBase64Url(parts[0]);
      const decodedPayload = this.parseJson(this.decodeBase64Url(parts[1]));

      this.http.getIdFromEmail(decodedPayload.unique_name)
        .subscribe({
          next: (response) => {
            this.userId.push(parseInt(response, 10))
          },
          error: (error) => {
            console.error('Error fetching ID:', error);
          }
        });

      // console.log('Header:', decodedHeader);
      // console.log('Payload:', decodedPayload);
      // Access specific claims from payload (e.g., username)
      const username = decodedPayload.sub; // Assuming 'sub' holds username
      // console.log('Username:', decodedPayload.unique_name);
    } catch (error) {
      console.error('Error decoding token:', error);
    }




  }

  //

}

