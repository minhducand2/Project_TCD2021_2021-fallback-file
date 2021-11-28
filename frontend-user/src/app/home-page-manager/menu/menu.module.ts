import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { TransferHttpCacheModule } from '@nguniversal/common';
import { MenuComponent } from './menu.component';
import { LoginCookie } from 'src/app/common/core/login-cookie';

@NgModule({
    declarations: [
        MenuComponent,
    ],
    imports: [
        TransferHttpCacheModule,
        CommonModule,
        RouterModule.forChild([
            {
                path: '', component: MenuComponent, children: [
                    {
                        path: '',
                        loadChildren: () => import('../content/content.module').then(m => m.ContentModule)
                    },
                    // {
                    //     path: 'candidate',
                    //     loadChildren: () => import('./content-candidate/content-candidate.module').then(m => m.ContentCandidateModule)
                    // },
                    // {
                    //     path: 'company',
                    //     loadChildren: () => import('./content-company/content-company.module').then(m => m.ContentCompanyModule)
                    // }, 

                ],
            }
        ]),
    ],
    providers: [LoginCookie]
})
export class MenuModule { }
