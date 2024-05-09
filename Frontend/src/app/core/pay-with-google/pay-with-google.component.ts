import { Component } from '@angular/core';
import { GooglePayButtonModule } from '@google-pay/button-angular';
import { RouterOutlet } from '@angular/router';


@Component({
  selector: 'app-pay-with-google',
  standalone: true,
  imports: [ GooglePayButtonModule, RouterOutlet],
  templateUrl: './pay-with-google.component.html',
  styleUrl: './pay-with-google.component.css'
})
export class PayWithGoogleComponent {

  //ngOnInit(){}
  constructor(){
    console.log("hi");
  }

  buttonWidth = 240
  paymentRequest: google.payments.api.PaymentDataRequest = {
    apiVersion:2,
    apiVersionMinor:0,
    allowedPaymentMethods:[
      {
        type:'CARD',
        parameters: {
          allowedAuthMethods: ["PAN_ONLY", "CRYPTOGRAM_3DS"],
          allowedCardNetworks: ["AMEX", "VISA", "MASTERCARD"]
        },
        tokenizationSpecification: {
          type: "PAYMENT_GATEWAY",
          parameters: {
            gateway:"example",
            gatewayMerchantId:"exampleGatewayMerchantId"
          }
        }
      }
    ],
    merchantInfo: {
      merchantId:"11111",
      merchantName: "Demo Merchant"
    },
    transactionInfo: {
      totalPriceStatus:"FINAL",
      totalPriceLabel:"TOTAL",
      totalPrice:"0.01",
      currencyCode:"USD",
      countryCode:"US"
    }
  }

  onLoadPaymentData(event:any){
    console.log(event, ">> Data");
  }

}
