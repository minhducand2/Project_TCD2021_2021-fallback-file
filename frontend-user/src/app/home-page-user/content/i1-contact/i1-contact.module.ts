import { TransferHttpCacheModule } from '@nguniversal/common';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { I1ContactComponent } from './i1-contact.component';
import { RouterModule } from '@angular/router';
import { FormsModule,ReactiveFormsModule }   from '@angular/forms';



@NgModule({
  declarations: [I1ContactComponent],
  imports: [
    TransferHttpCacheModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forChild([
      {
        path: '',
        component: I1ContactComponent,
        children: []
      }
    ]),
  ]
})
export class I1ContactModule { }
