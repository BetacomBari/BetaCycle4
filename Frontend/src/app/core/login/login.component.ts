import { HttpRequest, HttpStatusCode } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { HttprequestService } from '../../shared/services/httprequest.service';
import { Credientals } from '../../shared/models/credentials';
import { RouterModule } from '@angular/router';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UserCardComponent } from '../user-card/user-card.component';
declare var handleSignOut: any;

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [RouterModule, CommonModule, FormsModule, RouterModule,UserCardComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  type: string = "password";
  isText: boolean = false;
  eyeIcon:string = "fa-eye-slash"
  email_toShow:string="";
  logged_in: boolean = false;
  userProfile: any;


  loginCredientals: Credientals = new Credientals()

  constructor(private http: HttprequestService, private router: Router) { }

  

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
        if (resp.status == 200) {
          console.log("LOGIN OK!");
          this.logged_in = true;
          this.email_toShow = email.value;
        }else{
          console.log("Status: " + resp.status);      
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


  ngOnInit() {
      this.userProfile = JSON.parse(sessionStorage.getItem("loggedInUser") || "");
  }

  handleSignOut() {
    handleSignOut();
    sessionStorage.removeItem("loggedInUser");
    this.router.navigate(["/login"]).then( ()=>{
      window.location.reload();
    });
  }
}
