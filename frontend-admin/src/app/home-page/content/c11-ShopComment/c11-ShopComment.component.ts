import { Component, OnInit, Inject, ViewChild, OnDestroy, AfterViewInit, ElementRef } from '@angular/core';
import { ApiService } from '../../../common/api-service/api.service';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { Observable, Observer, Subscription } from 'rxjs';
import { SelectionModel } from '@angular/cdk/collections';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-c11-ShopComment',
  templateUrl: './c11-ShopComment.component.html',
  styleUrls: ['./c11-ShopComment.component.scss'],
})
export class C11ShopcommentComponent implements OnInit, AfterViewInit, OnDestroy {

  //subscription
  subscription: Subscription[] = [];

  /** for table */
  displayedColumns: string[] = [
    'select',
    'id',
    'IdShop',
    'IdUser',
    'IdCommentStatus',
    'Content',
    'CreatedAt',
    'reply',

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
      this.api.excuteAllByWhat(param, '1008').subscribe((data) => {
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
    return `${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.position + 1
      }`;
  }
  // end for table;

  // data reference binding
  ShopDatas: any[] = [];
  UserDatas: any[] = [];
  CommentStatusDatas: any[] = [];
  dataSourceTemp: any[] = [];
  StaffListDatas: any[] = [];


  // role
  staff: any;

  // data permission
  isPermissionMenu1: boolean = false;

  // condition fillter
  conditonFilter: string = '';
  conditions: any[] = [];

  // model
  IdShopModel: any = '0';
  IdUserModel: any = '0';
  IdCommentStatusModel: any = '0';


  /**
   * constructor
   * @param api
   * @param dialog
   */
  constructor(
    private api: ApiService,
    private router: Router,
    public dialog: MatDialog
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
    this.createConditionFilter();
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
  loadDataReference() {
    // load list Shop
    this.getListShop();

    // load list User
    this.getListUser();

    // load list CommentStatus
    this.getListCommentstatus();
    this.getListStaff();


  }

  /**                                                            
   * get list Shop
   */
  getListShop() {
    this.subscription.push(this.api.excuteAllByWhat({}, '700')
      .subscribe((data) => {
        // data = data['result']; 
        data.unshift({ id: '0', Title: 'All' });
        this.ShopDatas = data;
      })
    );
  }

  /**                                                            
   * get name display Shop by id     
   */
  getDisplayShopById(id) {
    return this.ShopDatas.filter((e) => e.id == id)[0]?.Title;
  }

  /**                                                            
   * get list User
   */
  getListUser() {
    this.subscription.push(this.api.excuteAllByWhat({}, '2000')
      .subscribe((data) => {
        // data = data['result']; 
        data.unshift({ id: '0', Fullname: 'All' });
        this.UserDatas = data;
      })
    );
  }

  /**                                                            
   * get name display User by id     
   */
  getDisplayUserById(id) {
    return this.UserDatas.filter((e) => e.id == id)[0]?.Fullname;
  }

  /**                                                            
   * get list CommentStatus
   */
  getListCommentstatus() {
    this.subscription.push(this.api.excuteAllByWhat({}, '3000')
      .subscribe((data) => {
        // data = data['result']; 
        data.unshift({ id: '0', Name: 'All' });
        this.CommentStatusDatas = data;
      })
    );
  }

  /**                                                            
   * get name display CommentStatus by id     
   */
  getDisplayCommentstatusById(id) {
    return this.CommentStatusDatas.filter((e) => e.id == id)[0]?.Name;
  }

  /**                                                            
  * get list CommentStatus
  */
  getListStaff() {
    this.subscription.push(this.api.excuteAllByWhat({}, '12')
      .subscribe((data) => {
        // data = data['result']; 
        this.StaffListDatas = data;
      })
    );
  }

  /**                                                            
   * get name display CommentStatus by id     
   */
  getDisplayStaffById(id) {
    return this.StaffListDatas.filter((e) => e.id == id)[0]?.name;
  }



  /**                                                        
   * on Shop Selection Change             
   * @param event                                            
   */
  onShopSelectionChange(event) {
    const condition = { key: "IdShop", value: event.value };

    // add new condition to list                             
    this.addNewConditionToList(condition);
  }
  /**                                                        
   * on User Selection Change             
   * @param event                                            
   */
  onUserSelectionChange(event) {
    const condition = { key: "IdUser", value: event.value };

    // add new condition to list                             
    this.addNewConditionToList(condition);
  }
  /**                                                        
   * on CommentStatus Selection Change             
   * @param event                                            
   */
  onCommentStatusSelectionChange(event) {
    const condition = { key: "IdCommentStatus", value: event.value };

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
      if ((this.conditions[i].key == condition.key)) {
        flg = true;
        break;
      }
    }

    // remove old key
    if (flg) {
      this.conditions.splice(i, 1);
    }

    // insert new seach condition if !=0
    if (condition.value != '0') {
      this.conditions.splice(0, 0, condition);
    }

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
      // if (this.conditonFilter == '') {
      //   this.conditonFilter = item.key + " = '" + item.value + "'";
      // } else {
      this.conditonFilter += ' AND ' + 't1.' + item.key + " = '" + item.value + "'";
      // }
    });
    if (this.conditonFilter != '') {
      this.conditonFilter = ' WHERE t1.IdTypeComment = 0' + this.conditonFilter;
    } else {
      this.conditonFilter = ' WHERE t1.IdTypeComment = 0'
    }

  }

