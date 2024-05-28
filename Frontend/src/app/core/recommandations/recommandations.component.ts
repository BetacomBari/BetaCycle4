import { Component } from '@angular/core';
import { NavbarComponent } from '../navbar/navbar.component';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttprequestService } from '../services/httprequest.service';

@Component({
  selector: 'app-recommandations',
  standalone: true,
  imports: [NavbarComponent, RouterModule, CommonModule, FormsModule],
  templateUrl: './recommandations.component.html',
  styleUrl: './recommandations.component.css'
})
export class RecommandationsComponent {
  product:any = []
  product_id?:number

  constructor(private mainhttp: HttprequestService){}

  getRecommand(product_id:HTMLInputElement){
    this.product_id = parseInt(product_id.value)
    this.mainhttp.getRecommandations(this.product_id).subscribe({
      next: (Data: any) => {
        this.product = Data
        console.log("Recom for id: " + this.product_id)
        console.log(this.product);
      },
      error: (error: any) => {
        console.log(error);
      }
    })
  }

}
