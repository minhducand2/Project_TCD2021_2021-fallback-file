import { Component, OnInit } from '@angular/core';
import { LoginCookie } from '../../common/core/login-cookie';
import { ApiService } from 'src/app/common/api-service/api.service';
// import $ from 'jquery';
import * as $ from 'jquery';
import { Subscription } from 'rxjs';
@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss'],
})
export class MenuComponent implements OnInit {
  //menu flag
  menuFlag: boolean = false;
  menuMobileFlag: boolean = false;
  menuMobileStaff: boolean = false;
  settingButton: boolean = false;
  searchFlag: boolean = false;
  isMobile: boolean = false;
  idStaff: Number;

  // flag for menu
  navLeft: boolean = false;
  navLeftMobile: boolean = false;
  settingFlag: boolean = false;

  //data permission menu
  isPermissionMenu1: boolean = false;
  isPermissionMenu2: boolean = false;
  isPermissionMenu3: boolean = false;

  // show dropdown (logout/myprofile/changepasswork)
  isShow: boolean = false;
  info: any = null;

  //pubic is visited
  public isVisited = false;
  public checkVisited() {
    // reverse the value of property
    this.isVisited = !this.isVisited;
  }

  //data binding
  staff: any;

  staffInfoLogin = {
    id: '',
    email: '',
    name: '',
    passwork: '',
    img: '',
    created_date: '',
  };

  subscription: Subscription[] = [];

  dataMenus: any[] = [];

  /**
   * constructor
   * @param login
   * @param api
   */
  constructor(private login: LoginCookie, public api: ApiService) {
    // get staff value
    this.staff = this.api.getAccountValue;
  }

  /**
   * ngOnInit
   */
  ngOnInit() {
    this.staff = this.api.accountSubject.value;
    this.idStaff = this.api.accountSubject.value.id;
    this.isMobile = this.isMobileDevice();
    // load data menu
    this.onLoadDataMenu();

    // this.onLoadPermission();

    // toggle Sidebar Click
    this.toggleSidebarClick();

    // toggle Sidebar Mobile Click
    this.toggleSidebarMobileClick();

    // getDataStaffLogin
    this.getDataStaffLogin();
  }

  /**
   * ngAfterViewInit
   */
  ngAfterViewInit() {
    // process click of menu response
    $(document).ready(function ($) {
      'use strict';
      //Open submenu on hover in compact sidebar mode and horizontal menu mode
      $(document).on(
        'mouseenter mouseleave',
        '.sidebar .nav-item',
        function (ev) {
          var body = $('#body');
          var sidebarIconOnly = body.hasClass('sidebar-icon-only');
          var horizontalMenu = body.hasClass('horizontal-menu');
          var sidebarFixed = body.hasClass('sidebar-fixed');
          if (!('ontouchstart' in document.documentElement)) {
            if (sidebarIconOnly || horizontalMenu) {
              if (sidebarFixed) {
                if (ev.type === 'mouseenter') {
                  body.removeClass('sidebar-icon-only');
                }
              } else {
                var $menuItem = $(this);
                if (ev.type === 'mouseenter') {
                  $menuItem.addClass('hover-open');
                } else {
                  $menuItem.removeClass('hover-open');
                }
              }
            }
          }
        }
      );
    });
  }

  /**
   * ngOnDestroy
   */
  ngOnDestroy(): void {
    throw new Error('Method not implemented.');
  }

  /**
   * on format icon from <i class="fas fa-address-card"></i> to fas fa-address-card
   * @param icon
   */
  onFormatIcon(icon) {
    // check icon have ""
    let startCopet = icon.search('"');
    if (startCopet != -1) {
      icon = icon.substring(startCopet + 1);
      let endCopet = icon.search('"');
      icon = icon.substring(0, endCopet);
    }
    return icon;
  }

  /**
   * get Data Staff Login
   */
  getDataStaffLogin() {
    const param = {
      id: this.staff.id,
    };

    // get data staff login
    this.subscription.push(
      this.api.excuteAllByWhat(param, '6').subscribe((data) => {
        // data = data['result']; 
        if (data.length > 0) {
          // set logo default
          if (data[0]['img'] == '' || data[0]['img'] == undefined) {
            data[0]['img'] = '../../../assets/images/logo-admin.png';
          }

          this.staffInfoLogin = data[0];
        }
      })
    );
  }

