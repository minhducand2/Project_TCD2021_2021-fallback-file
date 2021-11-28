import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { map } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { HttpHeaders } from '@angular/common/http';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { Security } from './security';
import { Router } from '@angular/router';
import { isPlatformBrowser } from '@angular/common';
import { NgxSpinnerService } from 'ngx-spinner';
@Injectable()
export class ApiService {
    private url = "";
    private getAllWhatUrl = "";
    private getImageUrl = "";
    protected se: Security = new Security();
    public lang: string = 'vn';
    public BASEURL: string = 'http://34.124.199.49:8000/';
    public linkUrlSeo: string = 'http://localhost:4200/#';

    // CONSTAINT URL
    public SERVER_URL: string = "https://rolievn.com/api/Controller/Upload.php";
    public FRONTEND_URL: string = "http://34.124.199.49:8000/api/v1/ProxyApi/SelectAllByWhat";
    public BACKEND_URL: string = "http://34.124.199.49:8000/api/v1/ProxyApi/SelectAllByWhat";
    public SERVER_CV_URL: string = "https://rolievn.com/api/Controller/UploadCV.php";
    public SENTMAIL_URL: string = "https://rolievn.com/api/Controller/BaseMail.php";
    public DOWNCV_URL: string = "https://rolievn.com/api/Controller/ConvertHtmlToPDF.php";
    public IMAGE_UPLOAD_URL = 'http://34.124.199.49:8000/api/upload';

    // authorize
    public staffSubject: BehaviorSubject<any>;
    public currentStaff: Observable<any>;
    public currentCompany: Observable<any>;
    public currentCandidate: Observable<any>;

    constructor(public httpClient: HttpClient,
        private toastrService: ToastrService,
        private router: Router,
        @Inject(PLATFORM_ID) private platform: Object,
        private spinner: NgxSpinnerService
    ) {
        this.staffSubject = new BehaviorSubject<any>(JSON.parse(localStorage.getItem('staffSubject')));

        this.currentStaff = this.staffSubject.asObservable();
        this.url = this.BACKEND_URL;
    }

