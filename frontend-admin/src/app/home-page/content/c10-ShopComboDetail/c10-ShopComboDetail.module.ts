import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TransferHttpCacheModule } from '@nguniversal/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSelectModule } from '@angular/material/select';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatRadioModule } from '@angular/material/radio';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDialogModule } from '@angular/material/dialog';
import { CKEditorModule } from 'ckeditor4-angular';
import { MatIconModule } from '@angular/material/icon';

import { MAT_DATE_FORMATS } from '@angular/material/core';
import { FORMAT } from 'src/app/common/api-service/fomat-date-input';
import { MatMomentDateModule } from '@angular/material-moment-adapter';

import { C10ShopcombodetailComponent, C10ShopcombodetailDialog } from '././c10-ShopComboDetail.component';  


@NgModule({
  declarations: [C10ShopcombodetailComponent, C10ShopcombodetailDialog],   

  imports: [
    TransferHttpCacheModule,
    CommonModule,
    RouterModule.forChild([
      {
        path: '', component: C10ShopcombodetailComponent, children: [
        ],
      }
    ]),
    FormsModule,
    ReactiveFormsModule,
    CKEditorModule,

    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatSelectModule,
    MatRadioModule,
    MatDialogModule,
    MatCardModule,
    MatSortModule,
    MatTableModule,
    MatPaginatorModule,
    MatButtonModule,
    MatCheckboxModule,
    MatIconModule,
    MatMomentDateModule
  ],
  providers: [
    { provide: MAT_DATE_FORMATS, useValue: FORMAT }
  ],
  entryComponents: [C10ShopcombodetailDialog]
})
export class C10ShopcombodetailModule { }	
