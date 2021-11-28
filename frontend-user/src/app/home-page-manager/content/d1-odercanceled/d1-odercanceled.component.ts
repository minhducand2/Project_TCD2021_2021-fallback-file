import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Observable, Observer, Subscription } from 'rxjs';
import { ApiService } from 'src/app/common/api-service/api.service';
import { MustMatch } from 'src/app/common/validations/must-match.validator';
import { Md5 } from 'ts-md5';
import { SEOService } from 'src/app/common/api-service/seo.service';
import { ViewChild } from '@angular/core';
import { ElementRef } from '@angular/core';
import { Router } from '@angular/router';
@Component({
  selector: 'app-d1-odercanceled',
  templateUrl: './d1-odercanceled.component.html',
  styleUrls: ['./d1-odercanceled.component.scss']
})
export class D1OdercanceledComponent implements OnInit,OnDestroy {

  /** for table */
  subscription: Subscription[] = [];
  observable: Observable<any>;
  observer: Observer<any>;


  // data binding
  staff = {
    id: '',
    IdUserStatus: '',
    Fullname: '',
    Email: '',
    Password: '',
    Avatar: '',
    IdRole: '',
    CreatedAt: '',
    UpdatedAt: '',
    IdAccount: '',
    Phone: '',
  };

  staffInfoLogin = {
    id: '',
    Fullname: '',
    Email: '',
    Avatar: '',
    UpdatedAt: '',
    Phone: '',
    Address: '',
    IdCity: '',
    IdDistrict: '',
    Sex: '',
  };
  dataOderMenu: any[] = [];
  ShopMenuDatas: any[] = [];
  ListOrderStauts: any[] = [];
  count = 0;

  // binding uploads image or file
  @ViewChild("inputImageThumbnail", { static: false }) inputImageThumbnail: ElementRef;
  ImagesThumbnailPath: string = "";


  /**
   * constructor
   * @param api
   * @param formBuilder
   */
  constructor(private api: ApiService,
    private formBuilder: FormBuilder,
    private seoService: SEOService,
    private router: Router,) {
    // get staff value
    this.staff = this.api.getStaffValue;
    // xử lý bất đồng bộ
    this.observable = Observable.create((observer: any) => {
      this.observer = observer;
    });
  }

  /**
   * ngOnInit
   */
  ngOnInit(): void {

    this.api.scrollToTop();

    let title = 'OrderRefuse';
    //Seo title
    this.seoService.setTitle(title);
    this.getListShopmenu();
    this.getListOrderStatus();
    this.observable.subscribe(data => {

      if (data == 2) {
        this.getDataStaffLogin();
      }
    });


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
     * get Data Staff Login
     */
  getDataStaffLogin() {
    const param = {
      'id': this.staff.id
    };
    // get data staff login
    this.subscription.push(this.api.excuteAllByWhat(param, '2004').subscribe((result) => {
      if (result) {
        // set logo default
        if (result['Avatar'] == '' || result['Avatar'] == undefined) {
          result['Avatar'] = "https://rolievn.com/api/uploads/images/logo-admin.png"
        }
        this.staffInfoLogin = result;
        this.getLoadDataUserOder();
      } else {
        this.api.showError('Vui lòng tải lại trang');
      }
    })
    );

  }


  /**
   * getLoadDataUserOder
   */
  getLoadDataUserOder() {
    const param = {
      'IdUser': this.staffInfoLogin.id
    }
    this.subscription.push(
      this.api.excuteAllByWhat(param, '2820').subscribe((result) => {
        let stt = 1;
        if (result.length > 0) {
          result.forEach(element => {
            element.stt = stt++
          });
          this.dataOderMenu = result;

        } else {
          this.dataOderMenu = [];
        }

      })
    );
  }

  /**                                                            
  * get list ShopMenu
  */
  getListShopmenu() {
    this.subscription.push(this.api.excuteAllByWhat({}, '700')
      .subscribe((result) => {
        this.ShopMenuDatas = result;
        this.count += 1;
        this.observer.next(this.count);
      })
    );
  }

  /**                                                            
 * get list ShopMenu
 */
  getListOrderStatus() {
    this.subscription.push(this.api.excuteAllByWhat({}, '2200')
      .subscribe((result) => {
        this.ListOrderStauts = result;
        this.count += 1;
        this.observer.next(this.count);
      })
    );
  }

  /**                                                            
  * get name display ShopMenu by id     
  */
  getDisplayShopmenuById(id) {
    return this.ShopMenuDatas.filter((e) => e.id == id)[0]?.Title;
  }

  /**                                                            
 * get Thumbnaildisplay ShopMenu by id     
 */
  getDisplayThumbnailShopmenuById(id) {
    return this.ShopMenuDatas.filter((e) => e.id == id)[0]?.Thumbnail;
  }

  /**                                                            
* getDisplayOrderStatusById   
*/
  getDisplayOrderStatusById(id) {
    return this.ListOrderStauts.filter((e) => e.id == id)[0]?.Name;
  }

  /**
   * onRecipeClick
   * @param id
   */
  onViewsClick(id) {
    const url = '/manager/admin/d2-order-views/' + id + '/' + '3' + '/orderview';
    this.router.navigate([url.toLowerCase()]);
  }

  onClickResetOrder(item) {
    const param = {
      id: item.id,
      IdUser: item.IdUser
    }

    this.subscription.push(this.api.excuteAllByWhat(param, '2821')
      .subscribe((result) => {
        if (result) {
          const url = '/g1-cart/cart';
          this.router.navigate([url]);
          this.api.showSuccess('Cảm ơn bạn đã đặt lại đơn hàng');
        } else {
          this.api.showError('Xử lý thất bại. Vui lòng thử lại')
        }
      })
    );
  }



}
