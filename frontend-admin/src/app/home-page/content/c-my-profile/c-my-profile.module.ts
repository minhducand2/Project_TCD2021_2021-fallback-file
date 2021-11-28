import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CMyProfileComponent } from './c-my-profile.component';
import { RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSelectModule } from '@angular/material/select';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { ReactiveFormsModule } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';

@NgModule({
  declarations: [CMyProfileComponent],
  imports: [
    CommonModule,
    RouterModule.forChild([
      {
        path: '',
        component: CMyProfileComponent,
        children: [],
      },
    ]),
    MatCardModule,
    MatCheckboxModule,
    MatPaginatorModule,
    MatSelectModule,
    MatSortModule,
    MatTableModule,
    MatInputModule,
    MatButtonModule,
    ReactiveFormsModule,
    FormsModule,
    MatIconModule,
  ],
})
export class IMyProfileModule {}
