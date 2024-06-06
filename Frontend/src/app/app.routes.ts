import { RouterModule, Routes } from '@angular/router';
import { LogoutComponent } from './core/logout/logout.component';
import { SignupComponent } from './core/signup/signup.component';
import { NgModule } from '@angular/core';
import { LoginComponent } from './core/login/login.component';
import { CustomerComponent } from './features/admin/customer/customer.component';
import { HomeComponent } from './features/home/home.component';
import { ResetComponent } from './core/reset/reset.component';
// import { PayWithGoogleComponent } from './core/pay-with-google/pay-with-google.component';
import { ProductComponent } from './core/product/product.component';
import { RecommandationsComponent } from './core/recommandations/recommandations.component';
import { ChatComponent } from './chat/chat.component';

import { CartComponent } from './features/cart/cart.component';
import { FaqComponent } from './features/faq/faq.component';
import { ProductDetailComponent } from './features/product-detail/product-detail.component';
import { CustomerNewComponent } from './features/admin/customer-new/customer-new.component';

export const routes: Routes = [
    {path:"", component:HomeComponent},
    {path:"logout", component:LogoutComponent},
    {path:"signup", component:SignupComponent},
    {path:"login", component:LoginComponent},
    {path:"customer", component:CustomerComponent},
    {path:"customerNew", component:CustomerNewComponent},
    {path:"reset", component: ResetComponent},
    {path:"chat", component: ChatComponent},
    {path:"recommandations", component: RecommandationsComponent},
    // {path:"payment", component:PayWithGoogleComponent},
    {path:"product", component: ProductComponent},
    {path:"product", component: ProductComponent},
    {path:"cart", component: CartComponent},
    {path:"detail", component: ProductDetailComponent},

]
