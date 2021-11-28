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
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-c4-role-detail',
  templateUrl: './c4-role-detail.component.html',
  styleUrls: ['./c4-role-detail.component.scss'],
})
export class C4RoleDetailComponent implements OnInit, AfterViewInit, OnDestroy {
  //subscription
  subscription: Subscription[] = [];

  /** for table */
  displayedColumns: string[] = [
    'select',
    'id',
    'IdParentMenu',
    'IsGroup',
    'Name',
    'Status',

    'edit',
  ];

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
      this.api.excuteAllByWhat(param, '106').subscribe((data) => {
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
  roleDatas: any[] = [];
  menuDatas: any[] = [];

  // role
  staff: any;

  // data permission
  isPermissionMenu1: boolean = false;

  // condition fillter
  conditonFilter: string = '';
  conditions: any[] = [];

  // input param
  paramIdRole: string = '';

  /**
   * constructor
   * @param api
   * @param dialog
   */
  constructor(
    private api: ApiService,
    public dialog: MatDialog,
    private router: Router,
    private route: ActivatedRoute
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

    // get parmeter
    this.route.params.subscribe((params) => {
      this.paramIdRole = params['id'];

      // load data user
      this.onLoadDataGrid();
      // this.onRoleSelectionChange(params['id']);
    });
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
  loadDataReference() {
    // load list Role
    this.getListRole();

    // load list menu
    this.getListMenu();
  }

  /**
   * get list Role
   */
  getListRole() {
    this.subscription.push(
      this.api.excuteAllByWhat({}, '200').subscribe((data) => {
        // data = data['result']; 
        this.roleDatas = data;
      })
    );
  }

  /**
   * get list menu
   */
  getListMenu() {
    this.subscription.push(
      this.api.excuteAllByWhat({}, '100').subscribe((data) => {
        // data = data['result']; 

        data.unshift({ id: '0', Name: 'Menu main' });

        this.menuDatas = data;
      })
    );
  }

  /**
   * get name display role by id
   */
  getDisplayRoleById(id) {
    return this.roleDatas.filter((e) => e.id == id)[0]?.Name;
  }

  /**
   * get name display menu by id
   */
  getDisplayMenuById(id) {
    return this.menuDatas.filter((e) => e.id == id)[0]?.Name;
  }

  /**
   * get name display status by id
   */
  getDisplayStatusById(id) {
    if (id == 0) return 'Không có quyền';
    if (id == 1) return 'Chỉ xem';
    if (id == 2) return 'Quản trị';
  }

  /**
   * on role Selection Change
   * @param id
   */
  onRoleSelectionChange(id) {
    const condition = { key: 'IdRole', value: id };

    // add new condition to list
    this.addNewConditionToList(condition);
  }

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
      idRole: this.paramIdRole,
      offset: Number(this.pageIndex * this.pageSize),
      limit: this.pageSize,
    };

    this.subscription.push(
      this.api.excuteAllByWhat(param, '109').subscribe((data) => {
        // process data
        // data = data['result'];  
         
        if (data.length > 0) {
          let arr = [];
          let firstId = data[0].id;
          arr.push({
            id: data[0].id,
            IdParentMenu: data[0].IdParentMenu,
            IsGroup: data[0].IsGroup,
            Name: data[0].Name,
            Slug: data[0].Slug,
            Icon: data[0].Icon,
            Position: data[0].Position,
            Status: data[0].Status == null ? '0' : data[0].Status+'',
            IdRoleDetail: data[0].IdRoleDetail,
            IdRoleDetail1: data[0].IdRoleDetail1,
          });

          for (let i = 0; i < data.length; i++) {
            // add parent node
            if (data[i].id != firstId) {
              arr.push({
                id: data[i].id,
                IdParentMenu: data[i].IdParentMenu,
                IsGroup: data[i].IsGroup,
                Name: data[i].Name,
                Slug: data[i].Slug,
                Icon: data[i].Icon,
                Position: data[i].Position,
                Status: data[i].Status == null ? '0' : data[i].Status+'',
                IdRoleDetail: data[i].IdRoleDetail,
                IdRoleDetail1: data[i].IdRoleDetail1,
              });

              // update first id
              firstId = data[i].id;
            }

            // add child node
            if (data[i].id1 != null) {
              arr.push({
                id: data[i].id1,
                IdParentMenu: data[i].IdParentMenu1,
                IsGroup: data[i].IsGroup1,
                Name: data[i].Name1,
                Slug: data[i].Slug1,
                Icon: data[i].Icon1,
                Position: data[i].Position1,
                Status: data[i].Status1 == null ? '0' : data[i].Status1 +'',
                IdRoleDetail: data[i].IdRoleDetail,
                IdRoleDetail1: data[i].IdRoleDetail1,
              });
            }
          } 
          // set data for table
          this.dataSource = new MatTableDataSource(arr);
        } else {
          this.dataSource = new MatTableDataSource([]);
        }

        this.dataSource.sort = this.sort;
        this.selection = new SelectionModel<any>(true, []);
      })
    );
  }

  /**
   * on Btn Role Click
   */
  onBtnRoleClick() {
    this.router.navigate(['/manager/c3-role']);
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
        this.api.excuteAllByWhat(param, '303').subscribe((data) => {
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
   * on Role Change
   * @param row
   */
  onRoleChange(row) {
    const param = {
      // if main menu then get IdRoleDetail
      id: row.IdParentMenu == '0' ? row.IdRoleDetail : row.IdRoleDetail1,
      IdRole: this.paramIdRole,
      IdMenu: row.id + '',
      Status: row.Status,
    };

    // check exists then update
    if (param.id != null) {
      this.api.excuteAllByWhat(param, '302').subscribe((data) => {
        this.api.showSuccess('Cập nhật quyền thành công');
        // load data user
        this.onLoadDataGrid();
      });
    } else {
      // if not exists then insert new
      this.api.excuteAllByWhat(param, '301').subscribe((data) => {
        this.api.showSuccess('Cập nhật quyền thành công');
        // load data user
        this.onLoadDataGrid();
      });
    }
  }

  /**
   * on insert data
   * @param event
   */
  onBtnInsertDataClick() {
    const dialogRef = this.dialog.open(C4RoleDetailDialog, {
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
   * on update data
   * @param event
   */
  onBtnUpdateDataClick(row) {
    const dialogRef = this.dialog.open(C4RoleDetailDialog, {
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
  selector: 'c4-role-detail-dialog',
  templateUrl: 'c4-role-detail-dialog.html',
  styleUrls: ['./c4-role-detail.component.scss'],
})
export class C4RoleDetailDialog implements OnInit, OnDestroy {
  observable: Observable<any>;
  observer: Observer<any>;
  type: number;

  //subscription
  subscription: Subscription[] = [];

  // init input value
  input: any = {
    IdRole: '',
    IdMenu: '',
    Status: '',
  };

  //form
  form: FormGroup;

  // status
  status: any[] = [
    { key: '0', value: 'Không có quyền' },
    { key: '1', value: 'Chỉ xem' },
    { key: '2', value: 'Quản trị' },
  ];

  // data reference binding
  roleDatas: any[] = [];
  menuDatas: any[] = [];

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
    public dialogRef: MatDialogRef<C4RoleDetailDialog>,
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
      IdRole: [null, [Validators.required]],
      IdMenu: [null, [Validators.required]],
      Status: [null, [Validators.required]],
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
  loadDataReference() {
    // load list Role
    this.getListRole();

    // load list menu
    this.getListMenu();
  }

  /**
   * get list Role
   */
  getListRole() {
    this.subscription.push(
      this.api.excuteAllByWhat({}, '200').subscribe((data) => {
        // data = data['result']; 
        this.roleDatas = data;
      })
    );
  }

  /**
   * get list menu
   */
  getListMenu() {
    this.subscription.push(
      this.api.excuteAllByWhat({}, '100').subscribe((data) => {
        // data = data['result']; 
        this.menuDatas = data;
      })
    );
  }

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
          .excuteAllByWhat(this.input, '' + Number(301 + this.type) + '')
          .subscribe((data) => {
            this.dialogRef.close(true);
            this.api.showSuccess('Xử lý thành công ');
          })
      );
    }
  }
}
