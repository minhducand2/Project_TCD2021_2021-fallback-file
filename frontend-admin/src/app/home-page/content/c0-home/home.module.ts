import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common'; 
import { TransferHttpCacheModule } from '@nguniversal/common';
import { RouterModule } from '@angular/router';
import { HomeComponent } from './home.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatNativeDateModule, MAT_DATE_FORMATS } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
import { ChartsModule } from 'ng2-charts';
import { MatMomentDateModule } from '@angular/material-moment-adapter';
import { FORMAT } from 'src/app/common/api-service/fomat-date-input';

@NgModule({
  declarations: [HomeComponent],
  imports: [
    TransferHttpCacheModule,
    CommonModule,
    RouterModule.forChild([
      {
        path: '', component: HomeComponent, children: [
        ],
      }
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
    MatPaginatorModule,
    MatButtonModule,
    MatCheckboxModule,
    MatIconModule,
    ChartsModule,
    MatMomentDateModule
  ],
  providers: [
    { provide: MAT_DATE_FORMATS, useValue: FORMAT }
  ],
})
export class HomeModule { }
