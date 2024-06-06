import { Component, Input } from '@angular/core';
import { Product } from './product';

@Component({
  selector: 'cart-product', // Selettore del componente
  templateUrl: './product.html', // Template HTML del componente
})
export class CartProduct {
    product?: Product;
}