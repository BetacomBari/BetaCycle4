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
<<<<<<< HEAD
>>>>>>> 58de71c94396aadc3c43c8e5e33268e99266a726
=======
import { CartComponent } from './features/cart/cart.component';
import { FaqComponent } from './features/faq/faq.component';
import { ProductDetailComponent } from './features/product-detail/product-detail.component';
>>>>>>> d3f155c7c5faecf8b6b633fd742bc92a96f0a0f0

export const routes: Routes = [
    {path:"", component:HomeComponent},
    {path:"logout", component:LogoutComponent},
    {path:"signup", component:SignupComponent},
    {path:"login", component:LoginComponent},
    {path:"customer", component:CustomerComponent},
    {path:"reset", component: ResetComponent},
<<<<<<< HEAD
<<<<<<< HEAD
    {path:"payment", component:PayWithGoogleComponent}
=======
    {path:"product", component: ProductComponent}
=======
    {path:"product", component: ProductComponent},
    {path:"cart", component: CartComponent},
    {path:"faq", component: FaqComponent},
    {path:"detail", component: ProductDetailComponent},
    {path:"cart", component: CartComponent}
>>>>>>> d3f155c7c5faecf8b6b633fd742bc92a96f0a0f0

>>>>>>> 58de71c94396aadc3c43c8e5e33268e99266a726
];
