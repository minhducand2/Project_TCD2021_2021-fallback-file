import { Component, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { ApiService } from 'src/app/common/api-service/api.service';
import { Router } from '@angular/router';
import { Subscription, Observable, Observer } from 'rxjs';
import { SendDataService } from 'src/app/common/api-service/send-data.service';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss'],
})
export class MenuComponent implements OnInit, AfterViewInit, OnDestroy {
  subscription: Subscription[] = [];

  // language: string;

  // observable
  observable: Observable<any>;
  observer: Observer<any>;

  // data binding
  headerInfo = {
    Phone: '',
    Mail: '',
    mailHref: '',
    phoneHref: '',
    Logo: '',
  };

  keyword: string = '';

  dataMenu: any;
  amount: number = 0;
  idUser: string;
  infoUser: any;
  userValue: any = [];

  // binding logo
  logo: string = '';
  avatar: string = '';
  Promotion: any = {
    Name: '',
    Link: '',
  };
  isCheckPromotion: boolean = false;

  menuParents = [
    {
      id: '',
      IdParent: '',
      Link: '',
      Name: '',
      Position: '',
    },
  ];

  menuChild = [
    {
      id: '',
      IdParent: '',
      Link: '',
      Name: '',
      Position: '',
    },
  ];

  listCateRecipe: any;
  shopMenuCatgores: any[] = [];
  shopMenuMobile: any[] = [];
  isMobile: boolean = false;
  isShowMenuMobile: boolean = false;
  isDisplayNone: boolean = true;
  isClassShow: boolean = false;
  isClass1Show: boolean = false;
  isClass2Show: boolean = false;
  isClassShowAccountMobile: boolean = false;

  // candidateValue login

  /**
   * constructor
   * @param api
   */
  constructor(
    public api: ApiService,
    private sendDataService: SendDataService,
    private router: Router
  ) {
    // xử lý bất đồng bộ
    this.observable = Observable.create((observer: any) => {
      this.observer = observer;
    });
    this.idUser = this.api.getStaffValue?.id;
    this.userValue = this.api.getStaffValue;
  }

  /**
   * ngOnInit
   */
  ngOnInit() {
    // this.onGetDataAll();

    this.getHeaderInfo();
    this.getShopMenuCategories();

    // update dataMenu
    this.sendDataService.data$.subscribe((data) => {
      this.getDataCart();
    });

    if (this.userValue != null) {
      this.infoUser = this.userValue;
    }
    if (this.idUser != null || this.idUser != undefined) {
      this.getDataCart();
    }

  }

  /**
   * ngAfterViewInit
   */
  ngAfterViewInit() {

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
  * isMobileDevice
  */
  isMobileDevice() {
    var userAgent = navigator.userAgent;
    if (/Android|webOS|iPhone|iPod|iPad|BlackBerry|IEMobile|Opera Mini/i.test(userAgent)) {
      // true for mobile device
      this.isMobile = true;
    } else {
      // false for not mobile device
      this.isMobile = false;
    }
  };

  /**
   * logOut
   */
  logOut() {
    this.api.logoutStaff();
  }

  /**
   * getDataCart
   */
  getDataCart() {
    const param = {
      IdUser: this.idUser,
    };
    this.subscription.push(
      this.api.excuteAllByWhat(param, '2813').subscribe((result) => {
        this.amount = result.count;
      })
    );
  }

  /**
   * getHeaderInfo
   */
  getHeaderInfo() {
    const param = {};
    this.subscription.push(
      this.api.excuteAllByWhat(param, '600').subscribe((result) => {
        if (result.length > 0) {
          this.headerInfo = result[0];
          result[0].phoneHref = 'tel:' + this.headerInfo.Phone;
          result[0].mailHref = 'mailto:' + this.headerInfo.Mail;
        }
      })
    );
  }

  /**
   * getTopShopMenu
   */
  getDataMenuHead() {
    const param = {};
    this.subscription.push(
      this.api.excuteAllByWhat(param, '500').subscribe((result) => {
        let data = result.data;

      })
    );
  }

  /**
   * navigatePage
   * @param item
   */
  navigatePage(cateRecipe): void {
    let formatTitle = this.api.cleanAccents(cateRecipe.Name).split(' ').join('-');
    let re = /\//gi;
    formatTitle = formatTitle.replace(re, '');
    this.router.navigate(['b1-list-recipe' + '/' + cateRecipe.id + '/' + formatTitle]);
  }

  /**
   * navigatePageShopMenu
   * @param item
   */
  navigatePageShopMenu(id, title): void {
    let formatTitle = this.api.cleanAccents(title).split(' ').join('-');
    let re = /\//gi;
    formatTitle = formatTitle.replace(re, '');
    const url = 'c1-list-menu/' + id + '/' + formatTitle;
    this.router.navigate([url.toLowerCase()]);
  }

  /**
   * navigatePageSearch
   */
  navigatePageSearch() {
    if (this.keyword == '') {
      this.api.showWarning('Vui lòng nhập từ khoá')
    } else {
      let formatTitle = this.keyword.split(' ').join('-');
      let re = /\//gi;
      formatTitle = formatTitle.replace(re, '');
      const url = 'l1-search-menu/' + formatTitle + '/' + 'search';
      this.router.navigate([url.toLowerCase()]);
      this.keyword = '';
    }

  }

  /**
   * getCateRecipe
   */
  getCateRecipe() {
    const param = {};
    this.subscription.push(
      this.api.excuteAllByWhat(param, '2300').subscribe((result) => {
        let data = result.data;
        if (result.status == true && data.length > 0) {
          this.listCateRecipe = data;
        }
      })
    );
  }

  /**
   * get Shop Menu Categories
   */
  getShopMenuCategories() {
    const param = {};
    this.subscription.push(
      this.api.excuteAllByWhat(param, '1100').subscribe((result) => {
        if (result.length > 0) {
          this.shopMenuCatgores = result;
        }
      })
    );
  }

  /**
   * onBtnLoginSocialClick
   */
  onBtnLoginSocialClick() {
    const url = '/login/dang-nhap';
    localStorage.setItem('lastUrl', this.router.url);
    this.router.navigate([url.toLowerCase()]);
  }


  /**
  * @param id onMenuClick
  * 
  */
  onMenuClick(id, title) {
    let formatTitle = this.api.cleanAccents(title).split(' ').join('-');
    let re = /\//gi;
    formatTitle = formatTitle.replace(re, '');
    const url = '/c2-detail-menu/' + id + '/' + formatTitle;

    this.router.navigate([url.toLowerCase()]);
  }

}
