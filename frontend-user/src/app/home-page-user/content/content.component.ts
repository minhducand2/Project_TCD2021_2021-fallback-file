import { Component, OnInit } from '@angular/core';
import {
  transition,
  trigger,
  query,
  style,
  animate,
  group,
  animateChild,
  keyframes,
  useAnimation
} from '@angular/animations';
import { moveFromTop } from "ngx-router-animations";
import { moveFromBottom } from "ngx-router-animations";
@Component({
  selector: 'app-content',
  templateUrl: './content.component.html',
  styleUrls: ['./content.component.scss'],
  animations: [
    trigger('routeAnimation', [
      transition('1 => 2, 2 => 3,1 => 3,2 => 4', [
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
      transition('3 => 2, 2 => 1,3 => 1,4 => 2', [
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
export class ContentComponent implements OnInit {
  getDepth(outlet) {
    return outlet.activatedRouteData['depth'];
  }
  constructor() { }

  ngOnInit() {
  }

}
