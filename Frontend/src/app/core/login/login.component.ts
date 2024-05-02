import { HttpRequest, HttpStatusCode } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { HttprequestService } from '../../shared/services/httprequest.service';
import { Credientals } from '../../shared/models/credentials';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  type: string = "password";
  isText: boolean = false;
  eyeIcon:string = "fa-eye-slash"

  loginCredientals: Credientals = new Credientals()

  constructor(private http: HttprequestService) { }

  

  login(email: HTMLInputElement, password: HTMLInputElement) {
    console.log("sono entrato nella funzione");
    this.loginCredientals.EmailAddress = email.value
    this.loginCredientals.Password = password.value
    console.log(email.value);
    console.log(password.value);

    if (email.value != "" && password.value != "") { 
      this.loginCredientals.EmailAddress = email.value
      this.loginCredientals.Password = password.value

      this.http.loginPost(this.loginCredientals).subscribe(resp =>{    
        if (resp.status === HttpStatusCode.Accepted) {
          console.log("LOGIN OK!");
        }else{
          console.log("LOGIN NON RIUSCITO" + resp.status);       
        }
      })
    } else{
      console.log("error")
    }
  } 

  hideShowPassword(){
    this.isText = !this.isText;
    this.isText ? this.eyeIcon = "fa-eye" : this.eyeIcon = "fa-eye-slash";
    this.isText ? this.type="text" : this.type = "password"
  }

}
