import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { Router } from '@angular/router';
import { ApiService } from '../common/api-service/api.service';
import { LoginCookie } from '../common/core/login-cookie';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { Md5 } from 'ts-md5';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit, OnDestroy {
  //data binding
  input = {
    email: '',
    password: '',
    role: '',
  };

  //subscription
  subscription: Subscription[] = [];

  //md5 password
  passwordMd5: string = '';
  form: FormGroup;

  /**
   * constructor
   * @param router
   * @param api
   * @param login
   * @param formBuilder
   */
  constructor(
    private router: Router,
    private api: ApiService,
    private login: LoginCookie,
    private formBuilder: FormBuilder
  ) {
    //form validation
    this.form = this.formBuilder.group({
      password: [null, [Validators.maxLength(50)]],
      email: [null, [Validators.maxLength(50)]],
    });
  }

  /**
   * ngOnInit
   */
  ngOnInit(): void {
    this.input = {
      email: '',
      password: '',
      role: '',
    };
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
   * on Login Click
   */
  onBtnLoginClick() {
    // password encryption
    this.passwordMd5 = Md5.hashAsciiStr(this.input.password).toString();

    // param
    const param = {
      email: this.input.email,
      password: this.passwordMd5,
    };

    // check login
    this.subscription.push(
      this.api.excuteAllByWhat(param, '0').subscribe((data) => { 
         
        this.api.remoteAllLoginAccount();

        // save token
        localStorage.setItem('token', data['token']);

        // data = data['result']; 
        if (data.length > 0) {
          localStorage.setItem('accountSubject', JSON.stringify(data[0]));
          this.api.accountSubject.next(data[0]);
          this.api.showSuccess('Đăng nhập Thành Công ');

          window.location.href = '/#/manager/home';
        } else {
          this.api.showError('Sai Email hoặc Password ');
        }
      })
    );
  }
}
