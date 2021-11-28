import { Component, EventEmitter, OnDestroy, OnInit, Output } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { ApiService } from 'src/app/common/api-service/api.service';
import { MustMatch } from 'src/app/common/validations/must-match.validator';
import { Md5 } from 'ts-md5';
import { SEOService } from 'src/app/common/api-service/seo.service';
import { ViewChild } from '@angular/core';
import { ElementRef } from '@angular/core';
import { HttpClient, HttpEventType } from '@angular/common/http';
@Component({
  selector: 'app-c1-account',
  templateUrl: './c1-account.component.html',
  styleUrls: ['./c1-account.component.scss']
})
export class C1AccountComponent implements OnInit,OnDestroy {

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
  form1: FormGroup;

  formresetpass: FormGroup;

  /** for table */
  subscription: Subscription[] = [];

  // flag insert
  insertFlag: boolean = false;
  isImg: boolean = true;

  // url Current
  urlCurrent: string = '';
  sex: any[] = [
    { id: '0', Name: 'Nữ' },
    { id: '1', Name: 'Nam' }
  ];


  // binding uploads image or file
  @ViewChild("inputImageThumbnail", { static: false }) inputImageThumbnail: ElementRef;
  ImagesThumbnailPath: string = "";

  public progress: number;
  public message: string;
  @Output() public onUploadFinished = new EventEmitter();


  /**
   * constructor
   * @param api
   * @param formBuilder
   */
  constructor(private api: ApiService,
    private formBuilder: FormBuilder,
    private seoService: SEOService,
    private http: HttpClient) {
    // get staff value
    this.staff = this.api.getStaffValue;

    // add validate for controls
    this.form = this.formBuilder.group({
      Name: [null, [Validators.required, Validators.minLength(5), Validators.maxLength(50)]],
      Email: [null,],
      Phone: [null, [Validators.required, Validators.pattern('[0-9]*')]],
      Sex: [null, [Validators.required]],
    });

    this.form1 = this.formBuilder.group({
      Address: [null, [Validators.required, Validators.minLength(5), Validators.maxLength(150)]],
      City: [null, [Validators.required]],
      District: [null, [Validators.required]],
      Avatar: [null],
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
    //scroll top mobile
    this.api.scrollToTop();

    this.getDataStaffLogin();

    this.getListCity();
    // url Current
    this.urlCurrent = this.api.getUrl();

    let title = 'Info-account';
    // if (this.urlCurrent.indexOf('/en/') == -1) {
    //   title = 'Thông tin tài khoản';
    // } else {
    //   title = 'Info-account'
    // }

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
      }
    }))
  }

  /**
   * public
   * @param files 
   * @returns 
   */
  public uploadFile = (files) => {
    if (files.length === 0) {
      return;
    }
    let fileToUpload = <File>files[0];
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    this.http.post(this.api.IMAGE_UPLOAD_URL, formData, { reportProgress: true, observe: 'events' })
      .subscribe(event => {
        if (event.type === HttpEventType.UploadProgress)
          this.progress = Math.round(100 * event.loaded / event.total);
        else if (event.type === HttpEventType.Response) {
          this.onUploadFinished.emit(event.body);
          this.staffInfoLogin.Avatar = this.api.BASEURL + event.body['dbPath'];
        }
      });
  }

  /**
   * on update info staff
   */
  onBtnUpdateClick() {

    this.staffInfoLogin.UpdatedAt = this.api.formatDateDOTNET(new Date());
    //check validation
    if (this.form.status != 'VALID') {
      this.api.showWarning('Thông tin nhập chưa hợp lệ ');
      return;
    } else {

      //update profile
      this.subscription.push(this.api.excuteAllByWhat(this.staffInfoLogin, '2002').subscribe((result) => {

        if (result) {
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
  // onBtnUploadImgClick() {
  //   const param = {
  //     'id': this.staffInfoLogin['id'],
  //     'Avatar': this.staffInfoLogin?.Avatar,
  //   };
  //   //update img in profile
  //   this.subscription.push(this.api.excuteAllByWhat(param, '3112').subscribe((result) => {
  //     if (result.status == true) {
  //       this.api.showSuccess('Cập nhật thành công ');
  //       this.getDataStaffLogin();
  //       this.isImg = true;
  //     } else {
  //       this.api.showError('Xử lý thất bại');
  //     }
  //   })
  //   );

  // }

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
