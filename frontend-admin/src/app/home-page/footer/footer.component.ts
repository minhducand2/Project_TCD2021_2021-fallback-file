import { Component, OnInit } from '@angular/core';
import {Location} from '@angular/common';


@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent implements OnInit {

  private listTitles: any[];
  location: Location;
  mobile_menu_visible: any = 0;
  private toggleButton: any;
  private sidebarVisible: boolean;

  /**
   * constructor
   * @param location
   */
  constructor(location: Location) {
    this.location = location;
        this.sidebarVisible = false;
  }

  /**
   * ngOnInit
   */
  ngOnInit(){
  }

  /**
   * side bar open
   */
  sidebarOpen() {
      const toggleButton = this.toggleButton;
      const body = document.getElementsByTagName('body')[0];
      setTimeout(function(){
          toggleButton.classList.add('toggled');
      }, 500);

      body.classList.add('nav-open');

      this.sidebarVisible = true;
  };

  /**
   * side bar Close
   */
  sidebarClose() {
      const body = document.getElementsByTagName('body')[0];
      this.toggleButton.classList.remove('toggled');
      this.sidebarVisible = false;
      body.classList.remove('nav-open');
  };

  /**
   * sidebar toggle
   */
  sidebarToggle() {
      // const toggleButton = this.toggleButton;
      // const body = document.getElementsByTagName('body')[0];
      var $toggle = document.getElementsByClassName('navbar-toggler')[0];

      if (this.sidebarVisible === false) {
          this.sidebarOpen();
      } else {
          this.sidebarClose();
      }
      const body = document.getElementsByTagName('body')[0];

      if (this.mobile_menu_visible == 1) {
          // $('html').removeClass('nav-open');
          body.classList.remove('nav-open');
          if ($layer) {
              $layer.remove();
          }
          setTimeout(function() {
              $toggle.classList.remove('toggled');
          }, 400);

          this.mobile_menu_visible = 0;
      } else {
          setTimeout(function() {
              $toggle.classList.add('toggled');
          }, 430);

          var $layer = document.createElement('div');
          $layer.setAttribute('class', 'close-layer');


          if (body.querySelectorAll('.main-panel')) {
              document.getElementsByClassName('main-panel')[0].appendChild($layer);
          }else if (body.classList.contains('off-canvas-sidebar')) {
              document.getElementsByClassName('wrapper-full-page')[0].appendChild($layer);
          }

          setTimeout(function() {
              $layer.classList.add('visible');
          }, 100);

          $layer.onclick = function() { //asign a function
            body.classList.remove('nav-open');
            this.mobile_menu_visible = 0;
            $layer.classList.remove('visible');
            setTimeout(function() {
                $layer.remove();
                $toggle.classList.remove('toggled');
            }, 400);
          }.bind(this);

          body.classList.add('nav-open');
          this.mobile_menu_visible = 1;

      }
  };

  /**
   * get titlLe
   */
  getTitle(){
    var titlee = this.location.prepareExternalUrl(this.location.path());
    if(titlee.charAt(0) === '#'){
        titlee = titlee.slice( 1 );
    }

    for(var item = 0; item < this.listTitles.length; item++){
        if(this.listTitles[item].path === titlee){
            return this.listTitles[item].title;
        }
    }
    return 'Dashboard';
  }

}
