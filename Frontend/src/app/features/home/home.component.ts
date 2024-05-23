import { Component } from '@angular/core';
import { NavbarComponent } from '../../core/navbar/navbar.component';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [NavbarComponent, FormsModule, CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {

  
  successMessage: string | null = null;
  
  constructor(private route: ActivatedRoute){}


  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.successMessage = params['message'];
    });
  }

}
