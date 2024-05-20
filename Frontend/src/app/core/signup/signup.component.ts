import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NavbarComponent } from '../navbar/navbar.component';
import { CustomerRegister } from '../../shared/models/CustomerRegister';
import { Address } from '../../shared/models/Address';
import { CustomerAddress } from '../../shared/models/CustomerAddress';
import { Credentials } from '../../shared/models/Credentials';
import { HttprequestService } from '../services/httprequest.service';
// KANEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE
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

  constructor(private http: HttprequestService){}

  hideShowPassword(){
    this.isText = !this.isText;
    this.isText ? this.eyeIcon = "fa-eye" : this.eyeIcon = "fa-eye-slash";
    this.isText ? this.type="text" : this.type = "password"
  }
  
  onSubmit(event: Event) {
    event.preventDefault(); // Previene il comportamento predefinito di submit del form
  }

  //! VERIFY
  isValidEmail(email: string): boolean {
    if (!email || email.trim() === '') {
      return false;
    }

    try {
      const emailRegex = /^[^@\s]+@[^@\s]+\.[^@\s]+$/;
      return emailRegex.test(email);
    } catch (e) {
      return false;
    }
  }

  verifyLength(input: string | null, maxLength: number, canBeEmpty: boolean): boolean {
    // Se l'input non può essere vuoto e l'input è null o vuoto, restituisce false
    if (!canBeEmpty && (!input || input.trim() === '')) {
      return false;
    }

    // Se l'input è null, significa che è accettabile (specialmente se può essere vuoto)
    if (input == null) {
      return true;
    }

    // Restituisce true se la lunghezza dell'input è minore o uguale a maxLength, altrimenti false
    return input.length <= maxLength;
  }

  isValidPassword(password: string): boolean {
    if (!password) {
      return false;
    }

    // Verifica che la password abbia almeno 8 caratteri
    if (password.length < 8) {
      return false;
    }

    // Verifica che la password contenga almeno un numero
    if (!/\d/.test(password)) {
      return false;
    }

    // Verifica che la password contenga almeno una lettera maiuscola
    if (!/[A-Z]/.test(password)) {
      return false;
    }

    // Verifica che la password contenga almeno un carattere speciale
    if (!/[!@#$%&?{}|<>.]/.test(password)) {
      return false;
    }

    return true;
  }
  //! END VERIFY


  register(FirstName: HTMLInputElement, MiddleName: HTMLInputElement, LastName: HTMLInputElement, Phone: HTMLInputElement, AddressType: HTMLInputElement, EmailAddress: HTMLInputElement, Password: HTMLInputElement, AddressLine1: HTMLInputElement, AddressLine2: HTMLInputElement, City: HTMLInputElement, StateProvince: HTMLInputElement, CountryRegion: HTMLInputElement, PostalCode: HTMLInputElement, PasswordConfirm:HTMLInputElement):boolean
  {
   this.errorMessage = [] 
    if (!this.verifyLength(FirstName.value, 50, false)) {      
      this.errorMessage.push("Il nome è obbligatorio e non può avere più di 50 caratteri.")
    }

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
      
      this.http.register(this.customerRegister).subscribe({
        next: (customerRegister: CustomerRegister) => {
          console.log("Register ok");
          return this.isRegistered == true
        },
        error: (error: any) => {
          if (error.error.message == "emailExist") {
            this.errorMessage.push("L'email inserita è già in uso.")
          }
          this.errorMessage.forEach(element => {
            console.log(element);     
          });
        }
      })


    }else{
      this.errorMessage.forEach(element => {
        console.log(element);     
      });
      return this.isRegistered == false  
    }
    return this.isRegistered == false  
  }
}
