import { Component, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { Validators } from '@angular/forms';
import { FormBuilder } from '@angular/forms';
import { FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { Observer } from 'rxjs/internal/types';
import { ApiService } from 'src/app/common/api-service/api.service';
import { SendDataService } from 'src/app/common/api-service/send-data.service';
import { CurrencyPipe } from '@angular/common';
import { SEOService } from 'src/app/common/api-service/seo.service';
import { IPayPalConfig, ICreateOrderRequest } from 'ngx-paypal';

@Component({
  selector: 'app-h1-payment',
  templateUrl: './h1-payment.component.html',
  styleUrls: ['./h1-payment.component.scss'],
  providers: [CurrencyPipe]
})
export class H1PaymentComponent implements OnInit, AfterViewInit, OnDestroy {
  subscription: Subscription[] = [];
  observable: Observable<any>;
  observer: Observer<any>;

  public payPalConfig?: IPayPalConfig;

  //form
  form: FormGroup;

  //idOrderMenu:
  idOrderMenu: string;

  // data reference binding
  ShopMenuDatas: any[] = [];
  PackageDatas: any[] = [];
  CityDatas: any[] = [];
  DistrictDatas: any[] = [];
  PaymentTypeDatas: any[] = [];

  count = 0;

  dataOderMenu: any[] = [];

  inputOrderMenu: any = {
    id: '',
    IdCity: '',
    IdDistrict: '',
    IdPaymentType: '',
    TotalPrice: '',
    PromotionCode: '',
    Name: '',
    Email: '',
    Phone: '',
    Address: '',
    Note: '',
    UpdatedAt: '',
    Point: '',
    IdUser: '',
    PointUser: '',
  }

  //PromotionCode
  promotionCode: string;
  promotionPercent = 0;
  promotionMoney = 0;
  isPay: string = '1';
  pointUser: number = 0;
  dataUser: any[] = [];
  pointAddUser: number = 0;


  Promotion: any = {
    id: '',
    Name: '',
    PromotionCode: '',
    Percent: '',
    MoneyDiscount: '',
  }

  totalMoney = 0;
  isPromotion: number = 0;
  priceShopMenu: number = 0;

  lisDistrict: any;

  userValue: any = [];
  //cost ship
  costShip: number = 0;
  dolarity: number = 0;

  // url Current
  urlCurrent: string = '';

  showSuccess: boolean = false;
  isShowPaypal: boolean = false;
  isUserPoint: boolean = false;

  showPoint: number = 0;
  updatePointUser: number = 0;

  constructor(private api: ApiService,
    private currencyPipe: CurrencyPipe,
    private activatedRoute: ActivatedRoute, private router: Router,
    private sendDataService: SendDataService, private formBuilder: FormBuilder, private seoService: SEOService) {
    // xử lý bất đồng bộ
    this.observable = Observable.create((observer: any) => {
      this.observer = observer;
    });

    // add validate for controls
    this.form = this.formBuilder.group({
      Name: [
        null,
        [
          Validators.required,
        ],
      ],

      Email: [
        null,
        [
          Validators.required,
          Validators.email,
        ],
      ],
      City: [
        null,
        [
          Validators.required,
        ],
      ],

      District: [
        null,
        [
        ],
      ],

      Phone: [
        null,
        [
          Validators.required,
          Validators.pattern('[0-9]*'),
          Validators.minLength(10), Validators.maxLength(11)
        ]
      ],

      Address: [
        null,
        [
          Validators.required,
        ],
      ],
      Note: [
        null,
        [
        ],
      ],

    });
  }

  /**
   * ngOnInit
   */
  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.idOrderMenu = param['id'];
      this.userValue = this.api.getStaffValue;
      this.inputOrderMenu.id = param['id'];
      this.getDistrictWithCity(this.userValue.IdCity);
      this.getListShopmenu();
      this.getUserPoint();
      this.getListCity();
      this.getListDistrict();
      this.getListDataShop();
      this.observable.subscribe(data => {
        if (data == 5) {
          this.getLoadDataUserOder();
        }
      });
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
   * get list City
   */
  getListCity() {
    this.subscription.push(this.api.excuteAllByWhat({}, '2500')
      .subscribe((result) => {
        this.CityDatas = result;
        this.count += 1;
        this.observer.next(this.count);

      })

    );

  }

  /**
   * get district
   */
  getDistrictWithCity(id) {
    const param = {
      id: id,
    }
    this.subscription.push(this.api.excuteAllByWhat(param, '2607').subscribe(result => {
      this.DistrictDatas = result;
      // this.count += 1;

    }))
  }

  /**                                                            
   * get list District
   */
  getListDistrict() {
    this.subscription.push(this.api.excuteAllByWhat({}, '2600')
      .subscribe((result) => {
        this.DistrictDatas = result;
        this.count += 1;
        this.observer.next(this.count);
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
        let title = 'Payment';

        //Seo title
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
          this.dolarity = Number(result[0]?.Usd);
        }

        this.count += 1;
        this.observer.next(this.count);
      })
    );
  }

  /**                                                            
  * get list City
  */
  getUserPoint() {
    const param = {
      id: this.userValue.id
    }
    this.subscription.push(this.api.excuteAllByWhat(param, '2014')
      .subscribe((result) => {
        this.pointUser = Number(result?.Point);
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
   * getDisplayCityById   
   */
  getDisplayCityById(id) {
    return this.CityDatas.filter((e) => e.id == id)[0]?.Name;
  }

  /**                                                            
   * getDisplayDistrictById   
   */
  getDisplayDistrictById(id) {
    return this.DistrictDatas.filter((e) => e.id == id)[0]?.Name;
  }

  /**                                                            
  * getDisplayPaymentTypeById   
  */
  getDisplayPaymentTypeById(id) {
    return this.PaymentTypeDatas.filter((e) => e.id == id)[0]?.Name;
  }

  /**                                                            
   * getListPaymentType
   */
  getListPaymentType() {
    this.subscription.push(this.api.excuteAllByWhat({}, '3600')
      .subscribe((result) => {
        let data = result.data;
        this.PaymentTypeDatas = data;
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
      'IdUser': this.userValue.id
    }
    this.subscription.push(
      this.api.excuteAllByWhat(param, '2815').subscribe((result) => {
        if (result.length > 0) {
          result.forEach(item => {
            item.PriceShopMenu = Number(this.ShopMenuDatas.filter((e) => e.id == item.IdShop)[0]?.PriceCurrent);
            item.Amount = item.Amount;
            this.priceShopMenu += (Number(item.PriceShopMenu) * Number(item.Amount));
          })
          this.dataOderMenu = result; 
          
          // add point user
          for (let i = 0; i < result.length; i++) {
            this.pointAddUser += Number(this.ShopMenuDatas.filter((e) => e.id == result[i]?.IdShop)[0]?.Point) * result[i]?.Amount;
          }
          //set data input user
          this.inputOrderMenu.IdCity = this.userValue.IdCity;
          this.inputOrderMenu.IdDistrict = this.userValue.IdDistrict;
          this.inputOrderMenu.Name = this.userValue.Fullname;
          this.inputOrderMenu.Email = this.userValue.Email;
          this.inputOrderMenu.Phone = this.userValue.Phone;
          this.inputOrderMenu.Address = this.userValue.Address;
          this.inputOrderMenu.TotalPrice = this.priceShopMenu;
          this.getTotalMoney();
        } else {
          this.dataOderMenu = [];
        }
      })
    );
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
   * getPriceProduct 
   */
  getTotalMoney() {
    this.totalMoney = 0;
    if (this.isPromotion == 1) {
      this.totalMoney = Number(this.inputOrderMenu.TotalPrice) - (Number(this.inputOrderMenu.TotalPrice) * Number(this.promotionPercent / 100)) + this.costShip;

    } else {
      if (this.isPromotion == 2) {
        this.totalMoney = Number(this.inputOrderMenu.TotalPrice) - Number(this.promotionMoney) + this.costShip;
      } else {
        this.totalMoney = this.inputOrderMenu.TotalPrice + this.costShip;
      }

    }
    this.initConfig();
  }

  onClickUserPoint() {
    if (this.isUserPoint) {
      let totalPoint = this.totalMoney / 1000;
      if (this.pointUser >= totalPoint) {
        this.updatePointUser = this.pointUser - totalPoint;
        this.showPoint = totalPoint;
        this.totalMoney = 0;
      } else {
        this.totalMoney = (totalPoint - this.pointUser) * 1000;
        this.showPoint = this.pointUser;
        this.updatePointUser = 0;
        this.initConfig();
      }
    } else {
      this.getTotalMoney();
      this.showPoint = 0;
      this.updatePointUser = this.pointUser;
    }
  }

  /**
   * onBntCodeClick
   */
  onBntCodeClick() {
    const param = {
      'PromotionCode': this.promotionCode.trim()
    }
    this.subscription.push(
      this.api.excuteAllByWhat(param, '2107').subscribe((result) => {

        // let data = result.data;
        this.promotionPercent = 0;
        this.promotionMoney = 0;
        if (result != null) {

          this.Promotion = result;

          this.api.showSuccess('Nhập mà giảm giá thành công');
          if (this.Promotion.PercentCode != '0') {
            this.promotionPercent = Number(this.Promotion.PercentCode);
            this.isPromotion = 1;
          } else {
            this.promotionMoney = Number(this.Promotion.MoneyDiscount);
            this.isPromotion = 2;
          }
          this.getTotalMoney();
        } else {
          this.Promotion = [];
          this.api.showWarning('Mã giảm giá đã hết hạn hoặc sai mã');
          this.totalMoney = this.inputOrderMenu.TotalPrice + this.costShip;

        }
      })
    );
  }

  /**
   * onBntSummitClick
   */
  onBntSummitClick() {

    this.form.markAllAsTouched();

    if (!this.form.invalid) {

      this.inputOrderMenu.IdPaymentType = this.isPay;
      this.inputOrderMenu.IdDistrict = this.inputOrderMenu.IdDistrict;
      this.inputOrderMenu.TotalPrice = this.totalMoney;
      this.inputOrderMenu.PromotionCode = this.promotionCode;
      this.inputOrderMenu.UpdatedAt = this.api.formatDateDOTNET(new Date());
      this.inputOrderMenu.Note == '' ? ' ' : this.inputOrderMenu.Note;
      this.inputOrderMenu.Point = this.showPoint;

      this.inputOrderMenu.IdUser = this.userValue.id;
      if (this.isUserPoint) {
        this.inputOrderMenu.PointUser = this.updatePointUser + this.pointAddUser;
      } else {
        this.inputOrderMenu.PointUser = this.pointAddUser + this.pointUser;
      }
      this.subscription.push(
        this.api.excuteAllByWhat(this.inputOrderMenu, '2817').subscribe((result) => {
          if (result) {
            // update phone & address 
            this.api.showSuccess('Đặt đơn hàng thành công');
            this.router.navigate(['/k1-thank-you/cam-on']);
            // this.sentMailForUser();
            this.sendDataService.setDataSend('true');
          } else {
            this.api.showWarning('Xử lý thất bại. Vui lòng thử lại');
          }
        })
      );
    }
  }


  /**
   * getDataRenderEmail
   */
  getDataRenderEmail() {
    const data = this.dataOderMenu;
    let contentTable = '';
    data.forEach(item => {
      contentTable += `<tbody
      bgcolor="#eee" style="font-family:Arial,Helvetica,sans-serif;font-size:12px;color:#444;line-height:18px">
        <tr>
            <td align="left" valign="top" style="padding:3px 9px">
                <strong>${item.IdShopMenu != 0 ?
          this.getDisplayShopmenuById(item?.IdShopMenu) : item.IdPackage != 0 ? this.getDisplayPackageById(item?.IdPackage) : ''
        }</strong>
            </td>

            <td align="center" valign="top" style="padding:3px 9px">
                <span>${item.IdShopMenu != 0 ? this.currencyPipe.transform(this.getPriceProduct(item.PriceShopMenu, item.Amount), "VND") : item.IdPackage != 0 ? this.currencyPipe.transform(this.getPriceProduct(item.PricePackage, item.Amount), "VND") : ''
        }</span></td>
            <td align="left" valign="top" style="padding:3px 9px">${item.Amount
        }
            </td>
            <td align="right" valign="top" style="padding:3px 9px" colspan="2">
                <span>${item.IdShopMenu != 0 ? this.currencyPipe.transform(this.getPriceProduct(item.PriceShopMenu, item.Amount), "VND") : item.IdPackage != 0 ?
          this.currencyPipe.transform(this.getPriceProduct(item.PricePackage, item.Amount), "VND") : ''
        }</span>
            </td>
        </tr>
    </tbody>`
    });

    return contentTable;
  }



  /**
   * sent Mail For User
   */
  sentMailForUser() {
    const from = 'noreply@rolievn.com';
    const to = this.inputOrderMenu.Email;

    let subject = '[ROLIEVN] DAT HANG THANH CONG';
    let typemail = 'CC';
    let message = `<table align="center" border="0" cellpadding="0" cellspacing="0" width="100%" bgcolor="#dcf0f8" style="margin:0;padding:0;background-color:#f2f2f2;width:100%!important;font-family:Arial,Helvetica,sans-serif;font-size:12px;color:#444;line-height:18px">
    <tbody>
        <tr>
            <td align="center" valign="top" style="font-family:Arial,Helvetica,sans-serif;font-size:12px;color:#444;line-height:18px;font-weight:normal">
                <table border="0" cellpadding="0" cellspacing="0" width="600" style="margin-top:15px">
                    <tbody>
                        <tr>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0" style="line-height:0">
                                    <tbody>

                                    </tbody>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" valign="bottom" id="m_1010356081423004935headerImage">
                                <table width="100%" cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr>
                                            <td valign="top" bgcolor="#FFFFFF" width="100%" style="padding:0">
                                                <div style="color:#fff;font-size:11px">Tổng giá trị đơn hàng là
                                                    <span>${this.currencyPipe.transform(this.inputOrderMenu.TotalPrice, "VND")} - Mã đơn: #DH${this.idOrderMenu} </span></div>
                                                <a style="border:medium none;text-decoration:none;color:#007ed3" 
                                                    target="_blank">
                                                    <img alt="Rolie" src="https://hoctienganhphanxa.com/api/p7rolie/uploads/images/2020-11-28T04:53:23.262Z-rolielogo.jpg"
                                                        style="border:none;outline:none;text-decoration:none;display:inline;height:auto" class="CToWUd">
                                                </a>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>

                        <tr style="background:#fff">
                            <td align="left" width="600" height="auto" style="padding:15px">
                                <table>
                                    <tbody>
                                        <tr>

                                            <td>
                                                <h1 style="font-size:14px;font-weight:bold;color:#444;padding:0 0 5px 0;margin:0">
                                                    Chúc mừng quý khách  ${this.inputOrderMenu.Name} đã đặt hàng thành công !

                                                </h1>


                                                <p style="margin:4px 0;font-family:Arial,Helvetica,sans-serif;font-size:12px;color:#444;line-height:18px;font-weight:normal">
                                                    Chúng tôi vừa kiểm tra đơn hàng của quý khách. Chúng tôi sẽ liên hệ với quý khách trong thời gian sớm nhất, vui lòng chú ý điện thoại để được nhân viên tư vấn chăm sóc sớm nhất
                                                </p>


                                                <h3 style="font-size:13px;font-weight:bold;color:#FF443B;text-transform:uppercase;margin:20px 0 0 0;border-bottom:1px solid #ddd">
                                                    Thông tin đơn hàng  #DH${this.idOrderMenu} <span style="font-size:12px;color:#777;text-transform:none;font-weight:normal">(` + this.inputOrderMenu.CreatedAt + `)</span> </h3>
                                            </td>
                                        </tr>
                                        <tr>
                                        </tr>
                                        <tr>
                                            <td style="font-family:Arial,Helvetica,sans-serif;font-size:12px;color:#444;line-height:18px">

                                                <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                                    <thead>
                                                        <tr>
                                                            <th align="left" width="50%" style="padding:6px 9px 0px 9px;font-family:Arial,Helvetica,sans-serif;font-size:12px;color:#444;font-weight:bold">
                                                                Thông tin thanh toán</th>
                                                            <th align="left" width="50%" style="padding:6px 9px 0px 9px;font-family:Arial,Helvetica,sans-serif;font-size:12px;color:#444;font-weight:bold">
                                                                Địa chỉ giao hàng</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <tr>
                                                            <td valign="top" style="padding:3px 9px 9px 9px;border-top:0;font-family:Arial,Helvetica,sans-serif;font-size:12px;color:#444;line-height:18px;font-weight:normal">

                                                                <span style="text-transform:capitalize"> ${this.inputOrderMenu.Name}</span><br>

                                                                <a href="mailto:${this.inputOrderMenu.Email}" target="_blank">${this.inputOrderMenu.Email}</a><br> 

                                                            </td>

                                                            <td valign="top" style="padding:3px 9px 9px 9px;border-top:0;border-left:0;font-family:Arial,Helvetica,sans-serif;font-size:12px;color:#444;line-height:18px;font-weight:normal">
                                                                ${this.inputOrderMenu.Name}<br>${this.inputOrderMenu.Address}<br>${this.getDisplayDistrictById(this.inputOrderMenu.IdDistrict)}, ${this.getDisplayCityById(this.inputOrderMenu.IdCity)}, Việt Nam<br> T: ${this.inputOrderMenu.Phone}
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td valign="top" style="padding:7px 9px 0px 9px;border-top:0;font-family:Arial,Helvetica,sans-serif;font-size:12px;color:#444" colspan="2">
                                                                <p style="font-family:Arial,Helvetica,sans-serif;font-size:12px;color:#444;font-weight:normal">
                                                                    <br>
                                                                    <strong>Phí vận chuyển: </strong>0₫

                                                                    <br>
                                                                    ${this.inputOrderMenu.Address}, ${this.getDisplayDistrictById(this.inputOrderMenu?.IdDistrict)}, ${this.getDisplayCityById(this.inputOrderMenu?.IdCity)}
                                                                </p>
                                                            </td>
                                                            
                                                        </tr>
                                                        <tr>
                                                        <td valign="top" style="padding:5px 9px 0px 9px;border-top:0;font-family:Arial,Helvetica,sans-serif;font-size:12px;color:#444" colspan="2">
                                                                <p style="font-family:Arial,Helvetica,sans-serif;font-size:12px;color:#444;font-weight:normal">
                                                                    <br>
                                                                    <strong>Thông tin chuyển khoản: </strong>                                                                    
                                                                    <h3> CHỦ TÀI KHOẢN: CTY TNHH ROLIE VIET NAM </h3>                                                                    
                                                                    <h4 > BIDV:12010007159253 - Ngân hàng TMCP ĐT &amp; PT Việt Nam- Chi nhánh sở giao dịch 1</h4>                                                                    
                                                                    <p><b>NỘI DUNG CHUYỂN KHOẢN:</b> #DH${this.idOrderMenu}-${this.inputOrderMenu.Phone}-${this.inputOrderMenu.TotalPrice} </p>
                                                                </p>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <h2 style="text-align:left;margin:10px 0;border-bottom:1px solid #ddd;padding-bottom:5px;font-size:13px;color:#FF443B">
                                                    CHI TIẾT ĐƠN HÀNG</h2>
                                                <table cellspacing="0" cellpadding="0" border="0" width="100%" style="background:#f5f5f5">
                                                    <thead>
                                                        <tr>
                                                            <th align="left" bgcolor="#FF443B" style="padding:6px 9px;color:#fff;text-transform:uppercase;font-family:Arial,Helvetica,sans-serif;font-size:12px;line-height:14px">
                                                                Sản phẩm</th>
                                                            <th align="left" bgcolor="#FF443B" style="padding:6px 9px;color:#fff;text-transform:uppercase;font-family:Arial,Helvetica,sans-serif;font-size:12px;line-height:14px">
                                                                Đơn giá</th>
                                                            <th align="left" bgcolor="#FF443B" style="padding:6px 9px;color:#fff;text-transform:uppercase;font-family:Arial,Helvetica,sans-serif;font-size:12px;line-height:14px">
                                                                Số lượng</th>
                                                            <th align="right" colspan="2" bgcolor="#FF443B" style="padding:6px 9px;color:#fff;text-transform:uppercase;font-family:Arial,Helvetica,sans-serif;font-size:12px;line-height:14px">
                                                                Tổng tạm</th>
                                                        </tr>
                                                    </thead>
                                                    ${this.getDataRenderEmail()}
                                                    <tfoot style="font-family:Arial,Helvetica,sans-serif;font-size:12px;color:#444;line-height:18px">
                                                        <tr>
                                                            <td colspan="4" align="right" style="padding:5px 9px">
                                                                Chi phí vận chuyển</td>
                                                            <td align="right" style="padding:5px 9px">
                                                                <span>0₫</span></td>
                                                        </tr>
                                                        <tr>
                                                         
                                                            <td colspan="4" align="right" style="padding:5px 9px">
                                                                Giảm giá</td>
                                                            <td align="right" style="padding:5px 9px">
                                                                <span>${this.promotionPercent}%<br>${this.promotionPercent > 0 ? 0 : this.promotionMoney}₫</span></td>
                                                       </tr>
                                                        <tr bgcolor="#eee">
                                                            <td colspan="4" align="right" style="padding:7px 9px">
                                                                <strong><big>Tổng giá trị đơn hàng</big></strong>
                                                            </td>
                                                            <td align="right" style="padding:7px 9px">
                                                                <strong><big><span>${this.currencyPipe.transform(this.inputOrderMenu.TotalPrice, "VND")}</span></big></strong>
                                                            </td>
                                                        </tr>
                                                    </tfoot>
                                                </table>
                                                <br>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <br>
                                                <p style="margin:0;font-family:Arial,Helvetica,sans-serif;font-size:12px;color:#444;line-height:18px;font-weight:normal">
                                                Sản phẩm sẽ được gửi đến quý khách qua Email trong vòng 24h sau khi Quý khách đã chuyển khoản thanh toán thành công. Xin chân thành cảm ơn! 

                                                </p>                                               

                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <br>
                                                <p style="font-family:Arial,Helvetica,sans-serif;font-size:12px;margin:0;padding:0;line-height:18px;color:#ff443b;font-weight:bold">
                                                    <span class="il">Trân trọng,</span><br>
                                                    <span class="il">Công ty TNHH ROLIE Việt Nam</span><br>
                                                    <span class="il"><a style="color:#ff443b; text-decoration: none;" href="tel:02473058580">Phone: (024) 73 058 580</a></span><br>
                                                    <span class="il"><a style="color:#ff443b; text-decoration: none;" href="mailto:cskh@rolievn.com">Email: cskh@rolievn.com</a></span><br>
                                                    <span class="il"><a style="color:#ff443b; text-decoration: none;" href="https://rolievn.com/"
                                                    target="_blank">Website: rolievn.com</a></span><br>

                                                </p>                                                
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
        <tr>
        </tr>
    </tbody>
</table>`;
    this.api.sentMail(from, to, subject, message, typemail).subscribe(data => { });
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

  /**
   * initConfig Paypal
   */
  private initConfig(): void {
    let USD = Math.ceil(this.totalMoney / this.dolarity) + '';
    this.payPalConfig = {
      currency: 'USD',
      clientId: 'sb',
      createOrderOnClient: (data) => <ICreateOrderRequest>{
        intent: 'CAPTURE',
        purchase_units: [
          {
            amount: {
              currency_code: 'USD',
              value: USD,
              breakdown: {
                item_total: {
                  currency_code: 'USD',
                  value: USD
                }
              }
            },
            // items: [
            //   {
            //     name: 'Enterprise Subscription',
            //     quantity: '1',
            //     category: 'DIGITAL_GOODS',
            //     unit_amount: {
            //       currency_code: 'USD',
            //       value: USD,
            //     },
            //   }
            // ]
          }
        ]
      },
      advanced: {
        commit: 'true'
      },
      style: {
        label: 'paypal',
        layout: 'vertical'
      },
      onApprove: (data, actions) => {
        console.log('onApprove - transaction was approved, but not authorized', data, actions);
        actions.order.get().then(details => {
          console.log('onApprove - you can get full order details inside onApprove: ', details);
        });
      },
      onClientAuthorization: (data) => {
        console.log('onClientAuthorization - you should probably inform your server about completed transaction at this point', data);
        if (data['status'] == 'COMPLETED') {
          this.onBntSummitClick();
        } else {
          this.api.showWarning('Xử lý thất bại. Vui lòng thử lại hoặc chuyển qua loại thanh toán khác')
        }
      },
      onCancel: (data, actions) => {
        console.log('OnCancel', data, actions);
      },
      onError: err => {
        console.log('OnError', err);
      },
      onClick: (data, actions) => {
        console.log('onClick', data, actions);
      },
    };
  }

}
