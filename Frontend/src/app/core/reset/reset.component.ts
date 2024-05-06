import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-reset',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './reset.component.html',
  styleUrl: './reset.component.css'
})
export class ResetComponent implements OnInit{
  isText: boolean = false;
  eyeIcon:string = "fa-eye-slash"
  type: string = "password";
  retypePassword!: string;
  arePasswordsEqual!: boolean ;

  constructor() {}

  ngOnInit(): void {
      
  }

  hideShowPassword(){
    this.isText = !this.isText;
    this.isText ? this.eyeIcon = "fa-eye" : this.eyeIcon = "fa-eye-slash";
    this.isText ? this.type="text" : this.type = "password"
  }

  checkEqualPassword(password: HTMLInputElement, event: string){
    const r_password = event;
    if (password.value == r_password){
      this.arePasswordsEqual = true;
      console.log(this.arePasswordsEqual)
    } else{
      this.arePasswordsEqual = false;
      console.log(this.arePasswordsEqual)
    }
    return this.arePasswordsEqual;
  }
}