  /**
   * get data grid
   */
  onLoadDataMenu() {
    const param = {
      idUser: this.staff.id,
    };

    this.subscription.push(
      this.api.excuteAllByWhat(param, '110').subscribe((data) => {
       
        // process data
        // data = data['result']; 

        if (data.length > 0) {
          let arr = [];
          let firstId = data[0].id;
          arr.push({
            id: data[0].id,
            IdParentMenu: data[0].IdParentMenu,
            IsGroup: data[0].IsGroup,
            Name: data[0].Name,
            Slug: data[0].Slug,
            Icon: data[0].Icon,
            Position: data[0].Position,
            Status: data[0].Status == null ? '0' : data[0].Status,
            IdRoleDetail: data[0].IdRoleDetail,
            IdRoleDetail1: data[0].IdRoleDetail1,
          });

          for (let i = 1; i < data.length; i++) {
            // check is group
            if (data[i].IsGroup == 1) {
              // add parent node
              if (data[i].id != firstId) {
                arr.push({
                  id: data[i].id,
                  IdParentMenu: data[i].IdParentMenu,
                  IsGroup: data[i].IsGroup,
                  Name: data[i].Name,
                  Slug: data[i].Slug,
                  Icon: data[i].Icon,
                  Position: data[i].Position,
                  Status: data[i].Status == null ? '0' : data[i].Status,
                  IdRoleDetail: data[i].IdRoleDetail,
                  IdRoleDetail1: data[i].IdRoleDetail1,
                  Nodes: [],
                });

                // add child node of group
                if (data[i].id1 != null) {
                  arr[arr.length - 1].Nodes.push({
                    id: data[i].id1,
                    IdParentMenu: data[i].IdParentMenu1,
                    IsGroup: data[i].IsGroup1,
                    Name: data[i].Name1,
                    Slug: data[i].Slug1,
                    Icon: data[i].Icon1,
                    Position: data[i].Position1,
                    Status: data[i].Status1 == null ? '0' : data[i].Status1,
                    IdRoleDetail: data[i].IdRoleDetail,
                    IdRoleDetail1: data[i].IdRoleDetail1,
                  });
                }

                // update first id
                firstId = data[i].id;
              } else {
                // add child node of group
                if (data[i].id1 != null) {
                  arr[arr.length - 1].Nodes.push({
                    id: data[i].id1,
                    IdParentMenu: data[i].IdParentMenu1,
                    IsGroup: data[i].IsGroup1,
                    Name: data[i].Name1,
                    Slug: data[i].Slug1,
                    Icon: data[i].Icon1,
                    Position: data[i].Position1,
                    Status: data[i].Status1 == null ? '0' : data[i].Status1,
                    IdRoleDetail: data[i].IdRoleDetail,
                    IdRoleDetail1: data[i].IdRoleDetail1,
                  });
                }
              }
            } else {
              // not group
              arr.push({
                id: data[i].id,
                IdParentMenu: data[i].IdParentMenu,
                IsGroup: data[i].IsGroup,
                Name: data[i].Name,
                Slug: data[i].Slug,
                Icon: data[i].Icon,
                Position: data[i].Position,
                Status: data[i].Status == null ? '0' : data[i].Status,
                IdRoleDetail: data[i].IdRoleDetail,
                IdRoleDetail1: data[i].IdRoleDetail1,
              });
            }
          }

          this.dataMenus = arr;
        }
      })
    );
  }

  /**
   * onLoadPermission
   */
  onLoadPermission() {
    // load permission
    if (this.staff.role.search('a:1') < 0) {
      this.isPermissionMenu1 = true;
    }

    if (this.staff.role.search('b:1') < 0) {
      this.isPermissionMenu2 = true;
    }

    if (this.staff.role.search('c:1') < 0) {
      this.isPermissionMenu3 = true;
    }
  }

  /**
   * is Mobile Device
   */
  isMobileDevice() {
    return (
      typeof window.orientation !== 'undefined' ||
      navigator.userAgent.indexOf('IEMobile') !== -1
    );
  }

  /**
   * onBtnLogOutStaffClick
   */
  onBtnLogOutStaffClick() {
    this.api.logoutAccount();
  }

  /**
   * logOut Staff
   */
  toggleSidebarClick() {
    $(document).ready(function () {
      $('.icon-toggle-sidebar').click(function () {
        $('html body').toggleClass('sidebar-icon-only');
      });
    });
  }

  /**
   * logOut Staff
   */
  toggleSidebarMobileClick() {
    $(document).ready(function () {
      $('.navbar-toggler-right').click(function () {
        $('.row-offcanvas-right').toggleClass('active');
      });
    });
  }

  /**
   * onToggleButtonDesktopClick
   */
  onToggleButtonDesktopClick() {
    this.navLeft = !this.navLeft;
  }

  /**
   * onToggleButtonMobileClick
   */
  onToggleButtonMobileClick() {
    this.navLeftMobile = !this.navLeftMobile;
  }

  onSettingButtonClick() {
    this.settingFlag = !this.settingFlag;
  }
}
