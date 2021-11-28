import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { map } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { HttpHeaders } from '@angular/common/http';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';
import { Router } from '@angular/router';
import { isPlatformBrowser } from '@angular/common';
import { Security } from './security';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable()
export class ApiService {
  private url = '';
  private getAllWhatUrl = '';
  protected se: Security = new Security();
  public lang: string = 'vn';
  public BASEURL: string = 'http://334.124.199.49:8000/';
  public BASE_UPLOAD_URL: string = 'http://localhost:8200/P5Upload';

  // public SERVER_URL = this.BASEURL + '/Controller/Upload.php';
  public SENTMAIL_URL = 'https://rolievn.com/api/Controller/BaseMail.php';
  public IMAGE_UPLOAD_URL = 'http://334.124.199.49:8000/api/upload';
  public FILE_UPLOAD_URL = 'http://334.124.199.49:8000/api/upload';

  // authrize
  public sysmemberSubject: BehaviorSubject<any>;
  public currentSysMember: Observable<any>;
  public accountSubject: BehaviorSubject<any>;
  public currentAccount: Observable<any>;
  public token: BehaviorSubject<any>;
  public tokenCurent: Observable<any>;

  constructor(
    public httpClient: HttpClient,
    private toastrService: ToastrService,
    private router: Router,
    @Inject(PLATFORM_ID) private platform: Object,
    private spinner: NgxSpinnerService
  ) {
    this.sysmemberSubject = new BehaviorSubject<any>(
      JSON.parse(localStorage.getItem('sysmemberSubject'))
    );
    this.accountSubject = new BehaviorSubject<any>(
      JSON.parse(localStorage.getItem('accountSubject'))
    );

    this.token = new BehaviorSubject<any>(
      localStorage.getItem('token')
    );
    this.tokenCurent = this.token.asObservable();

    this.currentAccount = this.accountSubject.asObservable();

    // this.url = this.BASEURL + '/P1Controller/C1Admin/SelectAllByWhat.php';
    this.url = 'http://334.124.199.49:8000/api/v1/ProxyApi/SelectAllByWhat';
  }