  /**
   * get data grid
   */
  onLoadDataGrid() {
    // get Length Of Page
    this.getLengthOfPage();

    const param = {
      'condition': this.conditonFilter,
      'offset': Number(this.pageIndex * this.pageSize),
      'limit': this.pageSize
    };
    console.log("🚀 ~ param", param)

    this.dataSourceTemp = [];
    let ans = 1;

    this.subscription.push(
      this.api.excuteAllByWhat(param, '1007').subscribe((data) => {
        // data = data['result']; 
        data.forEach((item) => {
          item.IdCommentStatus += '';
          item.IdCommentStatus1 += '';
          item.IdShop += '';
          item.IdShop1 += '';
          item.IdTypeComment += '';
          item.IdTypeComment1 += '';
          item.IdUser += '';
          item.IdUser1 += '';
          // item.id += '';
          // item.id1 += '';
          item.IdStaff += '';
          item.IdStaff1 += '';
        })
        if (data.length > 0) {
          // process add first parent 
          let temp = {
            stt: ans++,
            id: data[0].id,
            IdShop: data[0].IdShop,
            IdUser: data[0].IdUser,
            IdCommentStatus: data[0].IdCommentStatus,
            Content: data[0].Content,
            CreatedAt: data[0].CreatedAt,
            IdTypeComment: data[0].IdTypeComment,
            IdStaff: data[0].IdStaff,
          };
          this.dataSourceTemp.push(temp);

          // add first to childs 
          if (data[0].id1 != null) {
            this.dataSourceTemp.push({
              stt: '--',
              id: data[0].id1,
              IdShop: data[0].IdShop1,
              IdUser: data[0].IdUser1,
              IdCommentStatus: data[0].IdCommentStatus1,
              Content: data[0].Content1,
              CreatedAt: data[0].CreatedAt1,
              IdTypeComment: data[0].IdTypeComment1,
              IdStaff: data[0].IdStaff1,
            });
          }

          // loop and add parent of all part
          for (let i = 1; i < data.length; i++) {
            if (temp.id != data[i].id) {
              temp = {
                stt: 0,
                id: data[i].id,
                IdShop: data[i].IdShop,
                IdUser: data[i].IdUser,
                IdCommentStatus: data[i].IdCommentStatus,
                IdTypeComment: data[i].IdTypeComment,
                Content: data[i].Content,
                CreatedAt: data[i].CreatedAt,
                IdStaff: data[i].IdStaff,
              };



              this.dataSourceTemp.push({
                stt: ans++,
                id: data[i].id,
                IdShop: data[i].IdShop,
                IdUser: data[i].IdUser,
                IdCommentStatus: data[i].IdCommentStatus,
                IdTypeComment: data[i].IdTypeComment,
                Content: data[i].Content,
                CreatedAt: data[i].CreatedAt,
                IdStaff: data[i].IdStaff,
              });

              // add first to childs 
              if (data[i].id1 != null) {
                this.dataSourceTemp.push({
                  stt: '--',
                  id: data[i].id1,
                  IdShop: data[i].IdShop1,
                  IdUser: data[i].IdUser1,
                  IdCommentStatus: data[i].IdCommentStatus1,
                  IdTypeComment: data[i].IdTypeComment1,
                  Content: data[i].Content1,
                  IdStaff: data[i].IdStaff1,
                  CreatedAt: data[i].CreatedAt1,
                });
              }

            } else {
              // add child
              if (data[i].id1 != null) {
                this.dataSourceTemp.push({
                  stt: '--',
                  id: data[i].id1,
                  IdShop: data[i].IdShop1,
                  IdUser: data[i].IdUser1,
                  IdCommentStatus: data[i].IdCommentStatus1,
                  IdTypeComment: data[i].IdTypeComment1,
                  Content: data[i].Content1,
                  CreatedAt: data[i].CreatedAt1,
                  IdStaff: data[i].IdStaff1,
                });
              }
            }
          }

          // set data for table
          this.dataSource = new MatTableDataSource(this.dataSourceTemp);

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
        this.api.excuteAllByWhat(param, '1003').subscribe((data) => {
          if (!data.isError) {
            // load data grid
            this.onLoadDataGrid();

            //scroll top
            window.scroll({ left: 0, top: 0, behavior: 'smooth' });

            // show toast success
            this.api.showSuccess('Xóa thành công.');
          } else {
            // load data grid
            this.onLoadDataGrid();

            //scroll top
            window.scroll({ left: 0, top: 0, behavior: 'smooth' });

            // show error
            this.api.showError('Xóa không thành công');
            console.log('Error: ', data.error);
          }
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
    const dialogRef = this.dialog.open(C11ShopcommentDialog, {
      width: '100%',
      height: '100%',
      maxWidth: '100%',
      maxHeight: '100%',
      data: {
        type: 0,
        id: 0,
        IdShopModel: this.IdShopModel,
        IdUserModel: this.IdUserModel,
        IdCommentStatusModel: this.IdCommentStatusModel,

      },
      panelClass: 'custom-dialog',
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {

      }
      this.onLoadDataGrid();
    });
  }

  /**
   * on update data
   * @param event
   */
  onBtnUpdateDataClick(row) {
    const dialogRef = this.dialog.open(C11ShopcommentDialog, {
      width: '100%',
      height: '100%',
      maxWidth: '100%',
      maxHeight: '100%',
      data: { type: 1, input: row },
      panelClass: 'custom-dialog',
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {

      }
      this.onLoadDataGrid();
    });
  }
  /**
   * onBtnReplyClick
   * @param row 
   */
  onBtnReplyClick(row) {
    const dialogRef = this.dialog.open(C11ShopcommentDialog, {
      width: '100%',
      height: '100%',
      maxWidth: '100%',
      maxHeight: '100%',
      data: { type: 2, input: row },
      panelClass: 'custom-dialog',
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {

      }
      this.onLoadDataGrid();
    });
  }
}

/**
 * Component show thông tin để insert hoặc update
 */
@Component({
  selector: 'c11-ShopComment-dialog',
  templateUrl: 'c11-ShopComment-dialog.html',
  styleUrls: ['./c11-ShopComment.component.scss'],
})
export class C11ShopcommentDialog implements OnInit, OnDestroy {

  observable: Observable<any>;
  observer: Observer<any>;
  type: number;

  //subscription
  subscription: Subscription[] = [];

  // init input value
  input: any = {
    IdShop: '',
    IdUser: '',
    IdCommentStatus: '',
    Content: '',
    CreatedAt: '',
    IdTypeComment: '',
    IdStaff: '',
  };
  staff: any;
  //form
  form: FormGroup;

  // sex
  sexs: any[] = [
    { value: '1', viewValue: 'Nam' },
    { value: '0', viewValue: 'Nữ' },
  ];

  // data reference binding
  ShopDatas: any[] = [];
  UserDatas: any[] = [];
  CommentStatusDatas: any[] = [];


  // binding uploads image or file


  // isUpdate
  isUpdate: boolean = false;
  checked: boolean = false;

  /**
   * constructor
   * @param dialogRef
   * @param data
   * @param api
   * @param formBuilder
   */
  constructor(
    public dialogRef: MatDialogRef<C11ShopcommentDialog>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private api: ApiService,
    private formBuilder: FormBuilder
  ) {
    this.type = data.type;
    this.staff = this.api.getAccountValue;
    // nếu là update
    if (this.type == 1) {
      this.input = data.input;
    } else {
      if (this.type == 2) {
        this.input = data.input;
        this.input.IdStaff = this.staff.id + '';
        this.input.IdTypeComment = data.input.id + '';
        this.input.Content = '';
        this.input.IdUser = '0';

      } else {
        this.input.IdTypeComment = '0';
        this.input.IdStaff = '0';
        // mapping data filter
        this.input.IdShop = data.IdShopModel;
        this.input.IdUser = data.IdUserModel;
        this.input.IdCommentStatus = data.IdCommentStatusModel;
        this.input.CreatedAt = new Date();
      }

    }

    // add validate for controls
    this.form = this.formBuilder.group({
      IdShop: [
        null,
        [
          Validators.required,
        ],
      ],
      IdUser: [
        null,
        [
          Validators.required,
        ],
      ],
      IdCommentStatus: [
        null,
        [
          Validators.required,
        ],
      ],
      Content: [
        null,
        [
          Validators.required,
        ],
      ],
      CreatedAt: [
        null,
        [
          Validators.required,
        ],
      ],

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
    this.loadDataReference()
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
    // load list Shop
    this.getListShop();

    // load list User
    this.getListUser();

    // load list CommentStatus
    this.getListCommentstatus();


  }

  /**                                                            
   * get list Shop
   */
  getListShop() {
    this.subscription.push(this.api.excuteAllByWhat({}, '700')
      .subscribe((data) => {
        // data = data['result']; 
        data.forEach(element => {
          element.id += '';
        });
        this.ShopDatas = data;
      })
    );
  }

  /**                                                            
   * get list User
   */
  getListUser() {
    this.subscription.push(this.api.excuteAllByWhat({}, '2000')
      .subscribe((data) => {
        // data = data['result']; 
        data.forEach(element => {
          element.id += '';
        });
        this.UserDatas = data;
      })
    );
  }

  /**                                                            
   * get list CommentStatus
   */
  getListCommentstatus() {
    this.subscription.push(this.api.excuteAllByWhat({}, '3000')
      .subscribe((data) => {
        // data = data['result']; 
        data.forEach(element => {
          element.id += '';
        });
        this.CommentStatusDatas = data;
      })
    );
  }

  onClickGetCheckbox(event) {

  }




  /**
   * on Btn Submit Click
   */
  onBtnSubmitClick(): void {
    // disable button update
    this.isUpdate = true;
    let what = '1001'
    if (this.type == 1) {
      what = '1002'
    }
    // touch all control to show error
    this.form.markAllAsTouched();
    this.input.CreatedAt = this.api.formatDateDOTNET(new Date(this.input.CreatedAt));

    // check form pass all validate
    if (!this.form.invalid) {
      // if type = 0 insert else update
      this.subscription.push(this.api.excuteAllByWhat(this.input, what)
        .subscribe((data) => {

          if (!data.isError) {
            this.dialogRef.close(true);
            this.api.showSuccess('Xử lý thành công !');
          } else {
            this.api.showError('Xử lý không thành công !');
            console.log('Error: ', data.error);
          }
        })
      );
    }
  }
}
