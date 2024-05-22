import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { UserCardComponent } from '../user-card/user-card.component';
import { AuthService } from '../services/auth.service';
declare var handleSignOut: any;
declare var handleSignOut: any;

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, UserCardComponent],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {
  
  constructor(public authStatus: AuthService, private router: Router,) { }

handleSignOut() {
  handleSignOut();
  localStorage.removeItem("jwtToken")
  sessionStorage.removeItem("loggedInUser");
  this.router.navigate(["/login"]).then(() => {
    window.location.reload();
  });
  localStorage.removeItem("loggedInUser");
  this.router.navigate(["/login"]).then(() => {
    window.location.reload();
  });
}
}
