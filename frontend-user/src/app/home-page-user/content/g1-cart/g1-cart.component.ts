import { Component, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { Observer } from 'rxjs/internal/types';
import { ApiService } from 'src/app/common/api-service/api.service';
import { SendDataService } from 'src/app/common/api-service/send-data.service';
import { SEOService } from 'src/app/common/api-service/seo.service';
@Component({
  selector: 'app-g1-cart',
  templateUrl: './g1-cart.component.html',
  styleUrls: ['./g1-cart.component.scss']
})
export class G1CartComponent implements OnInit, AfterViewInit, OnDestroy {

  subscription: Subscription[] = [];
  observable: Observable<any>;
  observer: Observer<any>;

  // data reference binding
  ShopMenuDatas: any[] = [];
  PackageDatas: any[] = [];

  //idUser
  idUser: string;
  idOrderMenu: string;
  isSummit: boolean = false;
  idDel: string;
  isDisabled: boolean = false;

  //dataOderMenu
  dataOderMenu: any[] = [];
  count = 0;
  totalPrice = 0;
  pricePackage = 0;
  priceShopMenu = 0;
  costShip: number = 0;

  // url Current
  urlCurrent: string = '';

  constructor(private api: ApiService,
    private activatedRoute: ActivatedRoute, private router: Router,
    private sendDataService: SendDataService, private seoService: SEOService) {
    // xử lý bất đồng bộ
    this.observable = Observable.create((observer: any) => {
      this.observer = observer;
    });
    this.idUser = this.api.getStaffValue?.id
  }

  /**
   * ngOnInit
   */
  ngOnInit(): void {
    this.getListShopmenu();
    this.getListDataShop();

    this.observable.subscribe(data => {

      if (data == 2) {
        this.getLoadDataUserOder();
      }
    });

  }

  /**
  * ng After View Init
  */
  ngAfterViewInit(): void {
    // scroll top screen
    this.api.scrollToTop();
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
   * get list ShopMenu
   */
  getListShopmenu() {
    this.subscription.push(this.api.excuteAllByWhat({}, '700')
      .subscribe((result) => {
        this.ShopMenuDatas = result;
        //Seo title
        let title = 'List-cart';
        this.seoService.setTitle(title);

        this.count += 1;
        this.observer.next(this.count);
      })
    );
  }

  /**                                                            
* getListDataShop
*/
  getListDataShop() {
    this.subscription.push(this.api.excuteAllByWhat({}, '800')
      .subscribe((result) => {
        if (result.length > 0) {
          this.costShip = Number(result[0]?.Ship)
        }

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
   * getDisplayShopmenuThumbnailById   
   */
  getDisplayShopmenuThumbnailById(id) {
    return this.ShopMenuDatas.filter((e) => e.id == id)[0]?.Thumbnail;
  }

  /**                                                            
   * getDisplayShopmenuPriceCurrentById   
   */
  getDisplayShopmenuPriceCurrentById(id) {
    return this.ShopMenuDatas.filter((e) => e.id == id)[0]?.PriceCurrent;
  }

  /**                                                            
   * get list Package
   */
  getListPackage() {
    this.subscription.push(this.api.excuteAllByWhat({}, '1200')
      .subscribe((result) => {
        let data = result.data;
        this.PackageDatas = data;

        this.count += 1;
        this.observer.next(this.count);
      })
    );
  }

  /**                                                            
   * get name display Package by id     
   */
  getDisplayPackageById(id) {
    return this.PackageDatas.filter((e) => e.id == id)[0]?.Name;
  }

  /**                                                            
   * get name display Package by id     
   */
  getDisplayPackageImageById(id) {
    return this.PackageDatas.filter((e) => e.id == id)[0]?.Image;
  }

  /**                                                            
  * get name display Package by id     
  */
  getDisplayPackagePriceById(id) {
    return this.PackageDatas.filter((e) => e.id == id)[0]?.Price;
  }

  /**
   * getLoadDataUserOder
   */
  getLoadDataUserOder() {
    const param = {
      'IdUser': this.idUser
    }
    this.subscription.push(
      this.api.excuteAllByWhat(param, '2815').subscribe((result) => {

        if (result.length > 0) {
          result.forEach(item => {
            item.PriceShopMenu = this.ShopMenuDatas.filter((e) => e.id == item.IdShop)[0]?.PriceCurrent;
            item.Amount = item.Amount;
          })
          this.dataOderMenu = result;
          this.idOrderMenu = result[0].IdOrderShop;

          this.getTotalPrice();
        } else {
          this.dataOderMenu = [];
        }
      })
    );
  }

  /**
   * onFocusoutAmount
   * @param id 
   * @param event 
   */
  onFocusoutAmount(id, event) {
    if (Number(event.srcElement.value) <= 0) {
      this.api.showWarning('Số lượng phải lớn hơn 0');
      this.isSummit = true;
    } else {
      this.isSummit = false;
      const param = {
        'id': id,
        'Amount': event.srcElement.value
      }
      this.subscription.push(
        this.api.excuteAllByWhat(param, '4107').subscribe((result) => {
          let data = result.data;
          if (data) {
            this.api.showSuccess('Cập nhật thành công');
            this.getLoadDataUserOder();
          }
        }));
    }
  }

  /**
   * getPriceProduct
   * @param price 
   * @param amount 
   */
  getPriceProduct(price, amount) {
    return price * amount;
  }

  /**
   * getTotalPrice
   */
  getTotalPrice() {
    this.totalPrice = 0;
    this.priceShopMenu = 0;
    this.dataOderMenu.forEach((item) => {

      this.priceShopMenu += (Number(item.PriceShopMenu) * Number(item.Amount));

    });
    this.totalPrice = this.priceShopMenu + this.costShip;

  }

  /**
   * onBtnGetIdDelClick
   */
  onBtnGetIdDelClick(id) {
    this.idDel = id;
  }

  /**
 * onStepDown
 */
  onStepDown(id, amount) {
    if (amount > 1) {
      const param = {
        id: id
      }
      this.subscription.push(
        this.api.excuteAllByWhat(param, '2911').subscribe((result) => {
          if (result) {
            this.getLoadDataUserOder();
            this.sendDataService.setDataSend('true');
          } else {
            this.api.showWarning('Xử lý thất bại. Vui lòng thử lại');
          }
        }));
    }

  }

  /**
   * onStepUp
   * @param id 
   */
  onStepUp(id) {
    const param = {
      id: id
    }
    this.subscription.push(
      this.api.excuteAllByWhat(param, '2912').subscribe((result) => {
        if (result) {
          this.getLoadDataUserOder();
          this.sendDataService.setDataSend('true');
        } else {
          this.api.showWarning('Xử lý thất bại. Vui lòng thử lại');
        }
      }));

  }

  /**
   * onBtnDelClick
   */
  onBtnDelClick() {
    const param = {
      'listid': this.idDel
    }
    this.subscription.push(
      this.api.excuteAllByWhat(param, '2903').subscribe((result) => {

        if (result) {
          this.getLoadDataUserOder();
          this.sendDataService.setDataSend('true');
        } else {
          this.api.showWarning('Xử lý thất bại. Vui lòng thử lại');
        }
      }));
  }

  /**
   * onSummitClick
   */
  onSummitClick() {
    this.isDisabled = true;
    const url = '/h1-payment/' + this.idOrderMenu + '/thanh-toan'

    const param = {
      'id': this.idOrderMenu,
      'TotalPrice': this.totalPrice
    }
    this.subscription.push(
      this.api.excuteAllByWhat(param, '2816').subscribe((result) => {
        if (result) {
          this.api.showSuccess('Vui lòng điền đầy đủ thông tin để đặt hàng');
          this.router.navigate([url.toLowerCase()]);
        }
      })
    );
  }

  /**
  * onClickMenu
  * @param idCategories 
  * @param title 
  */
  onClickMenu(idCategories, title) {
    let formatTitle = this.api.cleanAccents(title).split(' ').join('-');
    let re = /\//gi;
    formatTitle = formatTitle.replace(re, '');
    const url = '/c1-list-menu/' + idCategories + '/' + formatTitle;
    this.router.navigate([url.toLowerCase()]);
  }

}
