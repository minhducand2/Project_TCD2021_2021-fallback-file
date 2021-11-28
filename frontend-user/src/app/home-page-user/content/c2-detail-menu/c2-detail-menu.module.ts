import { TransferHttpCacheModule } from '@nguniversal/common';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { C2DetailMenuComponent } from './c2-detail-menu.component';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SendDataService } from 'src/app/common/api-service/send-data.service';
// import { VnCurrencyPipe } from 'src/app/common/_helpers/change-currency-vn.pipe';
import { ShareButtonModule } from 'ngx-sharebuttons/button';
import { ShareIconsModule } from 'ngx-sharebuttons/icons';
import { LazyLoadImageModule } from 'ng-lazyload-image';


@NgModule({
  // declarations: [C2DetailMenuComponent, VnCurrencyPipe],
  declarations: [C2DetailMenuComponent],
  imports: [
    TransferHttpCacheModule,
    CommonModule,
    LazyLoadImageModule,
    FormsModule,
    ReactiveFormsModule,
    ShareButtonModule,
    ShareIconsModule,
    RouterModule.forChild([
      {
        path: '',
        component: C2DetailMenuComponent,
        children: []
      }
    ]),
  ],
})
export class C2DetailMenuModule { }
