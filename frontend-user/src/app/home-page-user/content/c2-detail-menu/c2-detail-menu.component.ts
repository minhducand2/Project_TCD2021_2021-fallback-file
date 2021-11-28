import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { ApiService } from 'src/app/common/api-service/api.service';
import { DomSanitizer } from '@angular/platform-browser';
import { SendDataService } from 'src/app/common/api-service/send-data.service';
import { SEOService } from 'src/app/common/api-service/seo.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-c2-detail-menu',
  templateUrl: './c2-detail-menu.component.html',
  styleUrls: ['./c2-detail-menu.component.scss']
})
export class C2DetailMenuComponent implements OnInit, OnDestroy {

  subscription: Subscription[] = [];

  isDisabled: boolean = false;

  idShopMenu: number;
  idNewShopMenu: string;


  //data binding
  shopMenuDetail: any = {
    Description: '',
    Duration: '',
    Images: '',
    PriceCurrent: '',
    PriceOrigin: '',
    Star: '',
    Thumbnail: '',
    Title: '',
    Video: '',
    id: '',
  }
  listShopMenuRelative: any;
  listShopMenuSeen: any;
  comboMenu: any;
  amount: number = 1;
  defaultImage: any = "assets/images/loading.gif";
  user: any;
  idUser: string;
  idLastShopMenu: string;

  //urlSEO
  urlSEO: string = ''; 
  titleShare: string = '';
  listDataComment: any[] = [];

  socialShare: any = {
    id: '',
    idShopMenu: '',
    facebook: '0',
    twitter: '0',
    pinterest: '0',
    email: '0',
  }

  isBtnComment: boolean = false;
  isShopMenuSeen: boolean = false;
  userComment: any = {
    IdShop: '',
    IdUser: '',
    Content: '',
    IdTypeComment: '',
  }
  idReply: string = '';
  offset: number = 0;
  limit: number = 5;
  nameControl = new FormControl(null);
  nameControl1 = new FormControl(null);
  contentControl = new FormControl(null, [Validators.required]);
  contentControl1 = new FormControl(null, [Validators.required]);
  Categories: any;
  nameComment: string = '';

  /**
   * constructor
   * @param activatedRoute 
   * @param router 
   * @param sanitizer 
   * @param api 
   * @param sendDataService 
   */
  constructor(private activatedRoute: ActivatedRoute,
    private router: Router,
    private sanitizer: DomSanitizer,
    private api: ApiService,
    private seoService: SEOService,
    private sendDataService: SendDataService,
    private spinner: NgxSpinnerService,
  ) {
    this.user = this.api.getStaffValue;
    this.idUser = this.api.getStaffValue?.id;
    if (this.user != null) {
      this.nameComment = this.user.Fullname;
      this.userComment.IdUser = this.user.id;
    }

  }

