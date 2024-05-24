import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { NavbarComponent } from '../navbar/navbar.component';
import { CustomerRegister } from '../../shared/models/CustomerRegister';
import { HttprequestService } from '../services/httprequest.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { FooterComponent } from '../footer/footer.component';
// KANEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE
@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [RouterModule, NavbarComponent, CommonModule, FormsModule, FooterComponent],
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.css'
})
export class SignupComponent {
  type: string = "password";
  isText: boolean = false;
  eyeIcon: string = "fca-eye-slash"
  errorMessage: string[] = []
  isRegistered: boolean = false;
  customerRegister: CustomerRegister = new CustomerRegister();

  constructor(private http: HttprequestService, private router: Router) { }

  hideShowPassword() {
    this.isText = !this.isText;
    this.isText ? this.eyeIcon = "fa-eye" : this.eyeIcon = "fa-eye-slash";
    this.isText ? this.type = "text" : this.type = "password"
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


  register(FirstName: HTMLInputElement, MiddleName: HTMLInputElement, LastName: HTMLInputElement, Phone: HTMLInputElement, AddressType: HTMLInputElement, EmailAddress: HTMLInputElement, Password: HTMLInputElement, AddressLine1: HTMLInputElement, AddressLine2: HTMLInputElement, City: HTMLInputElement, StateProvince: HTMLInputElement, CountryRegion: HTMLInputElement, PostalCode: HTMLInputElement, PasswordConfirm: HTMLInputElement): boolean {
    this.errorMessage = []
    if (!this.verifyLength(FirstName.value.trim(), 50, false)) {
      this.errorMessage.push("Il nome è obbligatorio e non può avere più di 50 caratteri.")
    }

    if (!this.verifyLength(MiddleName.value.trim(), 50, true)) {
      this.errorMessage.push("Il secondo nome non può avere più di 50 caratteri.")
    }

    if (!this.verifyLength(LastName.value.trim(), 50, false)) {
      this.errorMessage.push("Il cognome è obbligatorio e non può avere più di 50 caratteri.")
    }

    if (!this.verifyLength(Phone.value.trim(), 25, false)) {
      this.errorMessage.push("Il numero di telefono è obbligatorio e non può avere più di 25 caratteri.")
    }

    if (!this.verifyLength(AddressType.value.trim(), 50, false)) {
      this.errorMessage.push("Il tipo di indirizzo è obbligatorio e non può avere più di 50 caratteri.")
    }

    if (!this.isValidEmail(EmailAddress.value.trim())) {
      this.errorMessage.push("L'email è obbligatoria e deve essere simile al formato indicato: 'example@example.com'")
    }

    if (!this.verifyLength(AddressLine1.value.trim(), 60, false)) {
      this.errorMessage.push("L'indirizzo è obbligatorio e non può avere più di 60 caratteri.")
    }

    if (!this.verifyLength(AddressLine2.value.trim(), 60, true)) {
      this.errorMessage.push("Il secondo indirizzo non può avere più di 60 caratteri.")
    }

    if (!this.verifyLength(City.value.trim(), 30, false)) {
      this.errorMessage.push("La città è obbligatoria e non può avere più di 30 caratteri.")
    }

    if (!this.verifyLength(StateProvince.value.trim(), 50, false)) {
      this.errorMessage.push("La provincia è obbligatoria e non può avere più di 50 caratteri.")
    }

    if (!this.verifyLength(CountryRegion.value.trim(), 50, false)) {
      this.errorMessage.push("La regione è obbligatoria e non può avere più di 50 caratteri.")
    }

    if (!this.verifyLength(PostalCode.value.trim(), 15, false)) {
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
      this.customerRegister.FirstName = FirstName.value.trim();
      this.customerRegister.MiddleName = MiddleName.value.trim();
      this.customerRegister.LastName = LastName.value.trim();
      this.customerRegister.Phone = Phone.value.trim();

      //! AddressNew
      this.customerRegister.AddressLine1 = AddressLine1.value.trim();
      this.customerRegister.AddressLine2 = AddressLine2.value.trim();
      this.customerRegister.City = City.value.trim();
      this.customerRegister.StateProvince = StateProvince.value.trim();
      this.customerRegister.CountryRegion = CountryRegion.value.trim();
      this.customerRegister.PostalCode = PostalCode.value.trim();

      //! CustomerAddressNew
      this.customerRegister.AddressType = AddressType.value.trim();

      //! Credentials
      this.customerRegister.EmailAddress = EmailAddress.value.trim();
      this.customerRegister.Password = Password.value;

      this.http.register(this.customerRegister).subscribe({
        next: () => {

          //! customerNew
          FirstName.value = "";
          MiddleName.value = "";
          LastName.value = "";
          Phone.value = "";

          //! AddressNew
          AddressLine1.value = "";
          AddressLine2.value = "";
          City.value = "";
          StateProvince.value = "";
          CountryRegion.value = "";
          PostalCode.value = "";

          //! CustomerAddressNew
          AddressType.value = "";

          //! Credentials
          EmailAddress.value = "";
          Password.value = "";
          PasswordConfirm.value = "";
          this.router.navigate(['/login'], { queryParams: { message: 'Registrazione completata! Effettua il login per accedere.' } })
          return this.isRegistered == true
        },
        error: (error: any) => {
          this.errorMessage = []
          if (error.error.message == "emailExist") {
            this.errorMessage.push("L'email inserita è già in uso.")
          }
          this.errorMessage.forEach(element => {
            console.log(element);
          });
        }
      })


    } else {
      this.errorMessage.forEach(element => {
        console.log(element);
      });
      return this.isRegistered == false
    }
    return this.isRegistered == false
  }
}
