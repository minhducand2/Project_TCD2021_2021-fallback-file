import {
  Component,
  OnInit,
  OnDestroy,
  ViewChild,
  ElementRef,
} from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { ApiService } from 'src/app/common/api-service/api.service';
import { Staff } from '../../../common/models/140staff.models';
import { SEOService } from 'src/app/common/api-service/seo.service';
@Component({
  selector: 'app-a1-home',
  templateUrl: './a1-home.component.html',
  styleUrls: ['./a1-home.component.scss'],
})
export class A1HomeComponent implements OnInit, OnDestroy {
  subscription: Subscription[] = [];

  input: Staff;
  isShow: true;
  // data binding
  aboutMealPlan = {
    id: '',
    Title: '',
    SubTitle: '',
    Content: '',
  };

  listTopMenu: any[];
  listTopRecipe: any[];
  listBanner: any[];
  listService: any[];
  listFeedback: any[];
  listShopProduct: any[];
  shopMenuCatgores: any[];
  listProductSuggestions: any[];
  listProductPromotion: any[];
  // Title intro Meal Plan
  positionKeyMealPlan: number;
  keyMealPlan: string;
  keyIntroMealPlan: string;

  getUrl: any;

  idUser: string;
  infoUser: any = null;
  userValue: any = [];
  ads: any = [];
  defaultImage: any = "assets/images/loading.gif";

  @ViewChild('openModal') openModal: ElementRef;

  // url Current
  urlCurrent: string = '';

  /**
   * constructor
   * @param api
   */
  constructor(
    public api: ApiService,
    private router: Router,
    private seoService: SEOService
  ) {
  }

  /**
   * ngOnInit
   */
  ngOnInit(): void {
    this.api.scrollToTop();
    setTimeout(() => {
      // this.onGetDataAll();
    }, 500);
    this.getBanner();
    this.getTopShopMenu();
    this.getShopMenuCategories();
    this.getProductSuggestions();
    this.getProductPromotion();

    // url Current
    this.urlCurrent = this.api.getUrl();
    let title = 'Home-page';
    // if (this.urlCurrent.indexOf('/en/') == -1) {
    //   title = 'Trang chủ';
    // } else {
    //   title = 'Home-page';
    // }

    //Seo title
    this.seoService.setTitle(title);
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
   * getBanner
   */
  getBanner() {
    const param = {};
    this.subscription.push(
      this.api.excuteAllByWhat(param, '407').subscribe((result) => {

        if (result.length > 0) {
          this.listBanner = result;
        } else {
          this.listBanner = [];
        }
      })
    );
  }


  /**
   * getTopShopMenu
   */
  getTopShopMenu() {
    const param = {};
    this.subscription.push(
      this.api.excuteAllByWhat(param, '708').subscribe((result) => {
        if (result.length > 0) {
          result.forEach((item) => {
            //slice Title
            if (item.Title.length > 40) {
              item.titleShort = item.Title.slice(0, 40) + '...';
            } else {
              item.titleShort = item.Title;
            }
            //slice desc
            let text = this.api.stripHtml(item.Description);
            if (item.Description.length > 65) {

              item.descShort = text.slice(0, 60) + '...';
            } else {
              item.descShort = text;
            }

            // check image valid
            if (item.Thumbnail == '' || item.Thumbnail == null) {
              item.Thumbnail = 'assets/images/no-image.png';
            }
            item.Star = Number(item.Star);
            item.PricePromotion = Number(item.PricePromotion);
            item.Percent = Math.ceil((Number(item.PricePromotion) - Number(item.PriceCurrent)) / Number(item.PricePromotion) * 100);
          });
          this.listTopMenu = result;
        } else {
          this.listTopMenu = [];
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
      this.api.excuteAllByWhat(param, '1107').subscribe((result) => {

        if (result.length > 0) {
          let colSlide = [];
          let rowSlide = [
            {
              colSlide: [result[0], result[1], result[2], result[3]],
            },
            {
              colSlide: [result[4], result[5], result[6], result[7]],
            },
            {
              colSlide: [result[8], result[9], result[10], result[11]],
            },
            {
              colSlide: [result[12], result[13], result[14], result[15]],
            }
          ];
          this.shopMenuCatgores = rowSlide;
        }
      })
    );
  }

  /**
   * getProductSuggestions
   */
  getProductSuggestions() {
    const param = {};
    this.subscription.push(
      this.api.excuteAllByWhat(param, '709').subscribe((result) => {
        if (result.length > 0) {
          result.forEach((item) => {
            //slice Title
            if (item.Title.length > 40) {
              item.titleShort = item.Title.slice(0, 40) + '...';
            } else {
              item.titleShort = item.Title;
            }
            //slice desc
            let text = this.api.stripHtml(item.Description);
            if (item.Description.length > 65) {

              item.descShort = text.slice(0, 60) + '...';
            } else {
              item.descShort = text;
            }

            // check image valid
            if (item.Thumbnail == '' || item.Thumbnail == null) {
              item.Thumbnail = 'assets/images/no-image.png';
            }
            item.Star = Number(item.Star);
            item.PricePromotion = Number(item.PricePromotion);
            item.Percent = Math.ceil((Number(item.PricePromotion) - Number(item.PriceCurrent)) / Number(item.PricePromotion) * 100);
          });
          this.listProductSuggestions = result;
        } else {
          this.listProductSuggestions = [];
        }
      })
    );
  }

  /**
   * getProductSuggestions
   */
  getProductPromotion() {
    const param = {};
    this.subscription.push(
      this.api.excuteAllByWhat(param, '710').subscribe((result) => {

        if (result.length > 0) {
          result.forEach((item) => {
            //slice Title
            if (item.Title.length > 40) {
              item.titleShort = item.Title.slice(0, 40) + '...';
            } else {
              item.titleShort = item.Title;
            }
            //slice desc
            let text = this.api.stripHtml(item.Description);
            if (item.Description.length > 65) {

              item.descShort = text.slice(0, 60) + '...';
            } else {
              item.descShort = text;
            }

            // check image valid
            if (item.Thumbnail == '' || item.Thumbnail == null) {
              item.Thumbnail = 'assets/images/no-image.png';
            }
            item.Star = Number(item.Star);
            item.PricePromotion = Number(item.PricePromotion);
            item.Percent = Math.ceil((Number(item.PricePromotion) - Number(item.PriceCurrent)) / Number(item.PricePromotion) * 100);
          });
          this.listProductPromotion = result;
        } else {
          this.listProductPromotion = [];
        }
      })
    );
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


  /**
   * onBtnLoginSocialClick
   */
  onBtnLoginSocialClick() {
    const url = '/login/dang-nhap';
    localStorage.setItem('lastUrl', '/');
    this.router.navigate([url.toLowerCase()]);
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
}
