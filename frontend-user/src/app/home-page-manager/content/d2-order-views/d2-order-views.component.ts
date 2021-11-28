import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, Observer, Subscription } from 'rxjs';
import { ApiService } from 'src/app/common/api-service/api.service';
import { SEOService } from 'src/app/common/api-service/seo.service';

@Component({
  selector: 'app-d2-order-views',
  templateUrl: './d2-order-views.component.html',
  styleUrls: ['./d2-order-views.component.scss']
})
export class D2OrderViewsComponent implements OnInit, OnDestroy {

  /** for table */
  subscription: Subscription[] = [];
  observable: Observable<any>;
  observer: Observer<any>;

  //data binding
  IdOrderShop: string = '';
  IdMenu: string = '';

  dataOderMenu: any[] = [];
  ShopMenuDatas: any[] = [];
  count = 0;
  totalPrice: number = 0;


  idOrderStar: string = '';

  idShopStar: string = '';
  isShowReview: boolean = false;

  /**
  * constructor
  * @param api
  * @param formBuilder
  */
  constructor(private api: ApiService,
    private seoService: SEOService,
    private activatedRoute: ActivatedRoute,
    private router: Router) {
    // xử lý bất đồng bộ
    this.observable = Observable.create((observer: any) => {
      this.observer = observer;
    });
  }

  /**
   * ngOnInit
   */
  ngOnInit() {
    this.api.scrollToTop();
    let title = 'Order Detail';
    //Seo title
    this.seoService.setTitle(title);

    this.activatedRoute.params.subscribe(params => {
      this.IdOrderShop = params['idOrderShop'];
      this.IdMenu = params['idMenu'];
      if (params['idMenu'] == 2) {
        this.isShowReview = true;
      }

      this.getListShopmenu();
      this.observable.subscribe(data => {
        if (data == 1) {
          this.getLoadDataUserOder();
        }
      });
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
   * getLoadDataUserOder
   */
  getLoadDataUserOder() {
    const param = {
      'id': this.IdOrderShop
    }
    this.subscription.push(
      this.api.excuteAllByWhat(param, '2913').subscribe((result) => {
        let stt = 1;
        if (result.length > 0) {
          this.totalPrice = result[0].TotalPrice;
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
* get Thumbnaildisplay ShopMenu by id     
*/
  getDisplayPriceCurrentShopmenuById(id) {
    return this.ShopMenuDatas.filter((e) => e.id == id)[0]?.PriceCurrent;
  }

  /**
   * onBackClick
   */
  onBackClick() {
    let url = '';
    if (this.IdMenu == '1') {
      url = '/manager/admin/d1-oderplaced/Orders-placed';
    } else {
      if (this.IdMenu == '2') {
        url = '/manager/admin/d1-orderreceived/Order-received';
      } else {
        url = '/manager/admin/d1-odercanceled/Order-refuse';
      }
    }

    this.router.navigate([url.toLowerCase()]);
  }

  /**
 * getViewStar
 */
  getViewStar(item) {
    this.idOrderStar = item.id;
    this.idShopStar = item.IdShop;
  }

  /**
 * btnUpdateStar
 */
  btnUpdateStar(number) {
    const param = {
      idOrderStar: this.idOrderStar,
      idShopStar: this.idShopStar,
      number: number
    }
    this.subscription.push(
      this.api.excuteAllByWhat(param, '2914').subscribe((result) => { 
        if (result) {
          this.api.showSuccess('Cảm ơn bạn đã đánh giá');
        } else {
          this.api.showError('Xử lý không thành công ')
        }


      })
    );
  }

}
