import { Component, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { NavbarComponent } from '../../core/navbar/navbar.component';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { FooterComponent } from '../../core/footer/footer.component';
import { AuthService } from '../../core/services/auth.service';
import { HttprequestService } from '../../core/services/httprequest.service';
import { Product } from '../../shared/models/product';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [NavbarComponent, FormsModule, CommonModule, FooterComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class HomeComponent {
  products: Product[] = [];
  successMessage: string | null = null;
  
  constructor(private route: ActivatedRoute, private mainhttp: HttprequestService){}

  ngOnInit() {
    this.getLast12Product()
    this.route.queryParams.subscribe(params => {
      this.successMessage = params['message'];
      console.log(this.successMessage);
      
    });
  }

  getDecodedImage(thumbNailPhoto: string){
    return 'data:image/gif;base64,' + thumbNailPhoto;
  }

  getLast12Product(){
    this.mainhttp.getLast12Product().subscribe({
      next: (products: any) => {
        this.products = products
      },
      error: (error: any) => {
        console.log(error);
      }
    })
  }


}
