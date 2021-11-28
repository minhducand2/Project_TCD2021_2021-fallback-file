import { TransferHttpCacheModule } from '@nguniversal/common';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { L1SearchMenuComponent } from './l1-search-menu.component';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LazyLoadImageModule } from 'ng-lazyload-image';



@NgModule({
  declarations: [L1SearchMenuComponent],
  imports: [
    TransferHttpCacheModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    LazyLoadImageModule,
    RouterModule.forChild([
      {
        path: '',
        component: L1SearchMenuComponent,
        children: []
      }
    ]),
  ]
})
export class L1SearchMenuModule { }
