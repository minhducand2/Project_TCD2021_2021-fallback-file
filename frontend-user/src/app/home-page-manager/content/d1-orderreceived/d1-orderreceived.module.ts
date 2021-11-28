import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { D1OrderreceivedComponent } from './d1-orderreceived.component';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { FormsModule } from '@angular/forms';



@NgModule({
  declarations: [D1OrderreceivedComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    RouterModule.forChild([
      {
        path: '', component: D1OrderreceivedComponent, children: [
        ],
      }
    ]),
  ]
})
export class D1OrderreceivedModule { }
