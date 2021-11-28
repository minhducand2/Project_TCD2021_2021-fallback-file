import { TransferHttpCacheModule } from '@nguniversal/common';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { G1CartComponent } from './g1-cart.component';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { VnCurrencyPipe } from 'src/app/common/_helpers/change-currency-vn.pipe';


@NgModule({
  declarations: [G1CartComponent],
  imports: [
    TransferHttpCacheModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forChild([
      {
        path: '',
        component: G1CartComponent,
        children: []
      }
    ]),
  ]
})
export class G1CartModule { }
