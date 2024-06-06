import { Component } from '@angular/core';
import { NavbarComponent } from '../../../core/navbar/navbar.component';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttprequestService } from '../../../core/services/httprequest.service';

@Component({
  selector: 'app-customer-new',
  standalone: true,
  imports: [NavbarComponent, FormsModule, CommonModule],
  templateUrl: './customer-new.component.html',
  styleUrl: './customer-new.component.css'
})
export class CustomerNewComponent {
  customers: any[] = []
  customerByID: any[] = []
  customerDelete:any
  deleteCustomerID: number | null = null; // ID del cliente da eliminare

  constructor(private mainhttp: HttprequestService) { }

  ngOnInit(){
    this.getCustomerNew()
    console.log(this.customers);
    
  }
  getCustomerNew() {
    this.mainhttp.getCustomerNew().subscribe({
      next: (Data: any) => {
        this.customers = Data
        console.log(this.customers);     
      },
      error: (error: any) => {
        console.log(error);
      }
    })
  }

  confirmDeleteCustomer(customerId: number) {
    // Imposta l'ID del cliente da eliminare
    this.deleteCustomerID = customerId;

    // Mostra una finestra di dialogo di conferma all'utente
    if (confirm("Sei sicuro di voler eliminare il Customer con id  " + customerId + "?")) {
      // Se l'utente conferma, elimina il cliente
      this.deleteCustomerNewByID(this.deleteCustomerID);
    } else {
      // Se l'utente annulla, resetta l'ID del cliente da eliminare
      this.deleteCustomerID = null;
    }
  }

  deleteCustomerNewByID(id:number){
    this.mainhttp.deleteCustomerNewByID(id).subscribe({
      next: (data: any) => {
      this.deleteCustomerID = null;
        
        window.location.reload();
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