  /**
   * ngOnInit
   */
  ngOnInit(): void {
    this.api.scrollToTop();
    this.urlSEO = this.api.linkUrlSeo + this.router.url;  
    this.activatedRoute.params.subscribe(param => {
      this.idShopMenu = param['id'];
      this.getDetailShopMenu();
      this.getShopMenuRelative();
      this.getLoadDataComment();
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
   * getLoadDataSocial
   */
  getLoadDataSocial() {
    const param = {
      IdShopMenu: this.idShopMenu
    };
    this.subscription.push(this.api.excuteAllByWhat(param, '607').subscribe(result => {
      let data = result.data;
      if (result.status == true && data.length > 0) {
        this.socialShare = data[0];
      }
    }));
  }
  /**
   * onClickSocialShare
   * @param keyword 
   */
  onClickSocialShare(keyword) {
    const param = {
      IdShopMenu: this.idShopMenu
    }

    this.subscription.push(this.api.excuteAllByWhat(param, '607').subscribe(result => {
      let data = result.data;
      if (result.status == true && data.length > 0) {
        const updateSocial = {
          id: data[0].id,
          keyword: keyword
        }
        this.subscription.push(this.api.excuteAllByWhat(updateSocial, '608').subscribe(result => {
          let data = result.data;

          this.getLoadDataSocial();
        }));
      } else {
        const insertSocial = {
          IdShopMenu: this.idShopMenu,
          keyword: keyword
        }
        this.subscription.push(this.api.excuteAllByWhat(insertSocial, '609').subscribe(result => {
          let data = result.data;
          this.getLoadDataSocial();
        }));
      }
    }));
  }

  /**
   * getDetailShopMenu
   */
  getDetailShopMenu() {
    const param = {
      'id': this.idShopMenu
    };
    this.subscription.push(this.api.excuteAllByWhat(param, '704').subscribe(result => {
      if (result) {

        this.shopMenuDetail = result;
        this.shopMenuDetail.Video = this.sanitizer.bypassSecurityTrustHtml(this.shopMenuDetail.Video);
        this.getDataMenuCategories();
        //Seo title
        this.seoService.setTitle(this.shopMenuDetail.Title);

        //<!-- For Google -->
        this.seoService.setSocialMediaTagsGoogle(
          this.shopMenuDetail.Description,
          this.shopMenuDetail.Title,
        );

        //<!-- For Facebook -->
        this.seoService.setSocialMediaTagsFacebook(
          this.shopMenuDetail.Title,
          this.shopMenuDetail.Thumbnail,
          this.urlSEO,
          this.shopMenuDetail.Description,
        );

        //<!-- For Twitter -->
        this.seoService.setSocialMediaTagsTwitter(
          this.shopMenuDetail.Title,
          this.shopMenuDetail.Thumbnail,
          this.shopMenuDetail.Description,
        );
        this.titleShare = this.api.stripHtml(this.shopMenuDetail.Description).slice(0, 200) + '...';

      }

    }));
  }

  getDataMenuCategories() {
    const param = {
      id: this.shopMenuDetail.IdShopCategories
    };
    this.subscription.push(this.api.excuteAllByWhat(param, '1104').subscribe(result => {
      if (result) {
        this.Categories = result;

      } else {
        this.Categories = [];
      }
    }));
  }

  /**
   * getShopMenuRelative
   */
  getShopMenuRelative() {
    const param = {};
    this.subscription.push(this.api.excuteAllByWhat(param, '708').subscribe(result => {

      if (result) {
        result.forEach(item => {
          //slice Title
          if (item.Title.length > 30) {
            item.titleShort = item.Title.slice(0, 30) + '...';
          } else {
            item.titleShort = item.Title;
          }
        })
        this.listShopMenuRelative = result;
      }
    }));
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

    this.idShopMenu = id;

    this.getDetailShopMenu();
    this.getShopMenuRelative();
    this.getLoadDataComment();
    this.router.navigate([url.toLowerCase()]);
    this.isBtnComment = false;

    //isDisabled button
    this.isDisabled = false;
    this.api.scrollToTop();
  }

  /**
   * onStepDown
   */
  onStepDown() {
    if (this.amount > 1) {
      this.amount--;
    }
  }

  //** */
  onStepUp() {
    this.amount++;
  }

  /**
   * onBtnClickAddCart
   */
  onBtnClickAddCart() {
    let formatTitle = this.api.cleanAccents(this.shopMenuDetail.Title).split(' ').join('-');
    let re = /\//gi;
    formatTitle = formatTitle.replace(re, '');
    const url = '/c2-detail-menu/' + this.idShopMenu + '/' + formatTitle;

    if (this.user == '' || this.user == undefined) {
      //set localStorage url
      localStorage.removeItem('lastUrl');
      localStorage.setItem('lastUrl', url);

      this.api.showWarning('Bạn hãy đăng nhập rồi tiếp tục mua hàng');
      this.router.navigate(['/login/dang-nhap']);
    } else {
      //isDisabled button
      this.isDisabled = true;

      // check idShopMenu
      const param = {
        'IdUser': this.idUser,
        'IdShop': this.idShopMenu,
        'Amount': this.amount
      }
      this.subscription.push(
        // Find data with id pOrderMenu
        this.api.excuteAllByWhat(param, '2814').subscribe((result) => {
          if (result) {
            this.api.showSuccess('Thêm vào giỏ hàng thành công');
            this.sendDataService.setDataSend('true');
          } else {
            this.api.showWarning('Xử lý thất bại. Vui lòng thử lại')
          }

        }));
    }

  }

  /**
   * OnBntClickBuy
   */
  OnBntClickBuy() {
    let formatTitle = this.api.cleanAccents(this.shopMenuDetail.Title).split(' ').join('-');
    let re = /\//gi;
    formatTitle = formatTitle.replace(re, '');
    const urlLogin = '/c2-detail-menu/' + this.idShopMenu + '/' + formatTitle;
    const url = '/g1-cart/cart';

    //check login
    if (this.user == '' || this.user == undefined) {
      //set localStorage url
      localStorage.removeItem('lastUrl');
      localStorage.setItem('lastUrl', urlLogin);
      this.api.showWarning('Bạn hãy đăng nhập rồi tiếp tục mua hàng');
      this.router.navigate(['/login/dang-nhap']);
    } else {
      //isDisabled button
      this.isDisabled = true;

      // check idShopMenu
      const param = {
        'IdUser': this.idUser,
        'IdShop': this.idShopMenu,
        'Amount': this.amount
      }
      this.subscription.push(
        // Find data with id pOrderMenu
        this.api.excuteAllByWhat(param, '2814').subscribe((result) => {
          if (result) {
            this.api.showSuccess('Thêm vào giỏ hàng thành công');
            this.sendDataService.setDataSend('true');
            this.router.navigate([url]);
          } else {
            this.api.showWarning('Xử lý thất bại. Vui lòng thử lại')
          }
        }));
    }
  }


  /**
   * getLoadDataComment
   */
  getLoadDataComment() {
    this.listDataComment = [];

    const param = {
      'condition': this.idShopMenu,
      'offset': this.offset,
      'limit': this.limit
    };
    this.subscription.push(this.api.excuteAllByWhat(param, '1009').subscribe(result => {
      if (result.length > 0) {
        this.listDataComment = result;
        // load child main data
        this.subscription.push(this.api.excuteAllByWhat(param, '1010').subscribe(result1 => {
          if (result1.length > 0) {
            // add child menu to mainMenus
            this.listDataComment.forEach(item => {
              item.childs = [];
              result1.forEach(ele => {

                //push data child
                if (item.id == ele.IdTypeComment) {
                  item.childs.push(ele);
                }
              });
            });
          }
        }));

      } else {
        this.listDataComment = [];
      }
    }));
  }

  /**
   * getLoadChangePage
   */
  getLoadChangePage() {
    this.limit += 5;
    this.getLoadDataComment();

  }

  /**
   * onBtnSummitComment
   */
  onBtnSummitComment() {
    let formatTitle = this.api.cleanAccents(this.shopMenuDetail.Title).split(' ').join('-');
    let re = /\//gi;
    formatTitle = formatTitle.replace(re, '');
    const url = '/c2-detail-menu/' + this.idShopMenu + '/' + formatTitle;

    //check login
    if (this.user == '' || this.user == undefined) {
      //set localStorage url
      localStorage.removeItem('lastUrl');
      localStorage.setItem('lastUrl', url);

      this.api.showWarning('Bạn hãy đăng nhập để bình luận');
      this.router.navigate(['/login/dang-nhap']);
    } else {
      //insert comment
      if (this.contentControl.status != "VALID") {
        this.api.showWarning("Vui lòng nhập bình luận");
        return;
      } else {
        this.isBtnComment = true;
        const param = {
          IdShop: this.idShopMenu,
          IdUser: this.userComment.IdUser,
          Content: this.userComment.Content,
          IdTypeComment: '0',
        }
        this.subscription.push(
          this.api.excuteAllByWhat(param, '1011').subscribe((result) => {
            if (result) {
              this.getLoadDataComment();
              this.contentControl.reset();
            } else {
              this.api.showWarning('Xử lý thất bại. Vui lòng thử lại');
            }
          }));
      }
    }

  }


  /**
   * openModelReply
   * @param item 
   */
  openModelReply(item) {
    this.idReply = item.id;
  }

  /**
   * onBtnReplyComment
   */
  onBtnReplyComment() {
    if (this.contentControl1.status != "VALID") {
      this.api.showWarning("Vui lòng nhập bình luận");
      return;
    } else {
      const param = {
        IdShop: this.idShopMenu,
        IdUser: this.userComment.IdUser,
        Content: this.userComment.Content,
        IdTypeComment: this.idReply,
      }
      this.subscription.push(
        this.api.excuteAllByWhat(param, '1011').subscribe((result) => {
          if (result) {
            this.getLoadDataComment();
            this.contentControl1.reset();
          } else {
            this.api.showWarning('Xử lý thất bại. Vui lòng thử lại');
          }
        }));
    }
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
  * getCheckStaff
  */
  getCheckStaff() {
    const param = {
      email: this.user.Email
    }
    this.subscription.push(this.api.excuteAllByWhat(param, '13').subscribe(result => {
      let data = result.data;
      if (result.status == true && data.length > 0) {
        this.userComment.IsStaff = data[0].id;
      } else {
        this.userComment.IsStaff = '0';
      }
    }));
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
