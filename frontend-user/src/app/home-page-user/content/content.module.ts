// import { A1HomeModule } from './home-page-user/content/a1-home/a1-home.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ContentComponent } from './content.component';
import { RouterModule } from '@angular/router';
import { TransferHttpCacheModule } from '@nguniversal/common';
import { ApiService } from '../../common/api-service/api.service';
// import { H1ThankYouModule } from './h1-thank-you/h1-thank-you.module';
// import { H1ThankYouComponent } from './h1-thank-you/h1-thank-you.component';


@NgModule({
    declarations: [
        ContentComponent,
    ],
    imports: [
        TransferHttpCacheModule,
        CommonModule,
        RouterModule.forChild([
            {
                path: '',
                component: ContentComponent,
                children: [
                    {
                        path: '',
                        loadChildren: () =>
                            import('./a1-home/a1-home.module').then(m => m.A1HomeModule)
                    },
                    {
                        path: 'a1-home/:title', data: { depth: 1 },
                        loadChildren: () =>
                            import('./a1-home/a1-home.module').then(m => m.A1HomeModule)
                    },
                    {
                        path: 'c1-list-menu/:id/:title', data: { depth: 2 },
                        loadChildren: () =>
                            import('./c1-list-menu/c1-list-menu.module').then(m => m.C1ListMenuModule)
                    },
                    {
                        path: 'c2-detail-menu/:id/:title', data: { depth: 3 },
                        loadChildren: () =>
                            import('./c2-detail-menu/c2-detail-menu.module').then(m => m.C2DetailMenuModule)
                    },
                    {
                        path: 'g1-cart/:title', data: { depth: 2 },
                        loadChildren: () =>
                            import('./g1-cart/g1-cart.module').then(m => m.G1CartModule)
                    },
                    {
                        path: 'h1-payment/:id/:title', data: { depth: 4 },
                        loadChildren: () =>
                            import('./h1-payment/h1-payment.module').then(m => m.H1PaymentModule)
                    },

                    {
                        path: 'i1-contact/:title', data: { depth: 2 },
                        loadChildren: () =>
                            import('./i1-contact/i1-contact.module').then(m => m.I1ContactModule)
                    },
                    {
                        path: 'j1-about/:title', data: { depth: 2 },
                        loadChildren: () =>
                            import('./j1-about/j1-about.module').then(m => m.J1AboutModule)
                    },
                    {
                        path: 'k1-thank-you/:title', data: { depth: 2 },
                        loadChildren: () =>
                            import('./k1-thank-you/k1-thank-you.module').then(m => m.K1ThankYouModule)
                    },
                    {
                        path: 'l1-search-menu/:keyword/:title', data: { depth: 2 },
                        loadChildren: () =>
                            import('./l1-search-menu/l1-search-menu.module').then(m => m.L1SearchMenuModule)
                    },
                    {
                        path: 'login/:title',
                        loadChildren: () => import('./login/login.module').then(m => m.LoginModule)
                    },
                ]
            }
        ]),

    ],
    providers: [],
    entryComponents: []
})
export class ContentModule { }
