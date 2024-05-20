import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NavbarComponent } from '../navbar/navbar.component';
import { Customer } from '../../shared/models/customer';
import { HttprequestService } from '../../shared/services/httprequest.service';


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
  eyeIcon:string = "fca-eye-slash"
  errorMessage: string[] = []
  isRegistered: boolean = false;
  customerRegister: CustomerRegister = new CustomerRegister();

  constructor(private mainhttp: HttprequestService) { }

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

    if (!this.verifyLength(MiddleName.value, 50, true)) {   
      this.errorMessage.push("Il secondo nome non può avere più di 50 caratteri.")
    }

    if (!this.verifyLength(LastName.value, 50, false)) {
      this.errorMessage.push("Il cognome è obbligatorio e non può avere più di 50 caratteri.")    
    }

    if (!this.verifyLength(Phone.value, 25, false)) {
      this.errorMessage.push("Il numero di telefono è obbligatorio e non può avere più di 25 caratteri.")    
    }

    if (!this.verifyLength(AddressType.value, 50, false)) {
      this.errorMessage.push("Il tipo di indirizzo è obbligatorio e non può avere più di 50 caratteri.")     
    }

    if (!this.isValidEmail(EmailAddress.value)) {
      this.errorMessage.push("L'email è obbligatoria e deve essere simile al formato indicato: 'example@example.com'")     
    }

    if (!this.verifyLength(AddressLine1.value, 60, false)) {
      this.errorMessage.push("L'indirizzo è obbligatorio e non può avere più di 60 caratteri.")
    }
    
    if (!this.verifyLength(AddressLine2.value, 60, true)) {
      this.errorMessage.push("Il secondo indirizzo non può avere più di 60 caratteri.")     
    }
    
    if (!this.verifyLength(City.value, 30, false)) {
      this.errorMessage.push("La città è obbligatoria e non può avere più di 30 caratteri.")    
    }
    
    if (!this.verifyLength(StateProvince.value, 50, false)) {
      this.errorMessage.push("La provincia è obbligatoria e non può avere più di 50 caratteri.")     
    }
    
    if (!this.verifyLength(CountryRegion.value, 50, false)) {
      this.errorMessage.push("La regione è obbligatoria e non può avere più di 50 caratteri.")     
    }
    
    if (!this.verifyLength(PostalCode.value, 15, false)) {
      this.errorMessage.push("Il CAP è obbligatorio e non può avere più di 15 caratteri.")  
    }
    
    if (!this.isValidPassword(Password.value)) {
      this.errorMessage.push("La password deve avere almeno 8 caratteri, deve contenere almeno un carattere maiuscolo un numero e deve contenere uno dei seguenti caratteri speciali: [!@#$%&?{}|<>].")    
    }

    if (!(Password.value == PasswordConfirm.value)) {
      this.errorMessage.push("Le password inserite non coincidono.")
    }

    if (this.errorMessage.length == 0) {
      
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
      
      this.http.register(this.customerRegister).subscribe(response =>{     
        console.log("ciao");
                
        if (response.status == 200) {
          console.log("Register ok");
          return this.isRegistered == true
        }else if (response.status == 400 || response.error.message == "emailExist"){
          console.log("Questa Email è già utilizzata.");
          return this.isRegistered == false  
        }else{
          return this.isRegistered == false

        }
      })

    }else{
      console.log("ciao");
      this.errorMessage.forEach(element => {
        console.log(element); 
        
      });
      return this.isRegistered == false  
    }
    return this.isRegistered == false  
  }
}
