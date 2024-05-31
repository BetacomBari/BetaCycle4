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
cartList: any
cartItems: Product[] = []
userId: number = 1;

constructor (private http: HttprequestService){
  this.userId =  1
}

ngOnInit() {
  this.http.getCartProducts(this.userId).subscribe({
    next: (products: Product) => {
      this.cartList = products;
      console.log(this.cartList)
    }
  })
  console.log("Carrello ritirato con successo");
  console.log(this.cartItems[1]?.name);
}
// ngOnInit(){
  // this.http.getCartProducts(this.userId).subscribe({
  //   next: (products: Product[]) => {
  //     this.cartList = products

  //     const productRequest:any[] = [];

  //     products.forEach(element => {
  //       productRequest.push(this.http.getProductsForCart(element.productId))
  //     })
  //       })
  //       console.log("Carrello ritirato con successo")
  //       console.log(this.cartItems[1].name)
  //     }
  //   }

  // this.cartList.forEach(element => {
  //   this.http.getProductsForCart(element.productId).subscribe({
  //     next: (product: any) => {
  //       this.cartItems.push(product)
  //     }
  //   })

  };
