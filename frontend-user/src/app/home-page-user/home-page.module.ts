import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomePageComponent } from './home-page.component';
import { MenuComponent } from './menu/menu.component';
import { FooterComponent } from './footer/footer.component';
import { RouterModule } from '@angular/router';
import { TransferHttpCacheModule } from '@nguniversal/common';
import { LoginCookie } from '../common/core/login-cookie';
import { ApiService } from '../common/api-service/api.service';
import { SendDataService } from '../common/api-service/send-data.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
    declarations: [
        HomePageComponent,
        MenuComponent,
        FooterComponent,
    ],
    imports: [
        TransferHttpCacheModule,
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        RouterModule.forChild([
            {
                path: '', component: HomePageComponent, children: [
                    {
                        path: '',
                        loadChildren: () => import('./content/content.module').then(m => m.ContentModule)
                    },
                ],
            }
        ]),

    ],
    providers: [LoginCookie, SendDataService]
})
export class HomePageModule { }
