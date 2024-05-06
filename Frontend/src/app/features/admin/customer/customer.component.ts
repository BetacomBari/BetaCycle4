import { Component } from '@angular/core';
import { NavbarComponent } from '../../../core/navbar/navbar.component';
import { HttprequestService } from '../../../core/services/httprequest.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-customer',
  standalone: true,
  imports: [NavbarComponent, CommonModule, FormsModule],
  templateUrl: './customer.component.html',
  styleUrl: './customer.component.css'
})
export class CustomerComponent {
  customers: any[] = []
  customerByID: any = {}


  constructor(private mainhttp: HttprequestService) { }

  getCustomer() {
    this.mainhttp.getCustomer().subscribe({
      next: (Data: any) => {
        this.customers = Data
        console.log(this.customers);
        
      },
      error: (error: any) => {
        console.log(error);
      }
    })
  }

  getCustomerByID(input: HTMLInputElement) {
    this.mainhttp.getCustomerByID(parseInt(input.value)).subscribe({
      next: (Data: any) => {
        this.customerByID = Data
        console.log(this.customerByID);
        
      },
      error: (error: any) => {
        console.log(error);
      }
    })
  }

}
