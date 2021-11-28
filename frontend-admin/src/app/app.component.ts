import { Component, OnInit } from '@angular/core';
import {
  Router,
  Event,
  NavigationStart,
  NavigationEnd,
  NavigationError,
} from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  title = 'rolie-admin';

  ckeditorContent: string = '<p>Some html</p>';

  constructor(private router: Router,
    private spinner: NgxSpinnerService) {
    // check url change
    this.router.events.subscribe((event: Event) => {
      if (event instanceof NavigationStart) { 
        // Show loading indicator
        this.spinner.show();
      }

      if (event instanceof NavigationEnd) {
        /** spinner ends after 1 seconds */
        setTimeout(() => {
          this.spinner.hide();
        }, 500)
      }

      if (event instanceof NavigationError) {
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

  }
}
