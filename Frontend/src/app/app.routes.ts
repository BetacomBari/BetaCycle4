import { RouterModule, Routes } from '@angular/router';
import { LogoutComponent } from './core/logout/logout.component';
import { SignupComponent } from './core/signup/signup.component';
import { NgModule } from '@angular/core';
import { LoginComponent } from './core/login/login.component';
import { CustomerComponent } from './features/admin/customer/customer.component';
import { HomeComponent } from './features/home/home.component';
import { ResetComponent } from './core/reset/reset.component';
<<<<<<< HEAD
import { PayWithGoogleComponent } from './core/pay-with-google/pay-with-google.component';
=======
import { ProductComponent } from './core/product/product.component';
>>>>>>> 58de71c94396aadc3c43c8e5e33268e99266a726

export const routes: Routes = [
    {path:"", component:HomeComponent},
    {path:"logout", component:LogoutComponent},
    {path:"signup", component:SignupComponent},
    {path:"login", component:LoginComponent},
    {path:"customer", component:CustomerComponent},
    {path:"reset", component: ResetComponent},
<<<<<<< HEAD
    {path:"payment", component:PayWithGoogleComponent}
=======
    {path:"product", component: ProductComponent}

>>>>>>> 58de71c94396aadc3c43c8e5e33268e99266a726
];
