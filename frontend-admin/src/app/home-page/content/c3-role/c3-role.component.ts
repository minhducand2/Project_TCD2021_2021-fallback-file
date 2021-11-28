import {
  Component,
  OnInit,
  Inject,
  ViewChild,
  OnDestroy,
  AfterViewInit,
  ElementRef,
} from '@angular/core';
import { ApiService } from '../../../common/api-service/api.service';
import {
  MatDialog,
  MatDialogRef,
  MAT_DIALOG_DATA,
} from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { Observable, Observer, Subscription } from 'rxjs';
import { SelectionModel } from '@angular/cdk/collections';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-c3-role',
  templateUrl: './c3-role.component.html',
  styleUrls: ['./c3-role.component.scss'],
})
export class C3RoleComponent implements OnInit, AfterViewInit, OnDestroy {
  //subscription
  subscription: Subscription[] = [];

  /** for table */
  displayedColumns: string[] = ['select', 'id', 'Name', 'Role', 'edit'];

  dataSource: MatTableDataSource<any>;

  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: false }) sort: MatSort;

  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  selection = new SelectionModel<any>(true, []);

  // update pagination
  pageIndex = 0;
  pageLength = 0;
  pageSize = 10;
  pageSizeOptions: number[] = [10, 20, 50, 100];

  /**
   * getLengthOfPage
   */
  getLengthOfPage() {
    const param = { condition: this.conditonFilter };
    this.subscription.push(
      this.api.excuteAllByWhat(param, '206').subscribe((data) => {
      
        // data = data['result']; 
        if (data.length > 0) {
          this.pageLength = data[0]['CountPage'];
        } else {
          this.pageLength = 0;
        }
      })
    );
  }

  /**
   * onPageChange
   */
  onPageChange(event) {
    this.pageSize = event.pageSize;
    this.pageIndex = event.pageIndex;

    this.onLoadDataGrid();
  }

  /** Whether the number of selected elements matches the total number of rows. */
  isAllSelected() {
    if (this.dataSource) {
      const numSelected = this.selection.selected.length;
      const numRows = this.dataSource.data.length;
      return numSelected === numRows;
    }
    return null;
  }

  /** Selects all rows if they are not all selected; otherwise clear selection. */
  masterToggle() {
    this.isAllSelected()
      ? this.selection.clear()
      : this.dataSource.data.forEach((row) => this.selection.select(row));
  }

  /** The label for the checkbox on the passed row */
  checkboxLabel(row?: any): string {
    if (!row) {
      return `${this.isAllSelected() ? 'select' : 'deselect'} all`;
    }
    return `${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${
      row.position + 1
    }`;
  }
  // end for table;

  // data reference binding

  // role
  staff: any;

  // data permission
  isPermissionMenu1: boolean = false;

  // condition fillter
  conditonFilter: string = '';
  conditions: any[] = [];

  /**
   * constructor
   * @param api
   * @param dialog
   */
  constructor(
    private api: ApiService,
    public dialog: MatDialog,
    private router: Router
  ) {
    // load permission
    this.onLoadPermission();
  }

  /**
   * ngOnInit
   */
  ngOnInit() {
    // load data reference
    this.loadDataReference();

    // load data user
    this.onLoadDataGrid();
  }

  /**
   * ng After View Init
   */
  ngAfterViewInit(): void {
    // scroll top screen
    window.scroll({ left: 0, top: 0, behavior: 'smooth' });
  }

  /**
   * ngOnDestroy
   */
  ngOnDestroy() {
    this.subscription.forEach((item) => {
      item.unsubscribe();
    });
  }

  /**
   * onLoadPermission
   */
  onLoadPermission() {
    this.staff = this.api.accountSubject.value;
    const param = {
      idUser: this.staff.id,
      url: this.router.url,
    };

    this.subscription.push(
      this.api.excuteAllByWhat(param, '111').subscribe((data) => { 
        // data = data['result']; 
        if (data.length > 0) {
          // Status is Admin
          if (data[0].Status == '2') {
            this.isPermissionMenu1 = true;
          }
        }
      })
    );
  }

  /**
   * load Data reference
   */
  loadDataReference() {}

  /**
   * add New Condition To List
   * @param condition
   */
  addNewConditionToList(condition) {
    // check exists
    let flg = false;
    let i;

    // check condition exists
    for (i = 0; i < this.conditions.length; i++) {
      if (this.conditions[i].key == condition.key) {
        flg = true;
        break;
      }
    }

    // remove old key
    if (flg) {
      this.conditions.splice(i, 1);
    }

    // insert new seach condition
    this.conditions.splice(0, 0, condition);

    // render new condition filter
    this.createConditionFilter();

    // load grid with new condition
    this.onLoadDataGrid();
  }

  /**
   * create Condition Filter
   */
  createConditionFilter() {
    this.conditonFilter = '';
    this.conditions.forEach((item) => {
      if (this.conditonFilter == '') {
        this.conditonFilter = item.key + " = '" + item.value + "'";
      } else {
        this.conditonFilter += ' AND ' + item.key + " = '" + item.value + "'";
      }
    });
    this.conditonFilter = ' WHERE ' + this.conditonFilter;
  }

  /**
   * get data grid
   */
  onLoadDataGrid() {
    // get Length Of Page
    this.getLengthOfPage();

    const param = {
      condition: this.conditonFilter,
      offset: Number(this.pageIndex * this.pageSize),
      limit: this.pageSize,
    };

    this.subscription.push(
      this.api.excuteAllByWhat(param, '205').subscribe((data) => {
        // data = data['result']; 
        if (data) {
          // set data for table
          this.dataSource = new MatTableDataSource(data);
        } else {
          this.dataSource = new MatTableDataSource([]);
        }

        this.dataSource.sort = this.sort;
        this.selection = new SelectionModel<any>(true, []);
      })
    );
  }

  /**
   * on Delete Click
   */
  onBtnDelClick() {
    // get listId selection example: listId='1,2,6'
    let listId = '';
    this.selection.selected.forEach((item) => {
      if (listId == '') {
        listId += item.id;
      } else {
        listId += ',' + item.id;
      }
    });

    const param = { listid: listId };

    // start delete
    if (listId !== '') {
      this.subscription.push(
        this.api.excuteAllByWhat(param, '203').subscribe((data) => {
          // load data grid
          this.onLoadDataGrid();

          //scroll top
          window.scroll({ left: 0, top: 0, behavior: 'smooth' });

          // show toast success
          this.api.showSuccess('Xóa thành công.');
        })
      );
    } else {
      // show toast warning
      this.api.showWarning('Vui lòng chọn 1 mục để xóa.');
    }
    this.selection = new SelectionModel<any>(true, []);
  }

  /**
   * on insert data
   * @param event
   */
  onBtnInsertDataClick() {
    const dialogRef = this.dialog.open(C3RoleDialog, {
      width: '100%',
      height: '100%',
      maxWidth: '100%',
      maxHeight: '100%',
      data: { type: 0, id: 0 },
      panelClass: 'custom-dialog',
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.onLoadDataGrid();
      }
    });
  }

  /**
   * on Btn Role Click
   * @param row
   */
  onBtnRoleClick(row) {
    this.router.navigate(['/manager/c4-role-detail', row.id]);
  }

  /**
   * on update data
   * @param event
   */
  onBtnUpdateDataClick(row) {
    const dialogRef = this.dialog.open(C3RoleDialog, {
      width: '100%',
      height: '100%',
      maxWidth: '100%',
      maxHeight: '100%',
      data: { type: 1, input: row },
      panelClass: 'custom-dialog',
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.onLoadDataGrid();
      }
    });
  }
}

