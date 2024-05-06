import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NavbarComponent } from '../navbar/navbar.component';
import { Customer } from '../../shared/models/customer';

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

  customer: Customer = new Customer();

  hideShowPassword(){
    this.isText = !this.isText;
    this.isText ? this.eyeIcon = "fa-eye" : this.eyeIcon = "fa-eye-slash";
    this.isText ? this.type="text" : this.type = "password"
  }

  createCustomer(firstName: HTMLInputElement, middleName: HTMLInputElement, lastName: HTMLInputElement, email: HTMLInputElement, password: HTMLInputElement )
  {
    this.customer.firstName = firstName.value;
    this.customer.middleName = middleName.value;
    this.customer.lastName = lastName.value;
    this.customer.email = email.value;
    this.customer.password = password.value;
  }

}
