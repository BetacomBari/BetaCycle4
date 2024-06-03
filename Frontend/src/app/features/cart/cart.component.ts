import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NavbarComponent } from '../../core/navbar/navbar.component';
import { FooterComponent } from '../../core/footer/footer.component';
import { HttprequestService } from '../../core/services/httprequest.service';
import { Product } from '../../shared/models/product';
import { CartProduct } from '../../shared/models/cartProduct';
@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [FormsModule, CommonModule, NavbarComponent, FooterComponent],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css'
})
export class CartComponent {
products: Product[] =[]
cartList: any = []
cartItems: Product[] = []
userId: number = 1;
jwtToken: string | null = "";

constructor (private http: HttprequestService){
  this.jwtToken = localStorage.getItem("jwtToken")
}

// ngOnInit() {
//   this.http.getCartProducts(this.userId).subscribe({
//     next: (products: Product) => {
//       this.cartList = products;
//       console.log(this.cartList)
//     }
//   })
//   console.log("Carrello ritirato con successo");
//   console.log(this.cartList[0]);
// }
// ngOnInit(){
//   this.http.getCartProducts(this.userId).subscribe({
//     next: (products: Product[]) => {
//       this.cartList = products

//       const productRequest:any[] = [];

//       products.forEach(element => {
//         productRequest.push(this.http.getProductsForCart(element.productId))
//       })
//         })
//         console.log("Carrello ritirato con successo")
//         console.log(this.cartItems[1].name)
//       }

//   this.cartList.forEach(element => {
//     this.http.getProductsForCart(element.productId).subscribe({
//       next: (product: any) => {
//         this.cartItems.push(product)
//       }
//     })
// })
// }



ngOnInit(){
  this.getDecodeToken();
  this.http.getCartProducts(this.userId).subscribe({
    next: (products) => {
      this.cartList.push(products)
      this.cartList.forEach((element: any) => {
        console.log(element)
      });
      // console.log(this.cartList[0])
      const productRequest: any[] = [];

      products.forEach((element: any) => {
        productRequest.push(this.http.getProductsForCart(element.productId))
      })
      console.log("Carrello ritirato con successo")
    }
  })
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

  console.log('Header:', decodedHeader);
  console.log('Payload:', decodedPayload);

  // Access specific claims from payload (e.g., username)
  const username = decodedPayload.sub; // Assuming 'sub' holds username
  // console.log('Username:', decodedPayload.unique_name);
} catch (error) {
  console.error('Error decoding token:', error);
}
}

//

}

