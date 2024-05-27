import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NavbarComponent } from '../../core/navbar/navbar.component';
import { FooterComponent } from '../../core/footer/footer.component';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [FormsModule, CommonModule, NavbarComponent, FooterComponent],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css'
})
export class CartComponent {

}
