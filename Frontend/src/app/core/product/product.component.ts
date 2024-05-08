import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NavbarComponent } from '../navbar/navbar.component';
import { HttprequestService } from '../services/httprequest.service';
import { Product} from '../../shared/models/product';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
@Component({
  selector: 'app-product',
  standalone: true,
  imports: [RouterModule, NavbarComponent, CommonModule, FormsModule],
  templateUrl: './product.component.html',
  styleUrl: './product.component.css'
})
export class ProductComponent {
  product: any = {};
  productById: Product = new Product();
  constructor(private mainhttp: HttprequestService){}


  getProduct(){
    this.mainhttp.getProduct().subscribe({
      next: (Data: any) => {
        this.product = Data
        console.log(this.product);
      },
      error: (error: any) => {
        console.log(error);
      }
    })
  }
  getProductById(id:HTMLInputElement){
    this.mainhttp.getProductByID(parseInt(id.value)).subscribe({
      next: (Data: any) => {
        this.productById = Data
        console.log(this.productById);
        
      },
      error: (error: any) => {
        console.log(error);
      }
    })
  }
}
