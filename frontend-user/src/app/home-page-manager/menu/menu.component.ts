import { Component, OnInit } from '@angular/core';
import { LoginCookie } from '../../common/core/login-cookie';
import { ApiService } from 'src/app/common/api-service/api.service';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
    selector: 'app-menu',
    templateUrl: './menu.component.html',
    styleUrls: ['./menu.component.scss']
})
export class MenuComponent implements OnInit {

    menuFlag: boolean = false;
    menuMobileFlag: boolean = false;
    settingButton: boolean = false;
    searchFlag: boolean = false;
    isMobile: boolean = false;
    isShowMenu: boolean = false;
    staff = {
        Avatar: '',
        Fullname: '',
    };
    idStaff: number;

    nameTitle: string = '';
    subscription: Subscription[] = [];

    // data binding
    headerInfo = {
        Phone: '',
        Mail: '',
        mailHref: '',
        phoneHref: '',
        Logo: '',
    };

    // menu
    isShow: boolean = false;
    info: any = null;
    listCateRecipe: any;
    isShowMenuMobile: boolean = false;
    isShowMenuPassword: boolean = false;


    Promotion: any = {
        Name: '',
        Link: '',
    };
    isCheckPromotion: boolean = false;
    infoUser: any;
    userValue: any = {
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
        Point: '',
    };

    constructor(private login: LoginCookie, public api: ApiService,
        private router: Router,) {
        // company value
        this.staff = this.api.getStaffValue;
        this.idStaff = this.api.getStaffValue?.id;
    }

    /**
     * ngOnInit
     */
    ngOnInit() {
        this.isMobile = this.isMobileDevice();
        this.getDataStaffLogin();
        this.getHeaderInfo();
    }

    /**
     * getDataStaffLogin
     */
    getDataStaffLogin() {
        const param = {
            'id': this.idStaff
        };
        // get data staff login
        this.subscription.push(this.api.excuteAllByWhat(param, '2004').subscribe((result) => {
            if (result) { 
                // set logo default
                if (result['Avatar'] == '' || result['Avatar'] == undefined) {
                    result['Avatar'] = "https://rolievn.com/api/uploads/images/logo-admin.png"
                }

                //check authkey 
                if (result['authkey'] == "" || result['authkey'] == null) {
                    this.isShowMenuPassword = true;
                }
                this.userValue = result;


            } else {
                this.api.showError('Vui lòng tải lại trang');
            }
        })
        );

    }

    isMobileDevice() {
        return (typeof window.orientation !== "undefined") || (navigator.userAgent.indexOf('IEMobile') !== -1);
    };

    onClickOffMenu() {
        if (this.isMobile) {
            this.isShowMenu = !this.isShowMenu;
        }
    }
    /**
   * ngOnDestroy
   */
    ngOnDestroy() {
        this.subscription.forEach((item) => {
            item.unsubscribe();
        });
    }

    //log out
    logOut() {
        this.api.logoutStaff();
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



}
