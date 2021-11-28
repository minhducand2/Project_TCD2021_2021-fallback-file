import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { ApiService } from 'src/app/common/api-service/api.service';
import { MustMatch } from 'src/app/common/validations/must-match.validator';
import { Md5 } from 'ts-md5';
import { SEOService } from 'src/app/common/api-service/seo.service';
import { ViewChild } from '@angular/core';
import { ElementRef } from '@angular/core';
@Component({
  selector: 'app-c1-paypal',
  templateUrl: './c1-paypal.component.html',
  styleUrls: ['./c1-paypal.component.scss']
})
export class C1PaypalComponent implements OnInit,OnDestroy {

  // data binding
  staff = {
    id: '',
    IdUserStatus: '',
    Fullname: '',
    Email: '',
    Password: '',
    Avatar: '',
    IdRole: '',
    CreatedAt: '',
    UpdatedAt: '',
    IdAccount: '',
    Phone: '',
  };

  staffInfoLogin = {
    id: '',
    IdUserStatus: '',
    Fullname: '',
    Email: '',
    Password: '',
    Avatar: '',
    IdRole: '',
    CreatedAt: '',
    UpdatedAt: '',
    IdAccount: '',
    Phone: '',
    Address: '',
    IdCity: '',
    IdDistrict: ''
  };

  CityDatas: any[] = [];
  DistrictDatas: any[] = [];

  // value hide
  hide = true;
  hide1 = true;
  hide2 = true;

  // password
  passwordMdOld: any;
  passwordMdNew: any;

  // old password
  oldpassword: string;

  // new password
  newpassword: string;

  // repassword
  repassword: string;

  // validate
  form: FormGroup;

  formresetpass: FormGroup;

  /** for table */
  subscription: Subscription[] = [];

  // flag insert
  insertFlag: boolean = false;
  isImg: boolean = true;

  // url Current
  urlCurrent: string = '';

  // binding uploads image or file
  @ViewChild("inputImageThumbnail", { static: false }) inputImageThumbnail: ElementRef;
  ImagesThumbnailPath: string = "";


  /**
   * constructor
   * @param api
   * @param formBuilder
   */
  constructor(private api: ApiService, private formBuilder: FormBuilder, private seoService: SEOService) {
    // add validate for controls
    this.form = this.formBuilder.group({
      name: [null, [Validators.required, Validators.minLength(5), Validators.maxLength(50)]],
      email: [null, [Validators.required, Validators.minLength(5), Validators.maxLength(30)]],
      phone: [null, [Validators.required, Validators.pattern('[0-9]*'), Validators.minLength(10), Validators.maxLength(11)]],
      address: [null, [Validators.required, Validators.minLength(5), Validators.maxLength(150)]],
      City: [null, [Validators.required,],],

      District: [
        null, [Validators.required,],],
    });

    //form validation
    this.formresetpass = this.formBuilder.group({
      password: [null, [Validators.required, Validators.minLength(8), Validators.maxLength(50)]],
      newpassword: [null, [Validators.required, Validators.minLength(8), Validators.maxLength(50),],],
      repassword: [null, [Validators.required, Validators.minLength(8), Validators.maxLength(50),],],
    },
      //check newPassword == rePassword
      { validator: MustMatch('newpassword', 'repassword') }
    );
  }

