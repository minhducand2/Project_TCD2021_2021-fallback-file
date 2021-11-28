import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { LoginComponent } from "./login.component";
import { ApiService } from "../common/api-service/api.service";
import { TransferHttpCacheModule } from "@nguniversal/common";
import { RouterModule } from "@angular/router";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { LoginCookie } from "../common/core/login-cookie";
import { AngularFireModule } from '@angular/fire';
import { AngularFireAuthModule } from '@angular/fire/auth';
import { environment } from 'src/environments/environment';
import { SendDataService } from '../common/api-service/send-data.service';

@NgModule({
  declarations: [LoginComponent],
  imports: [
    TransferHttpCacheModule,
    CommonModule,
    RouterModule.forChild([
      {
        path: "",
        component: LoginComponent,
      },
    ]),
    FormsModule,
    ReactiveFormsModule,

    AngularFireModule.initializeApp(environment.firebaseConfig),
    AngularFireAuthModule,
  ],
  providers: [
    LoginCookie, SendDataService
  ],
})
export class LoginModule { }
