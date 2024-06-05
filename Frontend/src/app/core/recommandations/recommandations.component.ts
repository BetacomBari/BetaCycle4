import { Component, Renderer2 } from '@angular/core';
import { NavbarComponent } from '../navbar/navbar.component';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttprequestService } from '../services/httprequest.service';
import { Observable } from 'rxjs';
import {forkJoin} from 'rxjs';
import e from 'express';


@Component({
  selector: 'app-recommandations',
  standalone: true,
  imports: [NavbarComponent, RouterModule, CommonModule, FormsModule ],
  templateUrl: './recommandations.component.html',
  styleUrl: './recommandations.component.css'
})
export class RecommandationsComponent {
  product:any = []
  product_id?:number
  jwtToken: string | null = "";
  userId: number[] = [];
  id: number = -1;
  cnn_id: number =  -1
  lastItemBoughtId: number = -1
  user_id_map : number[] = []
  myvar:any
  data:number = -1
  htmlToAdd:string = '';


  constructor(private mainhttp: HttprequestService){}

  ngOnInit(){
    this.getDecodeToken();
    // console.log("id:" + this.id)
    // console.log("UserId:")
    // console.log(this.userId)
    // console.log("find index of 12") 
    // console.log(this.userId.indexOf(13))   
    // console.log(this.userId.at(-1))   
    // console.log("Length: " +this.userId.length)
    // console.log("CnnId:")
    // console.log(this.lastItemBoughtId)
    console.log(this.product);
 
  }

  getRecommandForUserByLastItemBought(userId: number[]){
    this.id  = userId[0]
    this.mainhttp.getLastItemBoughtById(this.id).subscribe({
      next: (Data: any) => {
        this.lastItemBoughtId = Data
        console.log(this.lastItemBoughtId);
    }})

    this.mainhttp.getRecommandations(this.lastItemBoughtId).subscribe({
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
  async getDecodeToken() {
    this.jwtToken = localStorage.getItem("jwtToken")
    try {
      const parts = this.splitToken(this.jwtToken);
      const decodedHeader = this.decodeBase64Url(parts[0]);
      const decodedPayload = this.parseJson(this.decodeBase64Url(parts[1]));

      let id_dirty = await this.mainhttp.getIdFromEmailTry(decodedPayload.unique_name)
      this.id = parseInt(id_dirty.toString(), 10)
      console.log("id_dirty: " + this.id)
      

      this.mainhttp.getIdFromEmail(decodedPayload.unique_name)
        .subscribe({
          next: (response) => {          
            //this.id = response
            console.log("id: " + this.id)
            this.userId.push(parseInt(response, 10))
          },
          error: (error) => {
            console.error('Error fetching ID:', error);
          }
        });

      let id_cnn_dirty = await this.mainhttp.getCnnIdfromIdTry(this.id)
      this.cnn_id = parseInt(id_cnn_dirty.toString(), 10)
      console.log("cnn_id_dirty: " + this.cnn_id)
      
        this.mainhttp.getCnnIdfromId(this.id)
        .subscribe({
          next: (response) => {
            console.log("CnnId " +response);
            this.cnn_id = response
          },
          error: (error) => {
            console.error('Error fetching ID:', error);
          }
        });

        let id_last_item_dirty = await this.mainhttp.getLastItemBoughtByIdTry(this.cnn_id)
        this.lastItemBoughtId = parseInt(id_last_item_dirty.toString(), 10)
        console.log("id_last_item_dirty: " + id_last_item_dirty)


        this.mainhttp.getLastItemBoughtById(this.cnn_id)
        .subscribe({
          next: (resp) => {
            console.log("Item:")
            console.log(resp);  
            this.lastItemBoughtId = resp 
          },
          error: (error) => {
            console.error('Error fetching ID:', error);
          }
        });

        console.log("///// RECOM /////");

        this.mainhttp.getRecommandations(this.lastItemBoughtId).subscribe({
          next: (Data: any) => {
            console.log("Try to print");
            this.product = Data
            console.log("Recom for id: " + this.product_id)
            console.log(this.product);
            
            
            this.htmlToAdd = this.product;
            
  

            ///////////////
            return `
            <h1>product</h1>
            `
            
            ///////////////


          },
          error: (error: any) => {
            console.log(error);
          }
        })

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
