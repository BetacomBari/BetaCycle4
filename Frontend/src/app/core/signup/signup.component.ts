import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NavbarComponent } from '../navbar/navbar.component';
import { CustomerRegister } from '../../shared/models/CustomerRegister';
import { Address } from '../../shared/models/Address';
import { CustomerAddress } from '../../shared/models/CustomerAddress';
import { Credentials } from '../../shared/models/Credentials';
import { HttprequestService } from '../services/httprequest.service';

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [RouterModule, NavbarComponent],
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.css'
})
export class SignupComponent {
  type: string = "password";
  isText: boolean = false;
  eyeIcon:string = "fa-eye-slash"

  constructor(private http: HttprequestService){}

  customerRegister: CustomerRegister = new CustomerRegister();
  // credentials: Credentials = new Credentials();
  // address: Address = new Address();
  // customerAddress: CustomerAddress = new CustomerAddress();

  hideShowPassword(){
    this.isText = !this.isText;
    this.isText ? this.eyeIcon = "fa-eye" : this.eyeIcon = "fa-eye-slash";
    this.isText ? this.type="text" : this.type = "password"
  }

  register(FirstName: HTMLInputElement, MiddleName: HTMLInputElement, LastName: HTMLInputElement, Phone: HTMLInputElement, AddressType: HTMLInputElement, EmailAddress: HTMLInputElement, Password: HTMLInputElement, AddressLine1: HTMLInputElement, AddressLine2: HTMLInputElement, City: HTMLInputElement, StateProvince: HTMLInputElement, CountryRegion: HTMLInputElement, PostalCode: HTMLInputElement)
  {
    //! customerNew
    this.customerRegister.FirstName = FirstName.value;
    this.customerRegister.MiddleName = MiddleName.value;
    this.customerRegister.LastName = LastName.value;
    this.customerRegister.Phone = Phone.value;

    //! AddressNew
    this.customerRegister.AddressLine1 = AddressLine1.value;
    this.customerRegister.AddressLine2 = AddressLine2.value;
    this.customerRegister.City = City.value;
    this.customerRegister.StateProvince = StateProvince.value;
    this.customerRegister.CountryRegion = CountryRegion.value;
    this.customerRegister.PostalCode = PostalCode.value;

    //! CustomerAddressNew
    this.customerRegister.AddressType = AddressType.value;

    //! Credentials
    this.customerRegister.EmailAddress = EmailAddress.value;
    this.customerRegister.Password = Password.value;
    

    this.http.register(this.customerRegister).subscribe({
      next: (custRegister: CustomerRegister) => {
        this.customerRegister = custRegister
        console.log(this.customerRegister);
        
      },
      error: (error: any) => {
        console.log(error);
      }
    })
  }

}
