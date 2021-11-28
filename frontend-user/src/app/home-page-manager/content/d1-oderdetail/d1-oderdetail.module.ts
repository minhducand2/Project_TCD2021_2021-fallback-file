import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { D1OderDetailComponent } from './d1-oderdetail.component';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { FormsModule } from '@angular/forms';



@NgModule({
  declarations: [D1OderDetailComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    RouterModule.forChild([
      {
        path: '', component: D1OderDetailComponent, children: [
        ],
      }
    ]),
  ]
})
export class D1OderDetailModule { }
