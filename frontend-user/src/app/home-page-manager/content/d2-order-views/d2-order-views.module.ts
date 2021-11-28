import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { D2OrderViewsComponent } from './d2-order-views.component';



@NgModule({
  declarations: [D2OrderViewsComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    RouterModule.forChild([
      {
        path: '', component: D2OrderViewsComponent, children: [
        ],
      }
    ]),
  ]
})
export class D2OrderViewsModule { }
