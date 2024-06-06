import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { UserCardComponent } from '../user-card/user-card.component';
import { AuthService } from '../services/auth.service';
declare var handleSignOut: any;
declare var handleSignOut: any;
import * as AOS from 'aos';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, UserCardComponent],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})

export class NavbarComponent implements OnInit {
  imageUrl: string = "";
  jwtToken: string|null = ""
  isAdmin:boolean = false
  @Input() decodedEmail: string = '';

  constructor(public authStatus: AuthService, private router: Router) { }
  ngOnInit() {
    AOS.init();
    this.getDecodeToken()

    this.imageUrl = '/assets/logo.png';

  }


  decodeBase64Url(str: string): string {
    // Replace non-URL-safe characters with URL-safe equivalents
    return decodeURIComponent(atob(str.replace(/-/g, '+').replace(/_/g, '/')));
  }

  parseJson(str: string): any {
    try {
      return JSON.parse(str);
    } catch (error) {
      throw new Error('Invalid JWT payload');
    }
  }

  splitToken(token: string | null): string[] {
    if (!token || token.length < 3) {
      throw new Error('Invalid JWT token');
    }
    return token.split('.');
  }

  // FUNZIONI PER LA DECODIFICA DEL TOKEN
  async getDecodeToken() {
    this.jwtToken = localStorage.getItem("jwtToken")
    try {
      const parts = this.splitToken(this.jwtToken);
      const decodedHeader = this.decodeBase64Url(parts[0]);
      const decodedPayload = this.parseJson(this.decodeBase64Url(parts[1]));

      console.log(decodedPayload);
      if (decodedPayload.unique_name == "admin@admin.com") {
        this.isAdmin = true
      }
      // console.log('Header:', decodedHeader);
      // console.log('Payload:', decodedPayload);
      // Access specific claims from payload (e.g., username)
      const username = decodedPayload.sub; // Assuming 'sub' holds username
      // console.log('Username:', decodedPayload.unique_name);
    } catch (error) {
      console.error('Error decoding token:', error);
    }
  }

  search(inputSearch: HTMLInputElement) {
    this.router.navigate(['/product'], { queryParams: { message: inputSearch.value.trim() } })
      .then(() => {
        this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
          this.router.navigate(['/product'], { queryParams: { message: inputSearch.value.trim() } });
        });
      });
  }
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
