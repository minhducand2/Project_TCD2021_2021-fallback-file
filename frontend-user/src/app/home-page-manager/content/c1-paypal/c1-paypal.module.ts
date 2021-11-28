import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { C1PaypalComponent } from './c1-paypal.component';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { FormsModule } from '@angular/forms';



@NgModule({
  declarations: [C1PaypalComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    RouterModule.forChild([
      {
        path: '', component: C1PaypalComponent, children: [
        ],
      }
    ]),
  ]
})
export class C1PaypalModule { }
