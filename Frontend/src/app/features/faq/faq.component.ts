import { Component } from '@angular/core';
import { NavbarComponent } from '../../core/navbar/navbar.component';
import { FooterComponent } from '../../core/footer/footer.component';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-faq',
  standalone: true,
  imports: [RouterModule, NavbarComponent, FooterComponent],
  templateUrl: './faq.component.html',
  styleUrl: './faq.component.css'
})
export class FaqComponent {

}
