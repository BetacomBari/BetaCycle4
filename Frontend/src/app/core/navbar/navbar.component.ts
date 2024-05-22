import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { UserCardComponent } from '../user-card/user-card.component';
import { AuthService } from '../services/auth.service';
declare var handleSignOut: any;

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, UserCardComponent],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {
  isLoggged: boolean = false
  constructor(private auth: AuthService) { }
  getLoginStatus() {
    this.auth.getLoginStatus()
  }
}
