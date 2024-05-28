import { RouterModule, Routes } from '@angular/router';
import { LogoutComponent } from './core/logout/logout.component';
import { SignupComponent } from './core/signup/signup.component';
import { NgModule } from '@angular/core';
import { LoginComponent } from './core/login/login.component';
import { CustomerComponent } from './features/admin/customer/customer.component';
import { HomeComponent } from './features/home/home.component';
import { ResetComponent } from './core/reset/reset.component';

import { PayWithGoogleComponent } from './core/pay-with-google/pay-with-google.component';

import { ProductComponent } from './core/product/product.component';

import { RecommandationsComponent } from './core/recommandations/recommandations.component';


export const routes: Routes = [
    {path:"", component:HomeComponent},
    {path:"logout", component:LogoutComponent},
    {path:"signup", component:SignupComponent},
    {path:"login", component:LoginComponent},
    {path:"customer", component:CustomerComponent},
    {path:"reset", component: ResetComponent},

    {path:"product", component: ProductComponent},
    {path:"recommandations", component: RecommandationsComponent}

];
