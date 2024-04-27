import { RouterModule, Routes } from '@angular/router';
import { LogoutComponent } from './core/logout/logout.component';
import { SignupComponent } from './core/signup/signup.component';
import { NgModule } from '@angular/core';
import { LoginComponent } from './core/login/login.component';

export const routes: Routes = [
    {path:"logout", component:LogoutComponent},
    {path:"signup", component:SignupComponent},
    {path:"login", component:LoginComponent}
];

