import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegisterComponent } from './register.component';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TransferHttpCacheModule } from '@nguniversal/common';
import { ApiService } from '../common/api-service/api.service';
import { MatFormFieldModule } from '@angular/material/form-field';


@NgModule({
  declarations: [RegisterComponent],
  imports: [
    TransferHttpCacheModule,
    CommonModule,
    MatFormFieldModule,
    RouterModule.forChild([
      {
        path: '',
        component: RegisterComponent,
      },
    ]),
    FormsModule,
    ReactiveFormsModule,
  ],
  providers: [ApiService],
})
export class RegisterModule {}
