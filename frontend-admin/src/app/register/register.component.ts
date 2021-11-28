import { Component, OnInit, OnDestroy } from '@angular/core';
import { ApiService } from '../common/api-service/api.service';
import { Subscription } from 'rxjs';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Md5 } from 'ts-md5';
import { MustMatch } from 'src/app/common/validations/must-match.validator';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent implements OnInit,OnDestroy {
  //data binding
  input = {
    name: '',
    email: '',
    password: '',
    repassword: '',
    role: '',
    img:'',
  };

  //subscrimption
  subscription: Subscription[] = [];

  //md5
  passwordMd5: string = '';

  //form
  form: FormGroup;

  /**
   * constructor
   * @param api
   * @param router
   * @param formBuilder
   */
  constructor(
    private api: ApiService,
    private router: Router,
    private formBuilder: FormBuilder
  ) {

    //form validation
    this.form = this.formBuilder.group(
      {
        name: [
          null,
          [
            Validators.required,
            Validators.maxLength(100),
            Validators.minLength(5),
          ],
        ],
        password: [
          null,
          [
            Validators.required,
            Validators.maxLength(50),
            Validators.minLength(5),
          ],
        ],
        email: [
          null,
          [
            Validators.required,
            Validators.maxLength(50),
            Validators.pattern(
              '[A-Za-z0-9._%-]+@[A-Za-z0-9._%-]+\\.[a-z]{2,3}'
            ),
          ],
        ],
        repassword: [null, [Validators.required]],
      },

      //confirm password
      {
        validator: MustMatch('password', 'repassword'),
      }
    );
  }

  /**
   * ngOnInit
   */
  ngOnInit(): void {
    this.input = {
      email: '',
      password: '',
      name: '',
      repassword: '',
      img:'https://geracilawfirm.com/wp-content/uploads/2019/12/FILLER-no-headshot.png',//img default
      role: 'a:1,b:1,c:1', //permission default
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
   * on Register Click
   */
  onBtnRegClick() {
    //validation form invalid
    if (this.form.status != 'VALID') {
      this.api.showWarning('Vui lòng điền đầy đủ tất cả thông tin ');
      return;
    }

    //password encryption
    this.passwordMd5 = Md5.hashAsciiStr(this.input.password).toString();

    //param
    const param = {
      name: this.input.name,
      email: this.input.email,
      password: this.passwordMd5,
      img:this.input.img,
      role: this.input.role,
    };

    //check email already exists
    this.subscription.push(
      this.api.excuteAllByWhat(param, '4').subscribe((data) => {
        if (data.length > 0) {
          this.api.showError('Tài khoản đã được đăng ký ');
        } else {
          //insert data invalid
          this.subscription.push(this.api.excuteAllByWhat(param, '1').subscribe((data) => {
              if (data) {
                this.api.showSuccess('Đăng ký thành công ');
                this.router.navigate(['']);
              }
            })
          );
        }
      })
    );
  }
}

