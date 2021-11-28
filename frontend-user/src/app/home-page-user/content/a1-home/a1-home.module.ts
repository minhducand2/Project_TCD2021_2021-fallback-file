import { TransferHttpCacheModule } from '@nguniversal/common';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { A1HomeComponent } from './a1-home.component';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { LazyLoadImageModule } from 'ng-lazyload-image'; 
@NgModule({
  declarations: [A1HomeComponent],
  imports: [
    TransferHttpCacheModule,
        CommonModule,
        LazyLoadImageModule,
        RouterModule.forChild([
            {
                path: '',
                component: A1HomeComponent,
                children: []
            }
        ]),
        FormsModule,
  ]
})
export class A1HomeModule { }
