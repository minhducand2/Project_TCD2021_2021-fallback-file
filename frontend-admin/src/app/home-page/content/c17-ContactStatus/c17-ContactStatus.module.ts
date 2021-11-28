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

import { C17ContactstatusComponent, C17ContactstatusDialog } from '././c17-ContactStatus.component';  


@NgModule({
  declarations: [C17ContactstatusComponent, C17ContactstatusDialog],   

  imports: [
    TransferHttpCacheModule,
    CommonModule,
    RouterModule.forChild([
      {
        path: '', component: C17ContactstatusComponent, children: [
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
  entryComponents: [C17ContactstatusDialog]
})
export class C17ContactstatusModule { }	
