import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ResetPassword } from '../../shared/models/reset-password';
import { ResetPasswordService } from '../services/reset-password.service';
import { Router } from '@angular/router';

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
  emailToReset: string = "";
  emailToken: string = "";
  retypePassword!: string;
  arePasswordsEqual!: boolean;
  resetPasswordObj: ResetPassword = new ResetPassword();

  constructor(private activatedRoute: ActivatedRoute, 
    private resetService: ResetPasswordService,
    private router: Router) {}

  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe(val => {
      this.emailToReset = val['email'];
      let uriToken = val['code'];
      this.emailToken = uriToken.replace(/ /g, '+')
      console.log(this.emailToReset, this.emailToken)
      console.log(this.emailToken)

    })
      
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

  reset() {
    if (this.arePasswordsEqual == true) {
      this.resetPasswordObj.email = this.emailToReset;
      this.resetPasswordObj.newPassword = this.retypePassword;
      this.resetPasswordObj.confirmPassword = this.retypePassword;
      this.resetPasswordObj.emailToken = this.emailToken;

      this.resetService.resetPassword(this.resetPasswordObj)
      .subscribe({
        next:(res) => {
          console.log("Change of password done");
          this.router.navigate(['/']);

        } , error: (err) => {
          console.log("Error in change password ")
          console.log(this.emailToken);
        }
      })

    } else {
      
    }
  }

}
