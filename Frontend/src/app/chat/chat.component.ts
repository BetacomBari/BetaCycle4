import { Component, viewChild } from '@angular/core';
import { NgModel } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ViewChild, ElementRef, AfterViewInit  } from '@angular/core';
import { HttprequestService } from '../core/services/httprequest.service';

@Component({
  selector: 'app-chat',
  standalone: true,
  imports:[FormsModule, CommonModule],
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent {
  data:any;
  constructor(private mainhttp: HttprequestService){}

 goToChat(){
  this.mainhttp.goToChat().subscribe({
    next: (Data: any) => {
      this.data = Data
      console.log(this.data);
    },
    error: (error: any) => {
      console.log(error);
    }
  })
 }
}

