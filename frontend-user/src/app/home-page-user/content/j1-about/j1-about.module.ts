import { TransferHttpCacheModule } from '@nguniversal/common';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { J1AboutComponent } from './j1-about.component';
import { RouterModule } from '@angular/router';
import { LazyLoadImageModule } from 'ng-lazyload-image'; 


@NgModule({
  declarations: [J1AboutComponent],
  imports: [
    TransferHttpCacheModule,
    CommonModule,
    LazyLoadImageModule,
    RouterModule.forChild([
      {
        path: '',
        component: J1AboutComponent,
        children: []
      }
    ]),
  ]
})
export class J1AboutModule { }
