import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NavbarComponent } from '../../core/navbar/navbar.component';
import { FooterComponent } from '../../core/footer/footer.component';
import { ActivatedRoute } from '@angular/router';
import { HttprequestService } from '../../core/services/httprequest.service';

@Component({
  selector: 'app-product-detail',
  standalone: true,
  imports: [FormsModule,CommonModule,NavbarComponent,FooterComponent],
  templateUrl: './product-detail.component.html',
  styleUrl: './product-detail.component.css'
})
export class ProductDetailComponent {
  productId:number = 0
  product: any[] = []

  constructor(private route: ActivatedRoute, private mainhttp: HttprequestService){}

  ngOnInit(){
    this.route.queryParams.subscribe(params => {
      this.productId = params['message'];
    });
    console.log(this.productId);
    
    this.getProduct(this.productId)
  }

  getProduct(productId:number){
    this.mainhttp.getProductByID(productId).subscribe({
      next: (data: any) => {
        this.product = data
      },
      error: (error: any) => {
        console.log(error);
      }
    })
  }
}
