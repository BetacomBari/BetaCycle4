import { HttpRequest, HttpStatusCode } from '@angular/common/http';
import { Component } from '@angular/core';
import { HttprequestService } from '../../shared/services/httprequest.service';
import { Credientals } from '../../shared/models/credentials';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
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

}