    public stripHtml(html) {
        let tmp = document.createElement("DIV");
        tmp.innerHTML = html;
        return tmp.textContent || tmp.innerText || "";
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
                    this.httpClient.post<any>(this.IMAGE_UPLOAD_URL, formData, {}).subscribe(data => { });

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
     * 
     * @param param 
     * @param what 
     */
    public excuteAllByWhat(param: any, what: string, frontend?: boolean): Observable<any> {
        this.spinner.show();
        const httpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
            })
        };

        // set param
        param.what = what;

        // set url
        this.getAllWhatUrl = this.url;

        if (frontend) {
            this.getAllWhatUrl = this.FRONTEND_URL;
        }
        return this.httpClient.post<any>(this.getAllWhatUrl, JSON.stringify(param), httpOptions)
            .pipe(map((response: Response) => {
                this.spinner.hide();
                return response;
            }));

    }

    /**
     * get Staff Value
     */
    public get getStaffValue(): any {
        if (this.staffSubject.value == null) {
            this.staffSubject = new BehaviorSubject<any>(JSON.parse(localStorage.getItem('staffSubject')));
        }
        return this.staffSubject.value;
    }

    /**
     * logout Staff
     */
    public logoutStaff() {
        localStorage.removeItem('staffSubject');
        this.staffSubject = new BehaviorSubject<any>(JSON.parse(localStorage.getItem('staffSubject')));
        this.router.navigate(['/']);
        setTimeout(() => {
            window.location.reload();
        }, 1500);
    }

    /**
     * remoteAllLoginAccount
     */
    public remoteAllLoginAccount() {
        localStorage.removeItem('staffSubject');
        this.staffSubject = new BehaviorSubject<any>(JSON.parse(localStorage.getItem('staffSubject')));
    }

    /**
     * upload image
     * @param formData 
     */
    public upload(formData) {
        return this.httpClient.post<any>(this.SERVER_URL, formData, {
            reportProgress: true,
            observe: 'events'
        });
    }

    /**
     * upload Curriculum Vitae
     * @param formData 
     */
    public uploadCV(formData) {
        return this.httpClient.post<any>(this.SERVER_CV_URL, formData, {
            reportProgress: true,
            observe: 'events'
        });
    }

    /**
     * Sent mail
     * @param from 
     * @param to 
     * @param subject 
     * @param message 
     * @param typemail 
     */
    public sentMail(from: any, to: any, subject: any, message: any, typemail: any): Observable<any> {
        this.spinner.show();
        const httpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json'
            })
        };
        const param = {
            'from': from,
            'to': to,
            'subject': subject,
            'message': message,
            'typemail': typemail,
        };

        // ecode param 
        // let encodeParam = btoa(JSON.stringify(param));
        // encodeParam = btoa(encodeParam);
        // encodeParam = btoa(encodeParam);
        // encodeParam = btoa(encodeParam);
        // encodeParam = btoa(encodeParam);

        return this.httpClient.post<any>(this.SENTMAIL_URL, JSON.stringify(param), httpOptions)
            .pipe(map((response: Response) => {
                this.spinner.hide();
                return response;
            }));
    }

    /**
     * convert To Data
     * @param data 
     */
    public convertToData(data): any[] {
        data = JSON.parse(data + '');
        let result: any[] = [];
        data.forEach(item => {
            item.fields.id = item.pk;
            result.push(item.fields);
        });
        return result;
    }

    /**
     * 
     * @param date 
     */
    public formatDate2(date: Date): string {
        const month = date.getMonth() + 1;
        const day = date.getDate();
        return date.getFullYear() + '-'
            + (month > 9 ? month : ('0' + month)) + '-'
            + (day > 9 ? day : ('0' + day));
    }

    /**
    *
    * @param date
    */
    public formatDate(date: Date): string {
        const month = date.getMonth() + 1;
        const day = date.getDate();
        return (
            date.getFullYear() +
            "-" +
            (date.getMonth() + 1 > 9
                ? date.getMonth() + 1
                : "0" + (date.getMonth() + 1)) +
            "-" +
            (date.getDate() > 9
                ? date.getDate()
                : "0" + date.getDate())
            + '\x20' +
            (date.getHours() > 9
                ? date.getHours()
                : "0" + date.getHours()) +
            ":" +
            (date.getMinutes() > 9
                ? date.getMinutes()
                : "0" + date.getMinutes()) +
            ":" +
            (date.getSeconds() > 9
                ? date.getSeconds()
                : "0" + date.getSeconds())
        )
    }

    /**
     * 
     * @param date 
     */
    public formatDateTime(date: Date): string {
        return (
            date.getFullYear() +
            "-" +
            (date.getMonth() + 1 > 9
                ? date.getMonth() + 1
                : "0" + (date.getMonth() + 1)) +
            "-" +
            (date.getDate() > 9
                ? date.getDate()
                : "0" + date.getDate())
            + '\x20' +
            (date.getHours() > 9
                ? date.getHours()
                : "0" + date.getHours()) +
            ":" +
            (date.getMinutes() > 9
                ? date.getMinutes()
                : "0" + date.getMinutes()) +
            ":" +
            (date.getSeconds() > 9
                ? date.getSeconds()
                : "0" + date.getSeconds())
        )
    }

    /**
   *
   * @param date
   */
    public formatDateDOTNET(date: Date): string {
        let now = new Date();
        return (
            date.getFullYear() +
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
    * formatDateToDDMMYYYY
    * @param date 
    */
    public formatDateToDDMMYYYY(date = new Date): string {
        const month = date.getMonth() + 1;
        const day = date.getDate();
        return (day > 9 ? day : ('0' + day)) + '-'
            + (month > 9 ? month : ('0' + month)) + '-'
            + date.getFullYear();
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
     * 
     * @param param 
     * @param what 
     */
    public excuteAllByWhatWithUrl(url: string, param: any, what: string): Observable<any> {
        const httpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
                'Authorization': 'my-auth-token',
                'Access-Control-Allow-Origin': '*',
            })
        };
        this.getAllWhatUrl = this.url + what;
        console.log('Param input', JSON.stringify(param));
        return this.httpClient.post<any>(url, JSON.stringify(param), httpOptions)
            .pipe(map((response: Response) => response.json()));
    }

    public showError(mess: string) {
        this.toastrService.error('Pinks Ways!', mess, {
            timeOut: 1500,
            progressBar: true
        });
    }

    public showSuccess(mess: string) {
        this.toastrService.success('Pinks Ways!', mess + '!', {
            timeOut: 1500,
            progressBar: true
        });
    }

    public showWarning(mess: string) {
        this.toastrService.warning('Pinks Ways!', mess + '!', {
            timeOut: 1500,
            progressBar: true
        });
    }

    /**
      * bỏ dấu tiếng việt để search
    */
    public cleanAccents(str: string): string {
        str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
        str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
        str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
        str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
        str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
        str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
        str = str.replace(/đ/g, "d");
        str = str.replace(/À|Á|Ạ|Ả|Ã|Â|Ầ|Ấ|Ậ|Ẩ|Ẫ|Ă|Ằ|Ắ|Ặ|Ẳ|Ẵ/g, "A");
        str = str.replace(/È|É|Ẹ|Ẻ|Ẽ|Ê|Ề|Ế|Ệ|Ể|Ễ/g, "E");
        str = str.replace(/Ì|Í|Ị|Ỉ|Ĩ/g, "I");
        str = str.replace(/Ò|Ó|Ọ|Ỏ|Õ|Ô|Ồ|Ố|Ộ|Ổ|Ỗ|Ơ|Ờ|Ớ|Ợ|Ở|Ỡ/g, "O");
        str = str.replace(/Ù|Ú|Ụ|Ủ|Ũ|Ư|Ừ|Ứ|Ự|Ử|Ữ/g, "U");
        str = str.replace(/Ỳ|Ý|Ỵ|Ỷ|Ỹ/g, "Y");
        str = str.replace(/Đ/g, "D");
        // Combining Diacritical Marks
        str = str.replace(/\u0300|\u0301|\u0303|\u0309|\u0323/g, ""); // huyền, sắc, hỏi, ngã, nặng 
        str = str.replace(/\u02C6|\u0306|\u031B/g, ""); // mũ â (ê), mũ ă, mũ ơ (ư)
        return str;
    }

    public getUrl() {
        return window.location.href;
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