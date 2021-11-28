import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { D1OdercanceledComponent } from './d1-odercanceled.component';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { FormsModule } from '@angular/forms';



@NgModule({
  declarations: [D1OdercanceledComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    RouterModule.forChild([
      {
        path: '', component: D1OdercanceledComponent, children: [
        ],
      }
    ]),
  ]
})
export class D1OdercanceledModule { }
