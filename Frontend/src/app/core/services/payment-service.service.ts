import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class PaymentServiceService {
    title = 'googlepaybtn';
  
    onLoadPaymentData(event: Event): void {
      const paymentData = (event as CustomEvent).detail;
  
      console.log('Data pagamento ricevuti:', paymentData);
  
      this.processPayment(paymentData);
    }
  
    private processPayment(paymentData: any): void {
      console.log('Elaborazione in corso:', paymentData);
      console.log('Pagamento OK');
    }
  }
  
