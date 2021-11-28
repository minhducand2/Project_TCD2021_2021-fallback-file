import { TransferHttpCacheModule } from '@nguniversal/common';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { C1ListMenuComponent } from './c1-list-menu.component';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LazyLoadImageModule } from 'ng-lazyload-image';

@NgModule({
  declarations: [C1ListMenuComponent],
  imports: [
    TransferHttpCacheModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    LazyLoadImageModule,
    RouterModule.forChild([
      {
        path: '',
        component: C1ListMenuComponent,
        children: []
      }
    ]),
  ]
})
export class C1ListMenuModule { }
