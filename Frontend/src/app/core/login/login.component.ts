import { HttpRequest, HttpStatusCode } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { HttprequestService } from '../services/httprequest.service';
import { RouterModule } from '@angular/router';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UserCardComponent } from '../user-card/user-card.component';
import { User } from '../../shared/models/user';
import { NavbarComponent } from '../navbar/navbar.component';
import { ResetPasswordService } from '../services/reset-password.service';
import { AuthService } from '../services/auth.service';
import { CustomerRegister } from '../../shared/models/CustomerRegister';
import { Credentials } from '../../shared/models/credentials';


@Component({
  selector: 'app-login',
  standalone: true,
  imports: [RouterModule, CommonModule, FormsModule, RouterModule, UserCardComponent, NavbarComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  type: string = "password";
  isText: boolean = false;
  eyeIcon: string = "fa-eye-slash"
  email_toShow: string = "";
  private isLogged: boolean = false;
  userProfile: any;
  customerRegister: CustomerRegister = new CustomerRegister();
  resetPassword!: string;
  isEmailForResetValid!: boolean;
  jwtToken: string = "";
  loginCredentials: Credentials = new Credentials()
  errorMessage: string[] = []

  constructor(private http: HttprequestService, private resetService: ResetPasswordService, public authStatus: AuthService) { }

  login(email: HTMLInputElement, password: HTMLInputElement){
    if (email.value != "" && password.value != "") {
      this.loginCredentials.EmailAddress = email.value.trim()
      this.loginCredentials.Password = password.value

      this.http.loginPostJwt(this.loginCredentials).subscribe({
        next: (resp: any) => {
          console.log("LOGIN OK!");
          this.isLogged = true;
          this.email_toShow = email.value;
          this.jwtToken = resp.body.token;
          localStorage.setItem('jwtToken', this.jwtToken)
          this.authStatus.setJwtLoginStatus(true, this.jwtToken);
        },
        error: (error: any) => {
          this.authStatus.setJwtLoginStatus(false);
          this.errorMessage = []
          if (error.error.message == "passwordError") {
            this.errorMessage.push("Password non valida.")
            console.log(this.errorMessage);          
          } else if(error.error.message == "registratiNuovamente"){
            this.errorMessage.push("Necessaria nuova registrazione per aggiornamento interno.")
            console.log(this.errorMessage);                     
          } else if (error.error.message == "emailError") {
            this.errorMessage.push("Email non registrata.")
            console.log(this.errorMessage);          
          } else{
            this.errorMessage.push("Errore generico.")
            console.log(this.errorMessage);
          }
        }
      })
    } else {
      this.errorMessage = []
      this.errorMessage.push("I campi non possono essere vuoti")
            console.log(this.errorMessage);
    }
  }

  hideShowPassword() {
    this.isText = !this.isText;
    this.isText ? this.eyeIcon = "fa-eye" : this.eyeIcon = "fa-eye-slash";
    this.isText ? this.type = "text" : this.type = "password"
  }

  writeInDb() {
    this.customerRegister.EmailAddress = this.userProfile.email;
    this.customerRegister.FirstName = this.userProfile.given_name;
    this.customerRegister.LastName = this.userProfile.family_name;
    this.customerRegister.Password = "passwordFromGoogle";

    this.http.register(this.customerRegister).subscribe({
      next: (data: any) => {
        this.customerRegister = data;
        console.log(this.customerRegister)
      },
      error: (err: any) => {
        console.log(err);
      }
    })
  }

  
  checkValidEmailForReset(event: string) {
    const value = event;
    //const pattern = /^[\w-\.]+@([\w-]+\.)+[\w-]{2,3}$/;
    let pattern = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,3}$/;
    this.isEmailForResetValid = pattern.test(value);
    console.log(this.isEmailForResetValid);
    return this.isEmailForResetValid;
  }
  
  confirmToSend() {
    if (this.checkValidEmailForReset(this.resetPassword)) {
      console.log(this.resetPassword);
      this.resetPassword = "";
      const buttonRef = document.getElementById("closeBtn");
      buttonRef?.click();
      // API call
      this.resetService.sendResetPasswordLink(this.resetPassword)
      .subscribe({
        next: (res) => {
          this.resetPassword = "";
          const buttonRef = document.getElementById("closeBtn");
          buttonRef?.click();
        },
        error: (err) => {
          console.log(err);
        }
      })
    }
  }
}
