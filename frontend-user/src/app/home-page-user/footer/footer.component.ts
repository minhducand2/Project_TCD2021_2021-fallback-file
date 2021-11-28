import {
  Component,
  OnInit,
  OnDestroy,
  Inject,
  PLATFORM_ID,
} from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { ApiService } from 'src/app/common/api-service/api.service';
import { isPlatformBrowser, Location } from '@angular/common';
import { DomSanitizer } from '@angular/platform-browser';
import { AfterViewInit } from '@angular/core';
declare var FB: any;

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss'],
})
export class FooterComponent implements OnInit, OnDestroy {
  subscription: Subscription[] = [];

  private listTitles: any[];
  location: Location;
  mobile_menu_visible: any = 0;
  private toggleButton: any;
  private sidebarVisible: boolean;

  // data binding
  dataFooter = {
    AndroidLink: '',
    IosLink: '',
    Content1: '',
    Content2: '',
    Content3: '',
  };

  dataInfoSocial = {
    Address: '',
    Phone: '',
    Mail: '',
    Working: '',
    Facebook: '',
    Instagram: '',
    Youtube: '',
    Twitter: '',
    Map: '',
  };
  newYear: any;

  constructor(
    private api: ApiService,
    location: Location,
    private router: Router,
    private sanitizer: DomSanitizer,
    @Inject(PLATFORM_ID) private platform: Object
  ) {
    this.location = location;
    this.sidebarVisible = false;
  }

  /**
   * ngOnInit
   */
  ngOnInit() {
    this.newYear = new Date().getFullYear();
    setTimeout(() => {
      this.getFooter();
      this.getInfoSocialFooter();
    }, 1500);
  }

  /**
   * ngOnDestroy
   */
  ngOnDestroy() {
    this.subscription.forEach((item) => {
      item.unsubscribe();
    });
  }



  sidebarOpen() {
    if (isPlatformBrowser(this.platform)) {
      const toggleButton = this.toggleButton;
      const body = document.getElementsByTagName('body')[0];
      setTimeout(function () {
        toggleButton?.classList.add('toggled');
      }, 500);

      body?.classList.add('nav-open');

      this.sidebarVisible = true;
    }
  }

  sidebarClose() {
    if (isPlatformBrowser(this.platform)) {
      const body = document.getElementsByTagName('body')[0];
      this.toggleButton?.classList.remove('toggled');
      this.sidebarVisible = false;
      body?.classList.remove('nav-open');
    }
  }

  sidebarToggle() {
    if (isPlatformBrowser(this.platform)) {
      var $toggle = document.getElementsByClassName('navbar-toggler')[0];

      if (this.sidebarVisible === false) {
        this.sidebarOpen();
      } else {
        this.sidebarClose();
      }
      const body = document.getElementsByTagName('body')[0];

      if (this.mobile_menu_visible == 1) {
        body?.classList.remove('nav-open');
        if ($layer) {
          $layer.remove();
        }
        setTimeout(function () {
          $toggle?.classList.remove('toggled');
        }, 400);

        this.mobile_menu_visible = 0;
      } else {
        setTimeout(function () {
          $toggle?.classList.add('toggled');
        }, 430);

        var $layer = document.createElement('div');
        $layer.setAttribute('class', 'close-layer');

        if (body.querySelectorAll('.main-panel')) {
          document.getElementsByClassName('main-panel')[0].appendChild($layer);
        } else if (body?.classList.contains('off-canvas-sidebar')) {
          document
            .getElementsByClassName('wrapper-full-page')[0]
            .appendChild($layer);
        }

        setTimeout(function () {
          $layer?.classList.add('visible');
        }, 100);

        $layer.onclick = function () {
          //asign a function
          body?.classList.remove('nav-open');
          this.mobile_menu_visible = 0;
          $layer?.classList.remove('visible');
          setTimeout(function () {
            $layer.remove();
            $toggle?.classList.remove('toggled');
          }, 400);
        }.bind(this);

        body?.classList.add('nav-open');
        this.mobile_menu_visible = 1;
      }
    }
  }

  getTitle() {
    var titlee = this.location.prepareExternalUrl(this.location.path());
    if (titlee.charAt(0) === '#') {
      titlee = titlee.slice(1);
    }

    for (var item = 0; item < this.listTitles.length; item++) {
      if (this.listTitles[item].path === titlee) {
        return this.listTitles[item].title;
      }
    }
    return 'Dashboard';
  }

  /**
   * getTopRecipe
   */
  getFooter() {
    const param = {};
    this.subscription.push(
      this.api.excuteAllByWhat(param, '500').subscribe((result) => {
        if (result.length > 0) {
          result[0].Content1 = this.sanitizer.bypassSecurityTrustHtml(result[0].Content1);
          result[0].Content2 = this.sanitizer.bypassSecurityTrustHtml(result[0].Content2);
          result[0].Content3 = this.sanitizer.bypassSecurityTrustHtml(result[0].Content3);
          this.dataFooter = result[0];
        }
      })
    );
  }

  /**
  * getTopRecipe
  */
  getInfoSocialFooter() {
    const param = {};
    this.subscription.push(
      this.api.excuteAllByWhat(param, '1500').subscribe((result) => {
        
        if (result.length > 0) {
          this.dataInfoSocial = result[0];
        }
      })
    );
  }
}
