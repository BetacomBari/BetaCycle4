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
  jwtToken: string = "";
  userId: number[] = [];
  lastItemBoughtId: number = -1

  constructor(private mainhttp: HttprequestService){}

  ngOnInit(){
    this.getDecodeToken();
  }
    
  

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

      this.mainhttp.getIdFromEmail(decodedPayload.unique_name)
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

}
