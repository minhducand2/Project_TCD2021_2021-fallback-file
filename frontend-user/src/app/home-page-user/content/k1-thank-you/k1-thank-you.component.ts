import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/common/api-service/api.service';
import { SEOService } from 'src/app/common/api-service/seo.service';
@Component({
  selector: 'app-k1-thank-you',
  templateUrl: './k1-thank-you.component.html',
  styleUrls: ['./k1-thank-you.component.scss']
})
export class K1ThankYouComponent implements OnInit {

  // url Current
  urlCurrent: string = '';

  constructor(private api: ApiService, private seoService: SEOService) { }

  /**
   * ngOnInit
   */
  ngOnInit(): void {
    this.api.scrollToTop();

    // url Current
    this.urlCurrent = this.api.getUrl();

    let title = '';
    if (this.urlCurrent.indexOf('/en/') == -1) {
      title = 'Cảm ơn';
    } else {
      title = 'Thank You!'
    }

    //Seo title
    this.seoService.setTitle(title);
  }

}
