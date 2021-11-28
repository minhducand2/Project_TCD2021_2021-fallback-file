import { Component, OnInit, OnDestroy } from "@angular/core";
import { Router } from "@angular/router";
import { Subscription } from "rxjs";

import { SEOService } from 'src/app/common/api-service/seo.service';
import firebase from "firebase/app";
import { AngularFireAuth } from "@angular/fire/auth";

import { FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { ViewChild } from "@angular/core";
import { ElementRef } from "@angular/core";
import { Staff } from "src/app/common/models/140staff.models";
import { ApiService } from "src/app/common/api-service/api.service";
import { SendDataService } from "src/app/common/api-service/send-data.service";
import { MustMatch } from "src/app/common/validations/must-match.validator"; 


declare var FB: any;

@Component({
  selector: "app-login",
  templateUrl: "./login.component.html",
  styleUrls: ["./login.component.scss"],
})
export class LoginComponent implements OnInit, OnDestroy {
  subscription: Subscription[] = [];

  input: Staff;

  inputOrderMenu: any = {
    id: '',
    Fullname: '',
    Email: '',
    Password: '',
    Sex: '1',
    authkey: '',
    Address: '',
    Phone: '',
    IdDistrict: '',
    IdCity: '',
  }

  // menu
  isShow: boolean = false;
  isRegister: boolean = false;
  info: any = null;

  // data binding
  headerInfo = {
    Phone: "",
    Mail: "",
    mailHref: "",
    phoneHref: "",
  };

  dataFooter = {
    AndroidLink: "",
    IosLink: "",
  };

  inputLogin: any = {
    Email: '',
    Password: '',
  }

  dataMenu: any;
  idUser: string;
  infoUser: any = null;
  userValue: any = [];
  amount: number = 0;
  CityDatas: any[] = [];
  DistrictDatas: any[] = [];
  shopMenuCatgores: any[] = [];
  shopMenuMobile: any[] = [];
  idCustomer: any;
  // binding logo
  logo: string = '';
  avatar: string = '';

  getUrl: any;

  // url Current
  urlCurrent: string = '';

  listCateRecipe: any;
  isMobile: boolean = false;
  isShowMenuMobile: boolean = false;
  isCheckEmail: boolean = false;
  isDisabledEmail: boolean = false;
  isClassShow: boolean = false;
  isClass1Show: boolean = false;
  //form
  form: FormGroup;
  form1: FormGroup;
  form2: FormGroup;

  @ViewChild('updateInformation') updateInformation: ElementRef;
  @ViewChild('login') login: ElementRef;


  /**
   * constructor
   * @param api
   */
  constructor(
    public api: ApiService,
    public auth: AngularFireAuth, // Inject Firebase auth service
    private router: Router,
    private sendDataService: SendDataService,
    private seoService: SEOService,
    private formBuilder: FormBuilder,
    private formBuilder1: FormBuilder,
  ) {
    this.getUrl = localStorage.getItem('lastUrl');
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
      Sex: [
        null,
      ],

      Address: [
        null,
        [
          Validators.required,
        ],
      ],

    });

    this.form1 = this.formBuilder.group({
      password: [
        null,
        [
          Validators.required,
          Validators.minLength(6),
        ],
      ],
      repassword: [
        null,
        [
          Validators.required,
        ],
      ],

    },
      { validator: MustMatch("password", "repassword") }
    );

    this.form2 = this.formBuilder.group({
      Email: [
        null,
        [
          Validators.required,
          Validators.email,
        ],
      ],
      password: [
        null,
        [
          Validators.required,
          Validators.minLength(6),
        ],
      ],
    },

    );




  }

  /**
   * ngOnInit
   */
  ngOnInit() {
    // this.getHeaderInfo();
    // this.getDataMenuHead();
    // this.getFooter();
    // this.getCateRecipe();
    // this.isMobileDevice();
    this.getListCity();
    // this.getShopMenuCategories();
    // this.getShopMenuMobile();
    // url Current
    this.urlCurrent = this.api.getUrl();

    let title = '';
    if (this.urlCurrent.indexOf('/en/') == -1) {
      title = 'Đăng nhập';
    } else {
      title = 'Login'
    }

    //Seo title
    this.seoService.setTitle(title);

    this.idUser = this.api.getStaffValue?.id;
    this.userValue = this.api.getStaffValue;

    if (this.userValue != null) {
      this.infoUser = this.userValue;

      // set avatar default user
      if (this.infoUser['Avatar'] == '' || this.infoUser['Avatar'] == null) {
        this.infoUser['Avatar'] = "https://rolievn.com/api/uploads/images/logo-admin.png";
      } else {
        this.infoUser['Avatar'] = this.infoUser['Avatar'];
      }
    }

    // login with facebook
    // (window as any).fbAsyncInit = function () {
    //   FB.init({
    //     appId: "371570617392399",
    //     cookie: true,
    //     xfbml: true,
    //     version: "v9.0",
    //   });
    //   FB.AppEvents.logPageView();
    // };

    (function (d, s, id) {
      var js,
        fjs = d.getElementsByTagName(s)[0];
      if (d.getElementById(id)) {
        return;
      }
      js = d.createElement(s);
      js.id = id;
      js.src = "https://connect.facebook.net/en_US/sdk.js";
      fjs.parentNode.insertBefore(js, fjs);
    })(document, "script", "facebook-jssdk");
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
 * isMobileDevice
 */
  isMobileDevice() {
    var userAgent = navigator.userAgent;
    if (/Android|webOS|iPhone|iPod|BlackBerry|IEMobile|Opera Mini/i.test(userAgent)) {
      // true for mobile device
      this.isMobile = true;
    } else {
      // false for not mobile device
      this.isMobile = false;
    }
  };

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
   * login With Google
   */
  loginWithGoogle() {
    this.auth
      .signInWithPopup(new firebase.auth.GoogleAuthProvider())
      .then((response) => {

        // clear cookie
        this.api.remoteAllLoginAccount();

        // check user exists
        this.api.excuteAllByWhat({ condition: response.user.email }, "2008").subscribe((result) => {
          // user exists
          if (result.length > 0) {
            localStorage.setItem("staffSubject", JSON.stringify(result[0]));
            this.api.staffSubject.next(result[0]);
            // redirect to last page
            if (this.getUrl == '' || this.getUrl == undefined) {
              this.router.navigate(["/"]);
            } else {
              this.router.navigate([this.getUrl]);
            }
            setTimeout(() => {
              window.location.reload();
            }, 500);
          } else {
            this.isRegister = false;
            this.inputOrderMenu.Fullname = response.user.displayName;
            this.inputOrderMenu.Email = response.user.email;
            this.inputOrderMenu.authkey = response.user.email;
            this.updateInformation?.nativeElement.click();
            this.isDisabledEmail = true;

          }
        });

      })
      .catch((error) => {
      });
  }

  /**
   * login With Facebook
   */
  loginWithFacebook() {
    FB.login((response) => {

      if (response.status == 'connected') {
        this.api.showSuccess(
          "Login facebook success! UserID: " + response.authResponse.userID
        );

        // clear cookie
        this.api.remoteAllLoginAccount();

        // check user exists
        this.api.excuteAllByWhat({ condition: response.authResponse.userID }, "2008").subscribe((result) => {
          // user exists
          if (result.length > 0) {
            localStorage.setItem("staffSubject", JSON.stringify(result[0]));
            this.api.staffSubject.next(result[0]);
            // redirect to last page
            if (this.getUrl == '' || this.getUrl == undefined) {
              this.router.navigate(["/"]);
            } else {
              this.router.navigate([this.getUrl]);
            }
            setTimeout(() => {
              window.location.reload();
            }, 500);
          } else {
            this.isRegister = false;
            this.inputOrderMenu.authkey = response.authResponse.userID;
            this.updateInformation.nativeElement.click();
          }
        });

      } else {
        this.api.showError('Vui lòng tải lại trang');

      }
    });
  }

  /**
   * getHeaderInfo
   */
  getHeaderInfo() {
    const param = {};
    this.subscription.push(
      this.api.excuteAllByWhat(param, "400").subscribe((result) => {
        let data = result.data;
        if (result.status == true && data.length > 0) {
          this.headerInfo = data[0];
          data[0].phoneHref = "tel:" + this.headerInfo.Phone;
          data[0].mailHref = "mailto:" + this.headerInfo.Mail;
        }
      })
    );
  }

  /**
     * getTopShopMenu
     */
  getDataMenuHead() {
    const param = {};
    this.subscription.push(this.api.excuteAllByWhat(param, '500').subscribe(result => {
      let data = result.data;
      if (data.length > 0) {
        if (this.userValue == null) {
          data['6']['Link'] = '/login/dang-nhap';
        }

        (data || []).map(item => {
          item['childs'] = [];
          (data || []).map(item1 => {
            if (+item1.IdParent === +item.id) {
              item['childs'].push(item1);
            }
          })
          return item
        });

        this.dataMenu = data.filter(item => +item.IdParent === 0);
      }
    }));
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
  navigatePageShopMenu(cateRecipe): void {
    let formatTitle = this.api.cleanAccents(cateRecipe.Name).split(' ').join('-');
    let re = /\//gi;
    formatTitle = formatTitle.replace(re, '');
    this.router.navigate(['c1-list-menu' + '/' + cateRecipe.id + '/' + formatTitle]);
  }

  /**
   * getTopRecipe
   */
  getFooter() {
    const param = {};
    this.subscription.push(
      this.api.excuteAllByWhat(param, "300").subscribe((result) => {
        let data = result.data;
        if (result.status == true && data.length > 0) {
          this.dataFooter = data[0];
        }
      })
    );
  }

  //log out candidate
  logOut() {
    this.api.logoutStaff();
    location.reload();
  }

  /**                                                            
   * get list City
   */
  getListCity() {
    this.subscription.push(this.api.excuteAllByWhat({}, '2500')
      .subscribe((result) => {
        this.CityDatas = result;
        this.getDistrictWithCity(this.CityDatas[0].id)
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
      if (result) {
        this.DistrictDatas = result;
        this.inputOrderMenu.IdDistrict = result[0].id;
      }
    }))
  }

  onCheckEmail(Email) {
    if (!this.isDisabledEmail) {
      const param = {
        Email: Email
      };

      this.subscription.push(this.api.excuteAllByWhat(param, '2011').subscribe(result => {
        if (result.length > 0) {
          this.isCheckEmail = true;

        } else {
          this.isCheckEmail = false;
        }
      }));
    }

  }



  /**
   * onBtnSummit
   */
  onBtnSummit() {
    let what = '2009';
    if (this.isRegister) {
      what = '2010';
    }
    if (!this.isRegister) {
      if (!this.form.invalid) {
        this.subscription.push(this.api.excuteAllByWhat(this.inputOrderMenu, what).subscribe(result => {
          let data = result.data;
          if (result.status) {
            localStorage.setItem("staffSubject", JSON.stringify(data));
            this.api.staffSubject.next(data);
            this.api.showSuccess('Đăng nhập thành công');
            //redirect to last page
            if (this.getUrl == '' || this.getUrl == undefined) {
              this.router.navigate(["/"]);
            } else {
              this.router.navigate([this.getUrl]);
            }
            this.form.reset();
            setTimeout(() => {
              window.location.reload();
            }, 500);
          } else {
            setTimeout(() => {
              this.api.showError(' Vui lòng đăng ký lại');
              this.updateInformation.nativeElement.click();
            }, 1000);
          }

        }))
      } else {
        setTimeout(() => {
          this.api.showError('Vui lòng điền đầy đủ thông tin để đăng ký');
          this.updateInformation.nativeElement.click();
        }, 1000);
      }
    } else {
      if (!this.form.invalid && !this.form1.invalid && !this.isCheckEmail) {

        this.subscription.push(this.api.excuteAllByWhat(this.inputOrderMenu, what).subscribe(result => {

          let data = result.data;
          if (result.status) {
            localStorage.setItem("staffSubject", JSON.stringify(data));
            this.api.staffSubject.next(data);
            this.api.showSuccess('Đăng nhập thành công');
            //redirect to last page
            if (this.getUrl == '' || this.getUrl == undefined) {
              this.router.navigate(["/"]);
            } else {
              this.router.navigate([this.getUrl]);
            }
            this.form.reset();
            this.form1.reset();
            setTimeout(() => {
              window.location.reload();
            }, 500);
          } else {
            setTimeout(() => {
              this.api.showError(' Vui lòng đăng ký lại');
              this.updateInformation.nativeElement.click();
            }, 1000);
          }

        }))
      } else {
        setTimeout(() => {
          this.api.showError('Vui lòng điền đầy đủ thông tin để đăng ký');
          this.updateInformation.nativeElement.click();
        }, 1000);
      }
    }
  }

  /**
   * btnLogin
   */
  btnLogin() {
    const param = {
      Email: this.inputLogin.Email,
      Password: this.inputLogin.Password,
    };
    if (!this.form2.invalid) {
      this.subscription.push(this.api.excuteAllByWhat(param, '2012').subscribe(result => {
        // let data = result.data;
        if (result != null) {
          localStorage.setItem("staffSubject", JSON.stringify(result));
          this.api.staffSubject.next(result);
          this.api.showSuccess('Đăng nhập thành công');
          //redirect to last page
          if (this.getUrl == '' || this.getUrl == undefined) {
            this.router.navigate(["/"]);
          } else {
            this.router.navigate([this.getUrl]);
          }
          setTimeout(() => {
            window.location.reload();
          }, 500);
        } else {
          setTimeout(() => {
            this.api.showError(' Mật khẩu hoặc Email chưa đúng');
            this.login.nativeElement.click();
          }, 1000);
        }
      }));
    } else {
      setTimeout(() => {
        this.api.showError('Vui lòng nhập đầy đủ thông tin');
        this.login.nativeElement.click();
      }, 1000);
    }

  }


  /**
   * get Shop Menu Categories
   */
  getShopMenuCategories() {
    const param = {};
    this.subscription.push(
      this.api.excuteAllByWhat(param, '5900').subscribe((result) => {
        let data = result.data;
        if (result.status == true && data.length > 0) {
          this.shopMenuCatgores = data;
        }
      })
    );
  }

  /**
  * onBtnMenuAll
  */
  onBtnMenuAll() {
    const url = '/c3-show-all/0/tat-ca-thuc-don';
    this.router.navigate([url.toLowerCase()]);

  }

  /**
  * onBtnMenuAll
  */
  onBtnRecipeAll() {
    const url = '/c3-show-all/1/tat-ca-cong-thuc';
    this.router.navigate([url.toLowerCase()]);

  }

  /**
  * get Shop Menu Categories
  */
  getShopMenuMobile() {
    const param = {};
    this.subscription.push(
      this.api.excuteAllByWhat(param, '5901').subscribe((result) => {

        let data = result.data;
        if (result.status == true && data.length > 0) {
          this.shopMenuMobile = data;
        } else {
          this.shopMenuMobile = [];
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

    //History user Shop menu
    if (this.idUser != null || this.idUser != undefined) {
      const param = {
        'idUser': this.idUser,
        'idShopMenu': id
      }
      this.subscription.push(
        this.api.excuteAllByWhat(param, '620').subscribe((result) => {
        })
      );
    }

    this.router.navigate([url.toLowerCase()]);
  }
}
