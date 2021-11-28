import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { C1AccountComponent, C1AccountDialog } from './c1-account.component';
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
import { MatNativeDateModule, MAT_DATE_FORMATS } from '@angular/material/core';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { FORMAT } from 'src/app/common/api-service/fomat-date-input';
import { MatMomentDateModule } from '@angular/material-moment-adapter';



@NgModule({
  declarations: [C1AccountComponent, C1AccountDialog],
  imports: [
    TransferHttpCacheModule,
    CommonModule,
    RouterModule.forChild([
      {
        path: '',
        component: C1AccountComponent,
        children: [],
      },
    ]),

    FormsModule,
    ReactiveFormsModule,
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
  entryComponents: [C1AccountComponent],
  providers: [
    { provide: MAT_DATE_FORMATS, useValue: FORMAT }
  ],
})
export class C1AccountModule { }
