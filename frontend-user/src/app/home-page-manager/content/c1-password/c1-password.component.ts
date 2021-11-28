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
  selector: 'app-c1-password',
  templateUrl: './c1-password.component.html',
  styleUrls: ['./c1-password.component.scss']
})
export class C1PasswordComponent implements OnInit,OnDestroy {

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
    Fullname: '',
    Email: '',
    Avatar: '',
    UpdatedAt: '',
    Phone: '',
    Address: '',
    IdCity: '',
    IdDistrict: '',
    Sex: '',
    Password: '',
  };

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

    //form validation
    this.form = this.formBuilder.group({
      password: [null, [Validators.required, Validators.minLength(6), Validators.maxLength(50)]],
      newpassword: [null, [Validators.required, Validators.minLength(6), Validators.maxLength(50),],],
      repassword: [null, [Validators.required, Validators.minLength(6), Validators.maxLength(50),],],
    },
      //check newPassword == rePassword
      { validator: MustMatch('newpassword', 'repassword') }
    );
  }

  /**
   * ngOnInit
   */
  ngOnInit(): void {

    //scroll top mobile
    this.api.scrollToTop();
    // get staff value
    this.staff = this.api.getStaffValue;

    let title = 'Change Pasword';

    //Seo title
    this.seoService.setTitle(title);
    this.getDataStaffLogin();
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
    this.subscription.push(this.api.excuteAllByWhat(param, '2004').subscribe((result) => {
      if (result) {
        // set logo default
        if (result['Avatar'] == '' || result['Avatar'] == undefined) {
          result['Avatar'] = "https://rolievn.com/api/uploads/images/logo-admin.png"
        }
        this.staffInfoLogin = result;
      } else {
        this.api.showError('Vui lòng tải lại trang');
      }
    })
    );
  }

  /**
   * on Change Password
   */
  onChangePassClick() {
    // return if error
    if (this.form.status != 'VALID') {
      this.api.showWarning('Vui lòng nhập các mục đánh dấu *');
      return;
    }
    //encode password
    this.passwordMdNew = Md5.hashStr(this.newpassword).toString().toLocaleUpperCase();
    this.passwordMdOld = Md5.hashStr(this.oldpassword).toString().toLocaleUpperCase();
    this.oldpassword = this.passwordMdOld;

    const param = {
      'Password': this.passwordMdNew,
      'id': this.staff.id,
    };

    //if password enter = password login
    if (this.oldpassword == this.staffInfoLogin['Password']) {
      //update password
      this.subscription.push(this.api.excuteAllByWhat(param, '2013').subscribe((result) => {

        if (result) {
          this.api.showSuccess('Cập nhật mật khẩu thành công ');
          this.form.reset();
          // load data staff to get password new
          window.location.reload();
        } else {
          this.api.showWarning('Xử lý thất bại. Vui lòng thử lại');
        }
      })
      );
    } else {
      this.api.showWarning('Mật khẩu cũ chưa đúng hoặc mật khẩu mới chưa trùng nhau ');
      this.form.reset();
    }
  }

  /**
   * on button Cancel
   */
  onBtnCancelClick() {
    this.form.reset();
  }



}
