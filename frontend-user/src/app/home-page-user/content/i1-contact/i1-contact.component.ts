import { Component, OnInit, OnDestroy } from '@angular/core';
import { ApiService } from 'src/app/common/api-service/api.service';
import { Subscription } from 'rxjs';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { SEOService } from 'src/app/common/api-service/seo.service';
import { DomSanitizer } from '@angular/platform-browser';
@Component({
  selector: 'app-i1-contact',
  templateUrl: './i1-contact.component.html',
  styleUrls: ['./i1-contact.component.scss']
})
export class I1ContactComponent implements OnInit, OnDestroy {
  subscription: Subscription[] = [];
  contactInfo = {
    Working: '',
    Address: '',
    Phone: '',
    Mail: '',
    Tel: '',
    ToMail: '',
    Facebook: '',
    Twitter: '',
    Instagram: '',
    Youtube: '',
    Map: '',
  };
  Name: string;
  Email: string;
  Message: string;

  Map: any;

  // url Current
  urlCurrent: string = '';

  // validate
  form: FormGroup;
  constructor(private api: ApiService, private formBuilder: FormBuilder, private seoService: SEOService, private sanitizer: DomSanitizer,) {
    this.form = this.formBuilder.group({
      fullName: [null, [Validators.required, Validators.minLength(5), Validators.maxLength(100)]],
      Email: [null, [Validators.required, Validators.minLength(5), Validators.maxLength(100), Validators.pattern('[A-Za-z0-9._%-]+@[A-Za-z0-9._%-]+\\.[a-z]{2,3}')]],
      Message: [null, [Validators.required]],
    })
  }

  /**
   * ngOnInit
   */
  ngOnInit(): void {
    this.api.scrollToTop();
    this.getDataContact();
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
  * get data Blog
  */
  getDataContact() {
    const param = {
    };
    this.subscription.push(this.api.excuteAllByWhat(param, '1500').subscribe(result => {

      if (result.length > 0) {

        result[0].Tel = 'tel:' + result[0].Phone;
        result[0].ToMail = 'mailto:' + result[0].Mail;
        this.Map = this.sanitizer.bypassSecurityTrustHtml(result[0].Map);
        this.contactInfo = result[0];

        //set SEO
        let title = 'Contact';
        this.seoService.setTitle(title);
      }
    }));
  }

  /**
   * onClickInsertContact
   */
  onClickInsertContact() {
    const param = {
      "Name": this.Name,
      "Email": this.Email,
      "Message": this.Message,
      "IdContactStatus": 1,
    };
    this.subscription.push(this.api.excuteAllByWhat(param, '1701').subscribe(result => {
      if (result) {
        this.api.showSuccess("Gửi lời nhắn thành công");
      } else {
        this.api.showWarning('Gửi lời nhắn chưa được');
      }
    }));
  }

}