  /**
   *
   * @param param
   * @param what
   */
  public excuteAllByWhat(param: any, what: string): Observable<any> {
    // this.spinner.show();

    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': (this.token.value != null ? this.token.value : ''),
      }),
    };

    param.what = what;
    this.getAllWhatUrl = this.url;
    // console.log('Param input', JSON.stringify(param)); 

    return this.httpClient
      .post<any>(this.getAllWhatUrl, JSON.stringify(param), httpOptions)
      .pipe(
        map((response: Response) => {
          // console.log(response);
          if (response['status_auth'] == false) {
            this.showWarning('Bạn không có quyền hoặc hết thời gian truy cập!');
            setTimeout(() => {
              window.location.href = '/';
            }, 500);
            return;
          }

          // this.spinner.hide();
          return response;
        })
      );
  }

  /**
   * logout SysMember
   */
  public logoutSysMember() {
    localStorage.removeItem('sysmemberSubject');
    this.router.navigate(['/login']);
  }

  /**
   * logout account
   */
  public logoutAccount() {
    this.remoteAllLoginAccount();
    window.location.href = '';
  }

  /**
   * remoteAllLoginAccount
   */
  public remoteAllLoginAccount() {
    localStorage.removeItem('accountSubject');
    localStorage.removeItem('token');
    this.accountSubject = new BehaviorSubject<any>(
      JSON.parse(localStorage.getItem('accountSubject'))
    );
    this.token = new BehaviorSubject<any>(
      JSON.parse(localStorage.getItem('token'))
    );
  }

  /**
   * get account Value
   */
  public get getAccountValue(): any {
    if (this.accountSubject.value == null) {
      this.accountSubject = new BehaviorSubject<any>(
        JSON.parse(localStorage.getItem('accountSubject'))
      );
    }
    return this.accountSubject.value;
  }

  /**
   * format Html Tag
   * @param content
   */
  public formatHtmlTag(content) {
    let result: string;
    let dummyElem = document.createElement('DIV');
    dummyElem.innerHTML = content;
    document.body.appendChild(dummyElem);
    result = dummyElem.textContent;
    document.body.removeChild(dummyElem);
    return result;
  }

  /**
   * convert To Data
   * @param data
   */
  public convertToData(data): any[] {
    data = JSON.parse(data + '');
    let result: any[] = [];
    data.forEach((item) => {
      item.fields.id = item.pk;
      result.push(item.fields);
    });
    return result;
  }

  /**
   *
   * @param date
   */
  public formatDate(date: Date): string {
    const month = date.getMonth() + 1;
    const day = date.getDate();
    return (
      // date.getFullYear() +
      // '-' +
      // (month > 9 ? month : '0' + month) +
      // '-' +
      // (day > 9 ? day : '0' + day)
      date.getFullYear() +
      '-' +
      (date.getMonth() + 1 > 9
        ? date.getMonth() + 1
        : '0' + (date.getMonth() + 1)) +
      '-' +
      (date.getDate() > 9 ? date.getDate() : '0' + date.getDate()) +
      '\x20' +
      (date.getHours() > 9 ? date.getHours() : '0' + date.getHours()) +
      ':' +
      (date.getMinutes() > 9 ? date.getMinutes() : '0' + date.getMinutes()) +
      ':' +
      (date.getSeconds() > 9 ? date.getSeconds() : '0' + date.getSeconds())
    );
  }

   /**
   *
   * @param date
   */
    public formatDateDOTNET(date: Date): string { 
      let now = new Date();
      return ( 
        date.getFullYear()  +
        '/' +
        (date.getMonth() + 1 > 9
          ? date.getMonth() + 1
          : '0' + (date.getMonth() + 1)) +
        '/' +
        (date.getDate() > 9 ? date.getDate() : '0' + date.getDate()) +
        '\x20' +
        (now.getHours() > 9 ? now.getHours() : '0' + now.getHours()) +
        ':' +
        (now.getMinutes() > 9 ? now.getMinutes() : '0' + now.getMinutes()) +
        ':' +
        (now.getSeconds() > 9 ? now.getSeconds() : '0' + now.getSeconds())
      );
    }

  /**
   *
   * @param date
   */
  public formatDateDDMMYY(date: Date): string {
    const month = date.getMonth() + 1;
    const day = date.getDate();
    return (
      date.getFullYear() +
      '-' +
      (month > 9 ? month : '0' + month) +
      '-' +
      (day > 9 ? day : '0' + day)

    );
  }

  /**
  *
  * @param date
  */
  public formatDateExpiryDate(date: Date): string {
    const month = date.getMonth() + 1;
    const day = date.getDate();
    return (
      (day > 9 ? day : '0' + day) +
      '-' +
      (month > 9 ? month : '0' + month) +
      '-' +
      date.getFullYear()

    );
  }

  /**
   *
   * @param date
   */
  public formatDateTime(date: Date): string {
    return (
      date.getFullYear() +
      '-' +
      (date.getMonth() + 1 > 9
        ? date.getMonth() + 1
        : '0' + (date.getMonth() + 1)) +
      '-' +
      (date.getDate() > 9 ? date.getDate() : '0' + date.getDate()) +
      '\x20' +
      (date.getHours() > 9 ? date.getHours() : '0' + date.getHours()) +
      ':' +
      (date.getMinutes() > 9 ? date.getMinutes() : '0' + date.getMinutes()) +
      ':' +
      (date.getSeconds() > 9 ? date.getSeconds() : '0' + date.getSeconds())
    );
  }

  /**
   *
   * @param param
   * @param what
   */
  public excuteAllByWhatWithUrl(
    url: string,
    param: any,
    what: string
  ): Observable<any> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Authorization: 'my-auth-token',
        'Access-Control-Allow-Origin': '*',
      }),
    };

    this.getAllWhatUrl = this.url + what;

    console.log('Param input', JSON.stringify(param));

    return this.httpClient
      .post<any>(url, JSON.stringify(param), httpOptions)
      .pipe(map((response: Response) => response.json()));
  }

  public showError(mess: string) {
    this.toastrService.error('Pinks Ways!', mess, {
      timeOut: 1500,
      progressBar: true,
    });
  }

  public showSuccess(mess: string) {
    this.toastrService.info('Pinks Ways!', mess + '!', {
      timeOut: 1500,
      progressBar: true,
    });
  }

  public showWarning(mess: string) {
    this.toastrService.warning('Pinks Ways!', mess + '!', {
      timeOut: 1500,
      progressBar: true,
    });
  }

  /**
   * bỏ dấu tiếng việt để search
   */
  public cleanAccents(str: string): string {
    str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, 'a');
    str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, 'e');
    str = str.replace(/ì|í|ị|ỉ|ĩ/g, 'i');
    str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, 'o');
    str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, 'u');
    str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, 'y');
    str = str.replace(/đ/g, 'd');
    str = str.replace(/À|Á|Ạ|Ả|Ã|Â|Ầ|Ấ|Ậ|Ẩ|Ẫ|Ă|Ằ|Ắ|Ặ|Ẳ|Ẵ/g, 'A');
    str = str.replace(/È|É|Ẹ|Ẻ|Ẽ|Ê|Ề|Ế|Ệ|Ể|Ễ/g, 'E');
    str = str.replace(/Ì|Í|Ị|Ỉ|Ĩ/g, 'I');
    str = str.replace(/Ò|Ó|Ọ|Ỏ|Õ|Ô|Ồ|Ố|Ộ|Ổ|Ỗ|Ơ|Ờ|Ớ|Ợ|Ở|Ỡ/g, 'O');
    str = str.replace(/Ù|Ú|Ụ|Ủ|Ũ|Ư|Ừ|Ứ|Ự|Ử|Ữ/g, 'U');
    str = str.replace(/Ỳ|Ý|Ỵ|Ỷ|Ỹ/g, 'Y');
    str = str.replace(/Đ/g, 'D');
    // Combining Diacritical Marks
    str = str.replace(/\u0300|\u0301|\u0303|\u0309|\u0323/g, ''); // huyền, sắc, hỏi, ngã, nặng
    str = str.replace(/\u02C6|\u0306|\u031B/g, ''); // mũ â (ê), mũ ă, mũ ơ (ư)

    return str;
  }

  /**
   * upload image Core
   */
  public uploadImageCore(inputFileAvatar): Observable<any> {
    const uploadFileFinish = new Observable((observer) => {
      let files = [];
      const fileUpload = inputFileAvatar.nativeElement;

      fileUpload.onchange = () => {
        for (let index = 0; index < fileUpload.files.length; index++) {
          const file = fileUpload.files[index];
          files.push({ data: file, inProgress: false, progress: 0 });
        }

        // upload file
        inputFileAvatar.nativeElement.value = '';
        files.forEach((file) => {
          // upload file image
          const formData = new FormData();
          formData.append('file', file.data);
          const current_datetime = new Date().toISOString();
          formData.append('current_datetime', current_datetime);
          file.inProgress = true;
          this.spinner.show();
          // post image to server
          this.httpClient
            .post<any>(this.IMAGE_UPLOAD_URL, formData, {})
            .subscribe((data) => { });

          setTimeout(() => {
            observer.next(current_datetime + '-' + file.data.name);
            this.spinner.hide();
          }, 2000);
        });
      };
      fileUpload.click();
    });

    return uploadFileFinish;
  }

  /**
   * upload File Core
   */
  public uploadFileCore(inputFileAvatar): Observable<any> {
    const uploadFileFinish = new Observable((observer) => {
      let files = [];
      const fileUpload = inputFileAvatar.nativeElement;

      fileUpload.onchange = () => {
        for (let index = 0; index < fileUpload.files.length; index++) {
          const file = fileUpload.files[index];
          files.push({ data: file, inProgress: false, progress: 0 });
        }

        // upload file
        inputFileAvatar.nativeElement.value = '';
        files.forEach((file) => {
          // upload file image
          const formData = new FormData();
          formData.append('file', file.data);
          const current_datetime = new Date().toISOString();
          formData.append('current_datetime', current_datetime);
          file.inProgress = true;
          this.spinner.show();

          // post file to server
          this.httpClient
            .post<any>(this.FILE_UPLOAD_URL, formData, {})
            .subscribe((data) => { });

          setTimeout(() => {
            observer.next(current_datetime + '-' + file.data.name);
            this.spinner.hide();
          }, 2000);
        });
      };
      fileUpload.click();
    });

    return uploadFileFinish;
  }

  /**
   * scroll to top with angular universal
   */
  public scrollToTop() {
    if (isPlatformBrowser(this.platform)) {
      window.scroll({ left: 0, top: 0 });
    }
  }

  /**
   * scroll to top with angular universal
   */
  public scrollToTopPosition(left, top) {
    if (isPlatformBrowser(this.platform)) {
      window.scroll({ left: left, top: top });
    }
  }
}
