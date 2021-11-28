import { TransferHttpCacheModule } from '@nguniversal/common';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { H1PaymentComponent } from './h1-payment.component';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule, FormsModule } from '@angular/forms'; 
import { NgxPayPalModule } from 'ngx-paypal';


@NgModule({
  declarations: [H1PaymentComponent],
  imports: [
    TransferHttpCacheModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule, 
    NgxPayPalModule,
    RouterModule.forChild([
      {
        path: '',
        component: H1PaymentComponent,
        children: []
      }
    ]),
  ]
})
export class H1PaymentModule { }
