import { HttpRequest, HttpStatusCode } from '@angular/common/http';
import { Component } from '@angular/core';
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

  login(usr: HTMLInputElement, pwd: HTMLInputElement) {

    if (usr.value != "" && pwd.value != "") {
      this.loginCredientals.EmailAddress = usr.value
      this.loginCredientals.Password = usr.value

      this.http.loginPost(this.loginCredientals).subscribe(resp =>{    
        if (resp.status === HttpStatusCode.Ok) {
          console.log("LOGIN OK!");
        }else{
          console.log("LOGIN NON RIUSCITO" + resp.status);       
        }
      })
    } else (alert("Usurname e password sono obbligatori"))
  }

  hideShowPassword(){
    this.isText = !this.isText;
    this.isText ? this.eyeIcon = "fa-eye" : this.eyeIcon = "fa-eye-slash";
    this.isText ? this.type="text" : this.type = "password"
  }

}
