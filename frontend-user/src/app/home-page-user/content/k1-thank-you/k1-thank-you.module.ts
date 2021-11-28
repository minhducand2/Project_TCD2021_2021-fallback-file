import { TransferHttpCacheModule } from '@nguniversal/common';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { K1ThankYouComponent } from './k1-thank-you.component';
import { RouterModule } from '@angular/router';



@NgModule({
  declarations: [K1ThankYouComponent],
  imports: [
    TransferHttpCacheModule,
    CommonModule,
    RouterModule.forChild([
      {
        path: '',
        component: K1ThankYouComponent,
        children: []
      }
    ]),
  ]
})
export class K1ThankYouModule { }
