import { Component, Injector, inject } from '@angular/core';
import { GooglePayButtonModule } from '@google-pay/button-angular';
import { RouterOutlet } from '@angular/router';
import { NgModule } from '@angular/core';

@Component({
  selector: 'app-pay-with-google',
  standalone: true,
  imports: [ GooglePayButtonModule, RouterOutlet],
  templateUrl: './pay-with-google.component.html',
  styleUrl: './pay-with-google.component.css'
})




export class PayWithGoogleComponent {
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
