import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { ApiService } from 'src/app/common/api-service/api.service';
import $ from "jquery";
import { SEOService } from 'src/app/common/api-service/seo.service';

@Component({
  selector: 'app-l1-search-menu',
  templateUrl: './l1-search-menu.component.html',
  styleUrls: ['./l1-search-menu.component.scss']
})

export class L1SearchMenuComponent implements OnInit, OnDestroy {

  subscription: Subscription[] = [];

  // data binding
  listShopMenu: any;

  // data source for combobox languages
  listFilter: any[] = [
    { value: "1", viewValue: "Tùy chọn" },
    { value: "2", viewValue: "Theo bảng chữ cái từ A-Z" },
    { value: "3", viewValue: "Theo bảng chữ cái từ Z-A" },
    { value: "4", viewValue: "Giá từ thấp đến cao" },
    { value: "5", viewValue: "Giá từ cao đến thấp" },
    { value: "6", viewValue: "Mới nhất" },
    { value: "7", viewValue: "Cũ nhất" },
  ];

  numFilter: any = '1';
  numFilterSql: string = '';

  // binding keyword
  keyword: string = '';

  // pagination
  page: number;
  limit: number = 8;
  count: number;
  numberPages: number;
  limitPage = 5;
  limitPages: any;
  idCateRecipe: any;

  isStar1: boolean = false;
  isStar2: boolean = false;
  isStar3: boolean = false;
  isStar4: boolean = false;
  isStar5: boolean = false;

  // url Current
  urlCurrent: string = '';
  idUser: string = '';
  defaultImage: any = "assets/images/loading.gif";

  idShopMenuCategories: string = '';
  titleCategories: string = '';
  titleProduct: string = '';


  constructor(private api: ApiService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private seoService: SEOService) {
    this.idUser = this.api.getStaffValue?.id;
  }

  /**
   * ngOnInit
   */
  ngOnInit(): void {
    this.api.scrollToTop();
    this.activatedRoute.params.subscribe(params => {
      this.titleProduct = params['keyword'].split('-').join(' ');
      // reset page click
      this.page = 1;
      this.getListShopMenu();

    });


  }

  /**
  * ngOnDestroy
  */
  ngOnDestroy() {
    this.subscription.forEach(item => {
      item.unsubscribe();
    });
  }


  /**
  * getListShopMenu
  */
  getListShopMenu() {
    const param = {
      'offset': (this.page - 1) * this.limit,
      'limit': this.limit,
      'condition': this.titleProduct,
    };
    this.subscription.push(this.api.excuteAllByWhat(param, '713').subscribe(result => {
      if (result.length > 0) {
        result.forEach(item => {
          //slice title
          if (item.Title.length > 45) {
            item.titleShort = item.Title.slice(0, 45) + '...';
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

          // saleOf
          if ((item.PricePromotion != null) || (item.PricePromotion != '')) {
            item.saleOf = 100 - (Math.round(Number(item.PriceCurrent) * 100 / Number(item.PricePromotion)));
          }

          // check image invalid
          if (item.Thumbnail == '' || item.Thumbnail == null) {
            item.Thumbnail = 'assets/images/no-image.png';
          }
          item.Star = Number(item.Star);
          item.PricePromotion = Number(item.PricePromotion);
        })
        this.listShopMenu = result;

        //Seo title 
        let title = 'Search-product';
        this.seoService.setTitle(title);

      } else {
        this.listShopMenu = [];
      }
    }));
    this.countNumberOfPageSearchResult();
  }

  /**
   * getShopNameCategories
   */
  getShopNameCategories(id) {

    const param = {
      id: id
    };
    this.subscription.push(
      this.api.excuteAllByWhat(param, '1104').subscribe((result) => {

        if (result) {
          this.titleCategories = result.Name;
        }
      })
    );
  }

  /**
   * countNumberOfPageSearchResult
   */
  countNumberOfPageSearchResult() {
    this.count = 0;
    this.numberPages = 0;

    const param = {
      'condition': this.titleProduct,
    };

    this.subscription.push(this.api.excuteAllByWhat(param, '714').subscribe(result => {
      if (result.length > 0) {
        this.count = result[0].count;
        if (this.count % this.limit == 0) {
          this.numberPages = this.count / this.limit;
          this.limitPagesPagination(this.page);
        }
        else {
          this.numberPages = Math.floor(this.count / this.limit) + 1;
          this.limitPagesPagination(this.page);
        }
      }
    }));
  }

  /**
   * onChangeAfterPageClick
   * @param curpage 
   */
  onChangeAfterPageClick(curpage) {
    if (curpage < (this.limitPage * Math.floor(this.numberPages / this.limitPage))) {
      if (curpage == 1) {
        this.page = curpage + this.limitPage;
      }
      else
        if (curpage % this.limitPage != 0) {
          if (this.numberPages % this.limitPage == 0 && curpage > (this.numberPages - this.limitPage)) {
            this.page = this.page
          }
          else {
            this.page = Math.floor(curpage / this.limitPage) * this.limitPage + this.limitPage + 1;
          }
        }
        else
          if (curpage % this.limitPage == 0) {
            this.page = curpage + 1;
          }
    }
    else {
      this.page = this.numberPages;
    }

    this.getListShopMenu();
    this.api.scrollToTop();
  }

  /**
 * onChangeBeforePageClick
 * @param curpage 
 */
  onChangeBeforePageClick(curpage) {
    if (curpage > this.limitPage) {
      if (curpage % this.limitPage != 0) {
        this.page = Math.floor(curpage / this.limitPage) * this.limitPage - this.limitPage + 1;
      }
      else
        if (curpage % this.limitPage == 0) {
          this.page = curpage - (2 * this.limitPage) + 1;
        }
    }
    else {
      this.page = 1;
    }
    this.getListShopMenu();
    this.api.scrollToTop();
  }

  /**
   * limitPagesPagination
   * @param curpage 
   */
  limitPagesPagination(curpage) {
    this.limitPages = [];
    if (curpage <= (Math.floor(this.numberPages / this.limitPage) * this.limitPage)) {
      for (var i = 0; i < this.limitPage; i++) {
        if (curpage % this.limitPage == 0) {
          this.limitPages[i] = i + curpage - this.limitPage + 1;
        }
        else {
          this.limitPages[i] = i + Math.floor(curpage / this.limitPage) * this.limitPage + 1;
        }
      }
    }
    else {
      for (var i = 0; i < (this.numberPages - Math.floor(this.numberPages / this.limitPage) * this.limitPage); i++) {
        if (curpage % this.limitPage == 0) {
          this.limitPages[i] = i + curpage - this.limitPage + 1;
        }
        else {
          this.limitPages[i] = i + Math.floor(curpage / this.limitPage) * this.limitPage + 1;
        }
      }
    }
  }

  /**
   * change Page
   * @param page 
   */
  changePage(page) {
    this.page = page;
    this.getListShopMenu();
    this.api.scrollToTop();
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
   * onSearchClick
   */
  onClickShop() {
    const url = '/c1-list-menu/0/all';
    this.router.navigate([url.toLowerCase()]);
    this.getListShopMenu();
  }



}
