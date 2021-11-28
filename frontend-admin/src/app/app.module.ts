import { BrowserModule } from '@angular/platform-browser';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { TransferHttpCacheModule } from '@nguniversal/common';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CommonModule } from '@angular/common';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ToastrModule } from 'ngx-toastr';
import { JwtInterceptor, ErrorInterceptor, AuthGuard } from './common/_helpers';
import { ApiService } from './common/api-service/api.service';
import { FormsModule } from '@angular/forms';
import { NgxSpinnerModule } from "ngx-spinner";

import { AppRoutingModule } from './app-routing.module'; 


@NgModule({
    declarations: [
        AppComponent
    ],
    imports: [
        FormsModule,
        BrowserModule.withServerTransition({ appId: 'my-app' }),
        AppRoutingModule,
        // https://www.npmjs.com/package/ngx-toastr
        ToastrModule.forRoot(), // ToastrModule added
        TransferHttpCacheModule,
        CommonModule,
        HttpClientModule,

        // https://www.npmjs.com/package/angular-animations
        BrowserAnimationsModule,
        // ServiceWorkerModule.register('ngsw-worker.js', { enabled: environment.production }),

        // https://www.npmjs.com/package/ngx-spinner#stackblitz-demo
        NgxSpinnerModule,

    ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA],
    providers: [
        { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
        { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
        ApiService
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
