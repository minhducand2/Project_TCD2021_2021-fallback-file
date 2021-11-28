import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ContentComponent } from './content.component';
import { RouterModule } from '@angular/router';
import { TransferHttpCacheModule } from '@nguniversal/common';
import { ApiService } from '../../common/api-service/api.service'; 

@NgModule({
    declarations: [ContentComponent],
    imports: [
        TransferHttpCacheModule,
        CommonModule,
        RouterModule.forChild([
            {
                // ng g module home-page/content/sellTicket --module content
                // ng g c home-page/content/sellTicket

                path: '',
                component: ContentComponent,
                children: [ 
                    {
                        path: 'c1-account/:title',
                        loadChildren: () =>
                            import('./c1-account/c1-account.module').then(m => m.C1AccountModule)
                    },
                    {
                      path: 'c1-password/:title',
                      loadChildren: () =>
                          import('./c1-password/c1-password.module').then(m => m.C1PasswordModule)
                    },
                    {
                      path: 'c1-paypal/:title',
                      loadChildren: () =>
                          import('./c1-paypal/c1-paypal.module').then(m => m.C1PaypalModule)
                    },
                    {
                      path: 'd1-oderplaced/:title',
                      loadChildren: () =>
                          import('./d1-oderplaced/d1-oderplaced.module').then(m => m.D1OderPlacedModule)
                    },
                    {
                      path: 'd1-orderreceived/:title',
                      loadChildren: () =>
                          import('./d1-orderreceived/d1-orderreceived.module').then(m => m.D1OrderreceivedModule)
                    },
                    {
                      path: 'd1-odercanceled/:title',
                      loadChildren: () =>
                          import('./d1-odercanceled/d1-odercanceled.module').then(m => m.D1OdercanceledModule)
                    },
                    {
                      path: 'd1-oderdetail/:title',
                      loadChildren: () =>
                          import('./d1-oderdetail/d1-oderdetail.module').then(m => m.D1OderDetailModule)
                    },
                    {
                        path: 'd2-order-views/:idOrderShop/:idMenu/:title',
                        loadChildren: () =>
                            import('./d2-order-views/d2-order-views.module').then(m => m.D2OrderViewsModule)
                      },
                ]
            }
        ]),

    ],
    providers: [],
    entryComponents: []
})
export class ContentModule { }
