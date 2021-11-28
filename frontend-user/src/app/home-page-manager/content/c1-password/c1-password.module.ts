import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { C1PasswordComponent } from './c1-password.component';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { FormsModule } from '@angular/forms';



@NgModule({
  declarations: [C1PasswordComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    RouterModule.forChild([
      {
        path: '', component: C1PasswordComponent, children: [
        ],
      }
    ]),
  ]
})
export class C1PasswordModule { }