  /**
   * ngOnInit
   */
  ngOnInit(): void {
    // on Close Sidebar Mobile
    // this.onCloseSidebarMobile();

    //scroll top mobile
    this.api.scrollToTop();
    // get staff value
    this.staff = this.api.getStaffValue;
    // getDataStaffLogin

    // this.getListCity();
    // url Current
    this.urlCurrent = this.api.getUrl();

    let title = '';
    if (this.urlCurrent.indexOf('/en/') == -1) {
      title = 'Thay đôi mật khẩu';
    } else {
      title = 'Info-account'
    }

    //Seo title
    this.seoService.setTitle(title);
    // this.getDataStaffLogin();
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
   * get Data Staff Login
   */
  getDataStaffLogin() {
    const param = {
      'id': this.staff.id
    };
    // get data staff login
    this.subscription.push(this.api.excuteAllByWhat(param, '3104').subscribe((result) => {
      let data = result.data;
      if (result.status == true) {
        // set logo default
        if (data[0]['Avatar'] == '' || data[0]['Avatar'] == undefined) {
          data[0]['Avatar'] = "https://rolievn.com/api/uploads/images/logo-admin.png"
        }

        this.staffInfoLogin = data[0];
        this.getListCity();
      } else {
        this.api.showError('Vui lòng tải lại trang');
      }
    })
    );

  }


  /**
   * get list City
   */
  getListCity() {
    this.subscription.push(this.api.excuteAllByWhat({}, '3700')
      .subscribe((result) => {
        let data = result.data;
        this.CityDatas = data;

      })
    );
    this.getDistrictWithCity();
  }

  /**
   * get district
   */
  getDistrictWithCity() {
    const param = {
      idcity: this.staffInfoLogin.IdCity,
    }
    this.subscription.push(this.api.excuteAllByWhat(param, '3801').subscribe(result => {
      let data = result.data;
      if (result.status == true && data.length > 0) {
        this.DistrictDatas = data;

      }
    }))
  }

  /**
   * on Close Sidebar Mobile
   */
  // onCloseSidebarMobile() {
  //   $(document).ready(function () {
  //     $(".row-offcanvas-right").removeClass("active");
  //   });
  // }

  /**
   * on update info staff
   */
  onBtnUpdateClick() {
    const param = {
      'id': this.staffInfoLogin['id'],
      'Fullname': this.staffInfoLogin?.Fullname,
      'Phone': this.staffInfoLogin.Phone,
      'Address': this.staffInfoLogin.Address,
      'IdCity': this.staffInfoLogin.IdCity,
      'IdDistrict': this.staffInfoLogin.IdDistrict
    };

    //check validation
    if (this.form.status != 'VALID') {
      this.api.showWarning('Thông tin nhập chưa hợp lệ ');
      return;
    } else {

      //update profile
      this.subscription.push(this.api.excuteAllByWhat(param, '3113').subscribe((result) => {
        if (result.status == true) {
          this.api.showSuccess('Cập nhật thành công ');
          window.location.reload();
        } else {
          this.api.showError('Xử lý thất bại');
        }
      })
      );
    }
  }

  /**
   * on update img
   */
  onBtnUploadImgClick() {
    const param = {
      'id': this.staffInfoLogin['id'],
      'Avatar': this.staffInfoLogin?.Avatar,
    };
    //update img in profile
    this.subscription.push(this.api.excuteAllByWhat(param, '3112').subscribe((result) => {
      if (result.status == true) {
        this.api.showSuccess('Cập nhật thành công ');
        this.getDataStaffLogin();
        this.isImg = true;
      } else {
        this.api.showError('Xử lý thất bại');
      }
    })
    );

  }

  /**
   * on Change Password
   */
  onChangePassClick() {
    // return if error
    if (this.formresetpass.status != 'VALID') {
      this.api.showWarning('Vui lòng nhập các mục đánh dấu *');
      return;
    }

    //encode password
    this.passwordMdNew = Md5.hashAsciiStr(this.newpassword).toString();
    this.passwordMdOld = Md5.hashAsciiStr(this.oldpassword).toString();
    this.oldpassword = this.passwordMdOld;

    const param = {
      'Password': this.passwordMdNew,
      'id': this.staff.id,
    };

    //if password enter = password login
    if (this.oldpassword == this.staffInfoLogin['password']) {
      // this.staff.password = this.newpassword;

      //update password
      this.subscription.push(this.api.excuteAllByWhat(param, '7').subscribe((result) => {
        if (result.status == true) {
          this.api.showSuccess('Cập nhật mật khẩu thành công ');
          this.formresetpass.reset();

          // load data staff to get password new
          window.location.reload();
        }
      })
      );
    } else {
      this.api.showWarning('Mật khẩu cũ chưa đúng hoặc mật khẩu mới chưa trùng nhau ');
      this.formresetpass.reset();
    }
  }

  /**
   * on button Cancel
   */
  onBtnCancelClick() {
    this.formresetpass.reset();
  }

  /**
   * on Images Upload image Click
   */
  onImagesUploadThumbnailClick() {
    this.api.uploadImageCore(this.inputImageThumbnail).subscribe((data) => {
      this.staffInfoLogin.Avatar = "https://rolievn.com/api/uploads/images/" + data;
      this.isImg = false;
    });
  }


}
