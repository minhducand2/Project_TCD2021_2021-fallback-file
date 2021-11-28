import {
  Component,
  OnInit,
  OnDestroy,
  Inject,
  PLATFORM_ID,
} from '@angular/core';
import { ApiService } from 'src/app/common/api-service/api.service';
import { Subscription } from 'rxjs';
import { isPlatformBrowser } from '@angular/common';
import { SEOService } from 'src/app/common/api-service/seo.service';
@Component({
  selector: 'app-j1-about',
  templateUrl: './j1-about.component.html',
  styleUrls: ['./j1-about.component.scss'],
})
export class J1AboutComponent implements OnInit, OnDestroy {
  about = {
    Banner: 'url(https://localhost:44321/Resources/Images/2021_04_28_14_02_09_pexels-eberhard-grossgasteiger-844297.jpg) no-repeat',
    Title1: '',
    Description1: '',
    Image1: 'url(https://localhost:44321/Resources/Images/2021_04_28_13_59_30_c3358bb87f18cf8bedac5b5f63710a0f.jpg) no-repeat',
    Title2: '',
    Description2: '',
    Image2: 'url(https://localhost:44321/Resources/Images/2021_04_28_14_02_09_pexels-eberhard-grossgasteiger-844297.jpg) no-repeat',
    Title3: '',
    Description3: '',
    Image3: 'url(https://localhost:44321/Resources/Images/2021_04_28_13_59_30_c3358bb87f18cf8bedac5b5f63710a0f.jpg) no-repeat',
    Belive1: '',
    Belive2: '',
    Belive3: '',
  };

  defaultImage: any = "assets/images/loading.gif";

  // url Current
  urlCurrent: string = '';

  subscription: Subscription[] = [];
  constructor(
    private api: ApiService, private seoService: SEOService,
    @Inject(PLATFORM_ID) private platform: Object
  ) { }

  /**
   * ngOnInit
   */
  ngOnInit(): void {
    this.api.scrollToTop();
    // this.getDataCommunityFirst();
    let title = 'About';
    this.seoService.setTitle(title);
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
   * getDataCommunityFirst
   */
  getDataCommunityFirst() {
    const param = {};
    this.subscription.push(
      this.api.excuteAllByWhat(param, '4200').subscribe((result) => {
        let data = result.data;
        if (result.status == true && data.length > 0) {
          data.forEach((element) => {
            element.Banner = 'url(' + element.Banner + ')' + ' ' + 'no-repeat';
            element.Image1 = 'url(' + element.Image1 + ')' + ' ' + 'no-repeat';
            element.Image2 = 'url(' + element.Image2 + ')' + ' ' + 'no-repeat';
            element.Image3 = 'url(' + element.Image3 + ')' + ' ' + 'no-repeat';
          });
          this.about = data[0];

          //Seo title
          let title = 'About';
          this.seoService.setTitle(title);
        }
      })
    );
  }

  /**
   * scroll
   * @param element
   */
  scroll(element) {
    if (isPlatformBrowser(this.platform)) {
      var ele = document.getElementById(element);
      window.scrollTo({
        left: ele.offsetLeft,
        top: ele.offsetTop,
        behavior: 'smooth',
      });
    }
  }
}
