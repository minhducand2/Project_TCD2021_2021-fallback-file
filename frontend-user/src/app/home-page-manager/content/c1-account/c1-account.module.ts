import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { C1AccountComponent } from './c1-account.component';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { FormsModule } from '@angular/forms';  



@NgModule({
  declarations: [C1AccountComponent],
  imports: [
    CommonModule,   
    ReactiveFormsModule,
    FormsModule, 
    RouterModule.forChild([
      {
        path: '', component: C1AccountComponent, children: [
        ],
      }
    ]),
  ]
})
export class C1AccountModule { }
