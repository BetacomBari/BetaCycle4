import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from './core/navbar/navbar.component';
import { LoginComponent } from './core/login/login.component';
import { LogoutComponent } from './core/logout/logout.component';
import { UserCardComponent } from './core/user-card/user-card.component';
import { SignupComponent } from './core/signup/signup.component';
import { ReactiveFormsModule } from '@angular/forms';
import { GooglePayButtonModule } from '@google-pay/button-angular';
import { GooglePayButtonComponent } from '@google-pay/button-angular';



@Component({
  selector: 'app-root',
  standalone: true,
<<<<<<< HEAD
  imports: [RouterOutlet, NavbarComponent, LoginComponent, LogoutComponent, UserCardComponent, 
    SignupComponent, ReactiveFormsModule],
=======
  imports: [RouterOutlet, NavbarComponent, LoginComponent, LogoutComponent, 
    UserCardComponent, SignupComponent, ReactiveFormsModule, GooglePayButtonModule],
>>>>>>> Register
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'betacycle';
}
