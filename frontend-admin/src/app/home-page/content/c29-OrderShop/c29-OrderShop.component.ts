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
  selector: 'app-c29-OrderShop',
  templateUrl: './c29-OrderShop.component.html',
  styleUrls: ['./c29-OrderShop.component.scss'],
})
export class C29OrdershopComponent implements OnInit, AfterViewInit, OnDestroy {

  //subscription
  subscription: Subscription[] = [];

  /** for table */
  displayedColumns: string[] = [
    'select',
    'id',
    'IdProductType',
    'IdUser',
    'IdOrderStatus',
    'IdCity',
    'IdDistrict',
    'IdPaymentStatus',
    'IdPaymentType',
    'Point',
    'TotalPrice',
    'view',
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
      this.api.excuteAllByWhat(param, '2806').subscribe((data) => {
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
  ProductTypeDatas: any[] = [];
  UserDatas: any[] = [];
  OrderStatusDatas: any[] = [];
  CityDatas: any[] = [];
  DistrictDatas: any[] = [];
  PaymentStatusDatas: any[] = [];
  PaymentTypeDatas: any[] = [];


  // role
  staff: any;

  // data permission
  isPermissionMenu1: boolean = false;

  // condition fillter
  conditonFilter: string = '';
  conditions: any[] = [];

  // model
  IdProductTypeModel: any = '0';
  IdUserModel: any = '0';
  IdOrderStatusModel: any = '0';
  IdCityModel: any = '0';
  IdDistrictModel: any = '0';
  IdPaymentStatusModel: any = '0';
  IdPaymentTypeModel: any = '0';
  maDH: string = '';

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
    // load list ProductType
    this.getListProducttype();

    // load list User
    this.getListUser();

    // load list OrderStatus
    this.getListOrderstatus();

    // load list City
    this.getListCity();

    // load list District
    this.getListDistrict();

    // load list PaymentStatus
    this.getListPaymentstatus();

    // load list PaymentType
    this.getListPaymenttype();


  }

  /**                                                            
   * get list ProductType
   */
  getListProducttype() {
    this.subscription.push(this.api.excuteAllByWhat({}, '2700')
      .subscribe((data) => {
        // data = data['result']; 
        data.unshift({ id: '0', Name: 'All' });
        this.ProductTypeDatas = data;
      })
    );
  }

  /**                                                            
   * get name display ProductType by id     
   */
  getDisplayProducttypeById(id) {
    return this.ProductTypeDatas.filter((e) => e.id == id)[0]?.Name;
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
   * get list OrderStatus
   */
  getListOrderstatus() {
    this.subscription.push(this.api.excuteAllByWhat({}, '2200')
      .subscribe((data) => {
        // data = data['result']; 
        data.unshift({ id: '0', Name: 'All' });
        this.OrderStatusDatas = data;
      })
    );
  }

  /**                                                            
   * get name display OrderStatus by id     
   */
  getDisplayOrderstatusById(id) {
    return this.OrderStatusDatas.filter((e) => e.id == id)[0]?.Name;
  }

  /**                                                            
   * get list City
   */
  getListCity() {
    this.subscription.push(this.api.excuteAllByWhat({}, '2500')
      .subscribe((data) => {
        // data = data['result']; 
        data.unshift({ id: '0', Name: 'All' });
        this.CityDatas = data;
      })
    );
  }

  /**                                                            
   * get name display City by id     
   */
  getDisplayCityById(id) {
    return this.CityDatas.filter((e) => e.id == id)[0]?.Name;
  }

  /**                                                            
   * get list District
   */
  getListDistrict() {
    this.subscription.push(this.api.excuteAllByWhat({}, '2600')
      .subscribe((data) => {
        // data = data['result']; 
        data.unshift({ id: '0', Name: 'All' });
        this.DistrictDatas = data;
      })
    );
  }

  /**                                                            
   * get name display District by id     
   */
  getDisplayDistrictById(id) {
    return this.DistrictDatas.filter((e) => e.id == id)[0]?.Name;
  }

  /**                                                            
   * get list PaymentStatus
   */
  getListPaymentstatus() {
    this.subscription.push(this.api.excuteAllByWhat({}, '2300')
      .subscribe((data) => {
        // data = data['result']; 
        data.unshift({ id: '0', Name: 'All' });
        this.PaymentStatusDatas = data;
      })
    );
  }

  /**                                                            
   * get name display PaymentStatus by id     
   */
  getDisplayPaymentstatusById(id) {
    return this.PaymentStatusDatas.filter((e) => e.id == id)[0]?.Name;
  }

  /**                                                            
   * get list PaymentType
   */
  getListPaymenttype() {
    this.subscription.push(this.api.excuteAllByWhat({}, '2400')
      .subscribe((data) => {
        // data = data['result']; 
        data.unshift({ id: '0', Name: 'All' });
        this.PaymentTypeDatas = data;
      })
    );
  }

  /**                                                            
   * get name display PaymentType by id     
   */
  getDisplayPaymenttypeById(id) {
    return this.PaymentTypeDatas.filter((e) => e.id == id)[0]?.Name;
  }



  /**                                                        
   * on ProductType Selection Change             
   * @param event                                            
   */
  onProductTypeSelectionChange(event) {
    const condition = { key: "IdProductType", value: event.value };

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
   * on OrderStatus Selection Change             
   * @param event                                            
   */
  onOrderStatusSelectionChange(event) {
    const condition = { key: "IdOrderStatus", value: event.value };

    // add new condition to list                             
    this.addNewConditionToList(condition);
  }
  /**                                                        
   * on City Selection Change             
   * @param event                                            
   */
  onCitySelectionChange(event) {
    const condition = { key: "IdCity", value: event.value };

    // add new condition to list                             
    this.addNewConditionToList(condition);
  }
  /**                                                        
   * on District Selection Change             
   * @param event                                            
   */
  onDistrictSelectionChange(event) {
    const condition = { key: "IdDistrict", value: event.value };

    // add new condition to list                             
    this.addNewConditionToList(condition);
  }
  /**                                                        
   * on PaymentStatus Selection Change             
   * @param event                                            
   */
  onPaymentStatusSelectionChange(event) {
    const condition = { key: "IdPaymentStatus", value: event.value };

    // add new condition to list                             
    this.addNewConditionToList(condition);
  }
  /**                                                        
   * on PaymentType Selection Change             
   * @param event                                            
   */
  onPaymentTypeSelectionChange(event) {
    const condition = { key: "IdPaymentType", value: event.value };

    // add new condition to list                             
    this.addNewConditionToList(condition);
  }

  /**                                                        
   * on ProductType Selection Change             
   * @param event                                            
   */
  onIdOrder(event) {
    if (event.srcElement.value > 0) {
      const condition = { key: "id", value: event.srcElement.value };
      // add new condition to list                             
      this.addNewConditionToList(condition);
    } else {
      this.conditonFilter = '';
      this.onLoadDataGrid();
    }
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
      if (this.conditonFilter == '') {
        this.conditonFilter = item.key + " = '" + item.value + "'";
      } else {
        this.conditonFilter += ' AND ' + item.key + " = '" + item.value + "'";
      }
    });
    if (this.conditonFilter != '') {
      this.conditonFilter = ' WHERE ' + this.conditonFilter;
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

    this.subscription.push(
      this.api.excuteAllByWhat(param, '2805').subscribe((data) => {
        // data = data['result']; 
        if (data.length > 0) {
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
        this.api.excuteAllByWhat(param, '2803').subscribe((data) => {
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
    const dialogRef = this.dialog.open(C29OrdershopDialog, {
      width: '100%',
      height: '100%',
      maxWidth: '100%',
      maxHeight: '100%',
      data: {
        type: 0,
        id: 0,
        IdProductTypeModel: this.IdProductTypeModel,
        IdUserModel: this.IdUserModel,
        IdOrderStatusModel: this.IdOrderStatusModel,
        IdCityModel: this.IdCityModel,
        IdDistrictModel: this.IdDistrictModel,
        IdPaymentStatusModel: this.IdPaymentStatusModel,
        IdPaymentTypeModel: this.IdPaymentTypeModel,

      },
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
    const dialogRef = this.dialog.open(C29OrdershopDialog, {
      width: '100%',
      height: '100%',
      maxWidth: '100%',
      maxHeight: '100%',
      data: {
        type: 1,
        input: row,
        ProductTypeDatas: this.ProductTypeDatas,
        UserDatas: this.UserDatas,
        OrderStatusDatas: this.OrderStatusDatas,
        CityDatas: this.CityDatas,
        DistrictDatas: this.DistrictDatas,
        PaymentStatusDatas: this.PaymentStatusDatas,
        PaymentTypeDatas: this.PaymentTypeDatas,
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
   * onBtnRouteOrder
   * @param row 
   */
  onBtnRouteOrder(row) {
    const url = '/manager/c30-OrderDetail' + '/' + row.id;
    this.router.navigate([url]);

  }


  /**
   * updateColor
   * @param status 
   */
  updateColor(status) {
    if (status == '1') {
      return '#00B74A'
    }
    if (status == '2') {
      return '#FFA900'
    }
    if (status == '3') {
      return '#1266F1'
    }
    if (status == '4') {
      return '#000000'
    }
    if (status == '5') {
      return '#ff4444'
    }
    return '#000'
  }
}

/**
 * Component show thông tin để insert hoặc update
 */
@Component({
  selector: 'c29-OrderShop-dialog',
  templateUrl: 'c29-OrderShop-dialog.html',
  styleUrls: ['./c29-OrderShop.component.scss'],
})
export class C29OrdershopDialog implements OnInit, OnDestroy {

  observable: Observable<any>;
  observer: Observer<any>;
  type: number;

  //subscription
  subscription: Subscription[] = [];

  // init input value
  input: any = {
    IdProductType: '',
    IdUser: '',
    IdOrderStatus: '',
    IdCity: '',
    IdDistrict: '',
    IdPaymentStatus: '',
    IdPaymentType: '',
    TotalPrice: '',
    PromotionCode: '',
    Name: '',
    Email: '',
    Phone: '',
    Address: '',
    Note: '',
    CreatedAt: '',
    UpdatedAt: '',

  };

  //form
  form: FormGroup;

  // sex
  sexs: any[] = [
    { value: '1', viewValue: 'Nam' },
    { value: '0', viewValue: 'Nữ' },
  ];

  // data reference binding
  ProductTypeDatas: any[] = [];
  UserDatas: any[] = [];
  OrderStatusDatas: any[] = [];
  CityDatas: any[] = [];
  DistrictDatas: any[] = [];
  PaymentStatusDatas: any[] = [];
  PaymentTypeDatas: any[] = [];


  // binding uploads image or file


  // isUpdate
  isUpdate: boolean = false;
  isCompletedOrder: boolean = false;
  isCanceledOrder: boolean = false;
  isErrWareHouse: boolean = false;
  listIdOrderMenu: string = '';

  listUserOrder: any[] = [];
  listProductWareHouse: any[] = [];
  listSumProductWareHouse: any[] = [];

  /**
   * constructor
   * @param dialogRef
   * @param data
   * @param api
   * @param formBuilder
   */
  constructor(
    public dialogRef: MatDialogRef<C29OrdershopDialog>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private api: ApiService,
    private formBuilder: FormBuilder
  ) {
    this.type = data.type;

    // nếu là update
    if (this.type == 1) {
      this.input = data.input;

      if (this.input.IdOrderStatus == '3' || this.input.IdOrderStatus == '4') {
        this.isCompletedOrder = true;
      } else {
        if (this.input.IdOrderStatus == '5') {
          this.isCanceledOrder = true;
        }
      }
    } else {
      // mapping data filter
      this.input.IdProductType = data.IdProductTypeModel;
      this.input.IdUser = data.IdUserModel;
      this.input.IdOrderStatus = data.IdOrderStatusModel;
      this.input.IdCity = data.IdCityModel;
      this.input.IdDistrict = data.IdDistrictModel;
      this.input.IdPaymentStatus = data.IdPaymentStatusModel;
      this.input.IdPaymentType = data.IdPaymentTypeModel;
      this.input.CreatedAt = new Date();
      this.input.UpdatedAt = new Date();
    }

    // add validate for controls
    this.form = this.formBuilder.group({
      IdProductType: [
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
      IdOrderStatus: [
        null,
        [
          Validators.required,
        ],
      ],
      IdCity: [
        null,
        [
          Validators.required,
        ],
      ],
      IdDistrict: [
        null,
        [
          Validators.required,
        ],
      ],
      IdPaymentStatus: [
        null,
        [
          Validators.required,
        ],
      ],
      IdPaymentType: [
        null,
        [
          Validators.required,
        ],
      ],
      TotalPrice: [
        null,
        [
          Validators.required,
        ],
      ],
      PromotionCode: [
        null,
        [
        ],
      ],
      Name: [
        null,
        [
        ],
      ],
      Email: [
        null,
        [
        ],
      ],
      Phone: [
        null,
        [
        ],
      ],
      Address: [
        null,
        [
        ],
      ],
      Note: [
        null,
        [
        ],
      ],
      CreatedAt: [
        null,
        [
        ],
      ],
      UpdatedAt: [
        null,
        [
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
    // load list ProductType
    this.getListProducttype();

    // load list User
    this.getListUser();

    // load list OrderStatus
    this.getListOrderstatus();

    // load list City
    this.getListCity();

    // load list District
    this.getListDistrict();

    // load list PaymentStatus
    this.getListPaymentstatus();

    // load list PaymentType
    this.getListPaymenttype();

    if (this.type == 1) {
      this.getListAmountWithIdOrder();
      // this.getListProductWare();
    }

  }

  /**                                                            
  *  getListAmountWithIdOrder
  */
  getListAmountWithIdOrder() {
    const param = {
      IdOrderShop: this.input.id,
      IdCity: this.input.IdCity
    }
    this.subscription.push(this.api.excuteAllByWhat(param, '2910')
      .subscribe((result) => {

        let data = result['amountUser'];
        if (data.length > 0 && !result['status']) {
          this.listUserOrder = data;
        } else {
          this.api.showWarning('Số lượng trong kho hiện không đủ. Vui lòng kiểm tra lại kho');
          this.isUpdate = true;
          this.isErrWareHouse = true;
        }
      })
    );
  }


  /**
   * getListProductWare
   */
  getListProductWare() {

    this.subscription.push(this.api.excuteAllByWhat({}, '3300')
      .subscribe((data) => {
        // data = data['result']; 
        if (data.length > 0) {
          data.forEach(element => {
            element.Amount = Number(element.Amount);
          });
          this.listProductWareHouse = data;

        }

      })
    );
  }


  /**                                                            
   * get list ProductType
   */
  getListProducttype() {
    this.subscription.push(this.api.excuteAllByWhat({}, '2700')
      .subscribe((data) => {
        // data = data['result']; 
        data.forEach(element => {
          element.id += '';
        });
        this.ProductTypeDatas = data;
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
   * get list OrderStatus
   */
  getListOrderstatus() {
    this.subscription.push(this.api.excuteAllByWhat({}, '2200')
      .subscribe((data) => {
        // data = data['result']; 
        data.forEach(element => {
          element.id += '';
        });
        this.OrderStatusDatas = data;
      })
    );
  }

  /**                                                            
   * get list City
   */
  getListCity() {
    this.subscription.push(this.api.excuteAllByWhat({}, '2500')
      .subscribe((data) => {
        // data = data['result']; 
        data.forEach(element => {
          element.id += '';
        });
        this.CityDatas = data;
      })
    );
  }

  /**                                                            
  * get name display City by id     
  */
  getDisplayCityById(id) {
    return this.CityDatas.filter((e) => e.id == id)[0]?.Name;
  }

  /**                                                            
   * get list District
   */
  getListDistrict() {
    this.subscription.push(this.api.excuteAllByWhat({}, '2600')
      .subscribe((data) => {
        // data = data['result']; 
        data.forEach(element => {
          element.id += '';
        });
        this.DistrictDatas = data;
      })
    );
  }

  /**                                                            
   * get list PaymentStatus
   */
  getListPaymentstatus() {
    this.subscription.push(this.api.excuteAllByWhat({}, '2300')
      .subscribe((data) => {
        // data = data['result']; 
        data.forEach(element => {
          element.id += '';
        });
        this.PaymentStatusDatas = data;
      })
    );
  }

  /**                                                            
   * get list PaymentType
   */
  getListPaymenttype() {
    this.subscription.push(this.api.excuteAllByWhat({}, '2400')
      .subscribe((data) => {
        // data = data['result']; 
        data.forEach(element => {
          element.id += '';
        });
        this.PaymentTypeDatas = data;
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
    this.input.CreatedAt = this.api.formatDateDOTNET(new Date(this.input.CreatedAt));
    this.input.UpdatedAt = this.api.formatDateDOTNET(new Date(this.input.UpdatedAt));
    this.listUserOrder.forEach(item => {
      item.IdShop1 = item.IdShop;
      item.UserAmount1 = item.UserAmount;
      item.WareHouseAmount1 = item.WareHouseAmount;
      item.IdCity = this.input.IdCity;
    });

    // check form pass all validate
    if (!this.form.invalid) {
      // if type = 0 insert else update
      this.subscription.push(this.api.excuteAllByWhat(this.input, '' + Number(2801 + this.type) + '')
        .subscribe((data) => {
          if (data) {
            if ((this.input.IdOrderStatus == '3' || this.input.IdOrderStatus == '4') && !this.isCompletedOrder) {

              //update ware house witt order completed
              const param = { data: this.listUserOrder };
              this.subscription.push(this.api.excuteAllByWhat(param, '3309')
                .subscribe((data) => {
                  if (data) {
                    this.api.showSuccess('Xử lý thành công!');
                    this.dialogRef.close(true);
                  }
                })
              );

            } else {
              if (this.input.IdOrderStatus == '5' && !this.isCanceledOrder) {

                //update ware house witt order canceled
                const param = { data: this.listUserOrder };
                this.subscription.push(this.api.excuteAllByWhat(param, '3310')
                  .subscribe((data) => {
                    if (data) {
                      this.api.showSuccess('Huỷ đơn thành công!');
                      this.dialogRef.close(true);
                    }
                  })
                );

              } else {

                this.api.showSuccess('Xử lý thành công!');
                this.dialogRef.close(true);
              }
            }


          } else {
            this.api.showError('Xử lý không thành công !');
            console.log('Error: ', data.error);
          }
        })
      );
    }
  }
}