/**
 * Component show thông tin để insert hoặc update
 */
@Component({
  selector: 'c3-role-dialog',
  templateUrl: 'c3-role-dialog.html',
  styleUrls: ['./c3-role.component.scss'],
})
export class C3RoleDialog implements OnInit, OnDestroy {
  observable: Observable<any>;
  observer: Observer<any>;
  type: number;

  //subscription
  subscription: Subscription[] = [];

  // init input value
  input: any = {
    Name: '',
  };

  //form
  form: FormGroup;

  // sex
  sexs: any[] = [
    { value: '1', viewValue: 'Nam' },
    { value: '0', viewValue: 'Nữ' },
  ];

  // data reference binding

  // binding uploads image or file

  // isUpdate
  isUpdate: boolean = false;

  /**
   * constructor
   * @param dialogRef
   * @param data
   * @param api
   * @param formBuilder
   */
  constructor(
    public dialogRef: MatDialogRef<C3RoleDialog>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private api: ApiService,
    private formBuilder: FormBuilder
  ) {
    this.type = data.type;

    // nếu là update
    if (this.type == 1) {
      this.input = data.input;
    }

    // add validate for controls
    this.form = this.formBuilder.group({
      Name: [null, [Validators.required]],
    });

    // xử lý bất đồng bộ
    this.observable = Observable.create((observer: any) => {
      this.observer = observer;
    });
  }

  /**
   * onInit
   */
  ngOnInit() {
    // get data reference
    this.loadDataReference();
  }

  /**
   * ngOnDestroy
   */
  ngOnDestroy() {
    this.subscription.forEach((item) => {
      item.unsubscribe();
    });
  }

  /**
   * load Data reference
   */
  loadDataReference() {}

  /**
   * on Btn Submit Click
   */
  onBtnSubmitClick(): void {
    // disable button update
    this.isUpdate = true;

    // touch all control to show error
    this.form.markAllAsTouched();
    // this.input.Born = this.api.formatDateDOTNET(new Date(this.input.Born));

    // check form pass all validate
    if (!this.form.invalid) {
      // if type = 0 insert else update
      this.subscription.push(
        this.api
          .excuteAllByWhat(this.input, '' + Number(201 + this.type) + '')
          .subscribe((data) => { 
            this.dialogRef.close(true);
            this.api.showSuccess('Xử lý thành công ');
          })
      );
    }
  }
}
