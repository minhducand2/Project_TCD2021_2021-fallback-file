import {
  Component,
  OnInit,
  OnDestroy,
  ViewChild,
  ElementRef,
} from '@angular/core';
import {
  Router,
  Event,
  NavigationStart,
  NavigationEnd,
  NavigationError,
  RouterOutlet,
} from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { Subscription } from 'rxjs';
import { ApiService } from 'src/app/common/api-service/api.service';
import {
  transition,
  trigger,
  query,
  style,
  animate,
  group,
  animateChild,
  keyframes
} from '@angular/animations';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  animations: [
    trigger('routeAnimation', [
      transition('1 => 2', [
        style({ height: '!' }),
        query(':enter', style({ transform: 'translateX(100%)' })),
        query(':enter, :leave', style({ position: 'absolute', top: 0, left: 0, right: 0 })),
        // animate the leave page away
        group([
          query(':leave', [
            animate('0.3s cubic-bezier(.35,0,.25,1)', style({ transform: 'translateX(-100%)' })),
          ]),
          // and now reveal the enter
          query(':enter', animate('0.3s cubic-bezier(.35,0,.25,1)', style({ transform: 'translateX(0)' }))),
        ]),
      ]),
      transition('2 => 1', [
        style({ height: '!' }),
        query(':enter', style({ transform: 'translateX(-100%)' })),
        query(':enter, :leave', style({ position: 'absolute', top: 0, left: 0, right: 0 })),
        // animate the leave page away
        group([
          query(':leave', [
            animate('0.3s cubic-bezier(.35,0,.25,1)', style({ transform: 'translateX(100%)' })),
          ]),
          // and now reveal the enter
          query(':enter', animate('0.3s cubic-bezier(.35,0,.25,1)', style({ transform: 'translateX(0)' }))),
        ]),
      ]),
    ])
  ]
})
export class AppComponent implements OnInit, OnDestroy {
  subscription: Subscription[] = [];
  ads: any = [];
  isShow = true;
  isNone = false;
  title = 'Rolie Viet Nam';
  getUrl: any;
  @ViewChild('openModal') openModal: ElementRef;
  isCheckADS: boolean = false;
  defaultImage: any = "assets/images/loading.gif";

  getDepth(outlet) {
    return outlet.activatedRouteData['depth'];
  }

  /**
   * constructor
   * @param api
   */
  constructor(
    public api: ApiService,
    private router: Router,
    private spinner: NgxSpinnerService
  ) {
    // check url change
    this.router.events.subscribe((event: Event) => {
      if (event instanceof NavigationStart) {
        // Show loading indicator
        this.spinner.show();
      }

      if (event instanceof NavigationEnd) {
        // Hide loading indicator
        document
          .getElementById('navbarSupportedContent')
          ?.classList.remove('show');

        /** spinner ends after 1 seconds */
        setTimeout(() => {
          this.spinner.hide();
        }, 500);
      }

      if (event instanceof NavigationError) {
        // Hide loading indicator
        // Present error to user
        console.log(event.error);

        /** spinner ends after 1 seconds */
        setTimeout(() => {
          this.spinner.hide();
        }, 500);
      }
    });
  }

  /**
   * ngOnInit
   */
  ngOnInit() {
    // setTimeout(() => {
    //   this.getAds();
    // }, 2000);
  }

  /**
   *
   */
  ngAfterViewInit() {
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
   * getAds
   */
  getAds() {
    const param = {};
    this.subscription.push(
      this.api.excuteAllByWhat(param, '5500').subscribe((result) => {
        let data = result.data;
        if (result.status == true && data.length > 0) {
          this.ads = data[0];
          this.isCheckADS = true;
        } else {
          this.ads = [];
          this.isCheckADS = false;
        }
        if (this.router.url == '/' && this.isCheckADS) {
          this.openModal?.nativeElement.click();
        }
      })
    );
  }

  /**
   * onAdsClick
   * @param link
   */
  onAdsClick(link) {
    const url = '/' + link;
    this.router.navigate([url.toLowerCase()]);

  }

  /***
   * closeModal
   */
  closeModal() {
    document.getElementById('exampleModalCenter').style.display =
      'none!important';
  }

  navigatePage(item): void {
    this.router.navigate([item]);
  }
}
