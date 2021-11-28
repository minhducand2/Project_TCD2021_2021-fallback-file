import { Component, OnInit, OnDestroy } from '@angular/core';
import { Observable, Observer, Subscription } from 'rxjs';
import { ApiService } from 'src/app/common/api-service/api.service';
import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
import { Label, Color } from 'ng2-charts';
import * as pluginDataLabels from 'chartjs-plugin-datalabels';
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit, OnDestroy {

  //subscription
  subscription: Subscription[] = [];

  //dashboard
  public barChartOptions: ChartOptions = {
    responsive: true,
    legend: {
      display: false
    },
    // We use these empty structures as placeholders for dynamic theming.
    scales: { xAxes: [{}], yAxes: [{}] },
    plugins: {
      datalabels: {
        anchor: 'end',
        align: 'end',
      }
    }
  };
  public barChartLabels: Label[] = ['NewOder', 'Processing', 'Shipping', 'Completed', 'Cancelled', 'Total'];
  public barChartType: ChartType = 'bar';
  public barChartLegend = true;
  public barChartPlugins = [pluginDataLabels];
  public barChartData: ChartDataSets[] = [
    {
      data: [],
    },
  ];
  public chartColors: any[] = [
    {
      backgroundColor: ["#1266F1", "#B23CFD", "#00B74A", "#FFA900", "#39C0ED", "#F93154"]
    }];

  //data fillter
  now = new Date();
  startDate: any;
  endDate: any;
  maxDate = new Date();
  update = new Date();

  //init total 
  userTotal = 0;
  totalProduct = 0;
  totalRevenue = 0;
  totalCost = 0;
  totalIncome = 0;
  totalIncomeDay = 0;

  //Top Blogs
  Blogs: any[] = [];
  Shops: any[] = [];
  BlogCategoriesDatas: any[] = [];
  totalBlogs = 0;
  totalShops = 0;


  /**
   * constructor
   */
  constructor(private api: ApiService) { }

  /**
   * ngOnInit
   */
  ngOnInit() {
    // on Close Sidebar Mobile
    this.onCloseSidebarMobile();   
    this.endDate = this.api.formatDate(this.now);
    this.now.setDate(this.now.getDate() - 30);
    this.startDate = this.api.formatDate(this.now);
    this.onFillterClick();

    //scroll top mobile
    window.scroll({ left: 0, top: 0, behavior: 'smooth' });
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
   * onFillterClick
   */
  onFillterClick() {  
    this.barChartData[0]['data'] = [];
    this.maxDate = new Date();
    this.getOnLoadCustomers();
  }
 

  /**
   * getOnLoadCustomers
   */
  getOnLoadCustomers() {
    const param = {
      'startDate': this.startDate == '' ? '0' : this.api.formatDateDDMMYY(new Date(this.startDate)),
      'endDate': this.endDate == '' ? '0' : this.api.formatDateDDMMYY(new Date(this.endDate))

    }
    this.api.excuteAllByWhat(param, '2007').subscribe((data) => { 
      // data = data['result']; 
      if (data.length > 0) {
        this.userTotal = data[0]['UserPeople'];
      } else {
        this.userTotal = 0;
      }
      this.getOnLoadProduct();
    })
  }

  /**
   * getOnLoadCustomers
   */
  getOnLoadProduct() {
    const param = {
      'startDate': this.startDate == '' ? '0' : this.api.formatDateDDMMYY(new Date(this.startDate)),
      'endDate': this.endDate == '' ? '0' : this.api.formatDateDDMMYY(new Date(this.endDate))

    }
    this.api.excuteAllByWhat(param, '2907').subscribe((data) => {
      // data = data['result']; 
      if (data.length > 0) {
        if (data[0]['Amount'] != null) {
          this.totalProduct = data[0]['Amount'];
        } else {
          this.totalProduct = 0;
        }

      } else {
        this.totalProduct = 0;
      }
      this.getOnLoadTotalMoney();
    })
  }


  /**
   * getOnLoadCustomers
   */
  getOnLoadTotalMoney() {
    const param = {
      'startDate': this.startDate == '' ? '0' : this.api.formatDateDDMMYY(new Date(this.startDate)),
      'endDate': this.endDate == '' ? '0' : this.api.formatDateDDMMYY(new Date(this.endDate))

    }
    this.api.excuteAllByWhat(param, '2908').subscribe((data) => { 
      // data = data['result']; 
      if (data) {
        this.totalRevenue = data.totalRevenueAll;
        this.totalCost = data.totalCostAll;
        this.totalIncome = this.totalRevenue - this.totalCost;
      } else {
        this.totalRevenue = 0;
        this.totalCost = 0;
        this.totalIncome = 0;
      }

      this.getOnLoadTotalRevenueDay();

    })
  }

  /**
   * getOnLoadTotalRevenueDay
   */
  getOnLoadTotalRevenueDay() {
    const param = {
      'nowDay': this.api.formatDateDDMMYY(new Date(this.maxDate)),

    }
    this.api.excuteAllByWhat(param, '2909').subscribe((data) => {

      // data = data['result']; 

      if (data) {
        this.totalIncomeDay = data;
      } else {
        this.totalIncomeDay = 0;
      }
      this.getOnLoadOrderStatus1();

    })
  }

  /**
  * getOnLoadOrderStatus1
  */
  getOnLoadOrderStatus1() {
    const param = {
      'startDate': this.startDate == '' ? '0' : this.api.formatDateDDMMYY(new Date(this.startDate)),
      'endDate': this.endDate == '' ? '0' : this.api.formatDateDDMMYY(new Date(this.endDate))

    }
    this.api.excuteAllByWhat(param, '2807').subscribe((data) => { 
      // data = data['result']; 
      if (data.length > 0) {
        this.barChartData[0]['data'].push(Number(data[0]['Amount']));
      }
      this.getOnLoadOrderStatus2();
    })
  }

  /**
  * getOnLoadOrderStatus2
  */
  getOnLoadOrderStatus2() {
    const param = {
      'startDate': this.startDate == '' ? '0' : this.api.formatDateDDMMYY(new Date(this.startDate)),
      'endDate': this.endDate == '' ? '0' : this.api.formatDateDDMMYY(new Date(this.endDate))

    }
    this.api.excuteAllByWhat(param, '2808').subscribe((data) => {
      // data = data['result']; 
      if (data.length > 0) {
        this.barChartData[0]['data'].push(Number(data[0]['Amount']));
      }
      this.getOnLoadOrderStatus3();
    })
  }

  /**
 * getOnLoadOrderStatus3
 */
  getOnLoadOrderStatus3() {
    const param = {
      'startDate': this.startDate == '' ? '0' : this.api.formatDateDDMMYY(new Date(this.startDate)),
      'endDate': this.endDate == '' ? '0' : this.api.formatDateDDMMYY(new Date(this.endDate))

    }
    this.api.excuteAllByWhat(param, '2809').subscribe((data) => {
      // data = data['result']; 
      if (data.length > 0) {
        this.barChartData[0]['data'].push(Number(data[0]['Amount']));
      }
      this.getOnLoadOrderStatus4();
    })
  }

  /**
* getOnLoadOrderStatus4
*/
  getOnLoadOrderStatus4() {
    const param = {
      'startDate': this.startDate == '' ? '0' : this.api.formatDateDDMMYY(new Date(this.startDate)),
      'endDate': this.endDate == '' ? '0' : this.api.formatDateDDMMYY(new Date(this.endDate))

    }
    this.api.excuteAllByWhat(param, '2810').subscribe((data) => {
      // data = data['result']; 
      if (data.length > 0) {
        this.barChartData[0]['data'].push(Number(data[0]['Amount']));
      }
      this.getOnLoadOrderStatus5();
    })
  }

  /**
* getOnLoadOrderStatus5
*/
  getOnLoadOrderStatus5() {
    const param = {
      'startDate': this.startDate == '' ? '0' : this.api.formatDateDDMMYY(new Date(this.startDate)),
      'endDate': this.endDate == '' ? '0' : this.api.formatDateDDMMYY(new Date(this.endDate))

    }
    this.api.excuteAllByWhat(param, '2811').subscribe((data) => {
      // data = data['result']; 
      if (data.length > 0) {
        this.barChartData[0]['data'].push(Number(data[0]['Amount']));
      }
      this.getOnLoadOrderStatusAll();
    })
  }

  /**
* getOnLoadOrderStatusAll
*/
  getOnLoadOrderStatusAll() {
    const param = {
      'startDate': this.startDate == '' ? '0' : this.api.formatDateDDMMYY(new Date(this.startDate)),
      'endDate': this.endDate == '' ? '0' : this.api.formatDateDDMMYY(new Date(this.endDate))

    }
    this.api.excuteAllByWhat(param, '2812').subscribe((data) => {
      // data = data['result']; 
      if (data.length > 0) {
        this.barChartData[0]['data'].push(Number(data[0]['Amount']));
      }
      this.getListBlogcategories()
    })
  }

  /**                                                            
   * get list BlogCategories
   */
  getListBlogcategories() {
    this.subscription.push(this.api.excuteAllByWhat({}, '1300')
      .subscribe((data) => {
        // data = data['result']; 
        this.BlogCategoriesDatas = data;
        this.getOnLoadTopBlogs();
      })
    );
  }

  /**                                                            
   * get name display BlogCategories by id     
   */
  getDisplayBlogcategoriesById(id) {
    return this.BlogCategoriesDatas.filter((e) => e.id == id)[0]?.Name;
  }

  /**
   * getOnLoadTopBlogs
   */
  getOnLoadTopBlogs() {
    this.api.excuteAllByWhat({}, '1407').subscribe((data) => {
      // data = data['result']; 
      if (data.length > 0) {
        let stt = 1;
        data.forEach(element => {
          element.stt = stt++;
        });
        this.Blogs = data;
      } else {
        this.Blogs = []
      }

      this.getCountTotalBlog();

    })
  }

  /**                                                            
   * get list getCountTotalBlog
   */
  getCountTotalBlog() {
    this.subscription.push(this.api.excuteAllByWhat({}, '1406')
      .subscribe((data) => {
        // data = data['result']; 
        if (data.length > 0) {
          this.totalBlogs = data[0]['CountPage'];
        } else {
          this.totalBlogs = 0;
        }
        this.getTopShop();
      })
    );
  }

  /**                                                            
  * get list getTopShop
  */
  getTopShop() {
    this.subscription.push(this.api.excuteAllByWhat({}, '707')
      .subscribe((data) => { 
        // data = data['result']; 
        if (data.length > 0) {
          let stt = 1;
          data.forEach(element => {
            element.stt = stt++;
          });
          this.Shops = data;
        } else {
          this.Shops = [];

        }
        this.getCountTotalShop();
      })
    );
  }

  /**                                                            
   * get list getCountTotalShop
   */
  getCountTotalShop() {
    this.subscription.push(this.api.excuteAllByWhat({}, '706')
      .subscribe((data) => {
        // data = data['result']; 
        if (data.length > 0) {
          this.totalShops = data[0]['CountPage'];
        } else {
          this.totalShops = 0;
        }
      })
    );
  }




  /**
   * on Close Sidebar Mobile
   */
  onCloseSidebarMobile() {
    $(document).ready(function () {
      $(".row-offcanvas-right").removeClass("active");
    });
  }

}
