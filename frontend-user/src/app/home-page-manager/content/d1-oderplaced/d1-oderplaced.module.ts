import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { D1OderPlacedComponent } from './d1-oderplaced.component';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { FormsModule } from '@angular/forms';



@NgModule({
  declarations: [D1OderPlacedComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    RouterModule.forChild([
      {
        path: '', component: D1OderPlacedComponent, children: [
        ],
      }
    ]),
  ]
})
export class D1OderPlacedModule { }
