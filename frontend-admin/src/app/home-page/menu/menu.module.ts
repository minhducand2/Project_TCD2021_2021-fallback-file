import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatNativeDateModule } from '@angular/material/core';
import { TransferHttpCacheModule } from '@nguniversal/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    MatNativeDateModule,
    TransferHttpCacheModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
  ],
})
export class MenuModule {}
