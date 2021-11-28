import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ContentComponent } from './content.component';
import { RouterModule } from '@angular/router';
import { TransferHttpCacheModule } from '@nguniversal/common';
import { ApiService } from '../../common/api-service/api.service';
import { IMyProfileModule } from './c-my-profile/c-my-profile.module';

@NgModule({
  declarations: [ContentComponent],
  imports: [
    TransferHttpCacheModule,
    CommonModule,
    RouterModule.forChild([
      {
        // ng g module home-page/content/sellTicket --module content
        // ng g c home-page/content/sellTicket
        path: '',
        component: ContentComponent,
        children: [
          {
            path: 'my-profile',
            loadChildren: () =>
              import('./c-my-profile/c-my-profile.module').then(
                (m) => m.IMyProfileModule
              ),
          },
          {
            path: 'home',
            loadChildren: () =>
              import('./c0-home/home.module').then((m) => m.HomeModule),
          },
          {
            path: 'c1-account',
            loadChildren: () =>
              import('./c1-account/c1-account.module').then((m) => m.C1AccountModule),
          },
          {
            path: 'c2-menu',
            loadChildren: () =>
              import('./c2-menu/c2-menu.module').then((m) => m.C2MenuModule),
          },
          {
            path: 'c3-role',
            loadChildren: () =>
              import('./c3-role/c3-role.module').then((m) => m.C3RoleModule),
          },
          {
            path: 'c4-role-detail/:id',
            loadChildren: () =>
              import('./c4-role-detail/c4-role-detail.module').then((m) => m.C4RoleDetailModule),
          },

          {                                                               
            path: 'c5-Banner',                                               
            loadChildren: () =>                                           
              import('./c5-Banner/c5-Banner.module').then((m) => m.C5BannerModule), 
          },                                          
          {                                                               
            path: 'c6-Footer',                                               
            loadChildren: () =>                                           
              import('./c6-Footer/c6-Footer.module').then((m) => m.C6FooterModule), 
          },                                          
          {                                                               
            path: 'c7-HeaderInfo',                                               
            loadChildren: () =>                                           
              import('./c7-HeaderInfo/c7-HeaderInfo.module').then((m) => m.C7HeaderinfoModule), 
          },                                          
          {                                                               
            path: 'c8-Shop',                                               
            loadChildren: () =>                                           
              import('./c8-Shop/c8-Shop.module').then((m) => m.C8ShopModule), 
          },                                          
          {                                                               
            path: 'c9-ShopCombo',                                               
            loadChildren: () =>                                           
              import('./c9-ShopCombo/c9-ShopCombo.module').then((m) => m.C9ShopcomboModule), 
          },                                          
          {                                                               
            path: 'c10-ShopComboDetail',                                               
            loadChildren: () =>                                           
              import('./c10-ShopComboDetail/c10-ShopComboDetail.module').then((m) => m.C10ShopcombodetailModule), 
          },                                          
          {                                                               
            path: 'c11-ShopComment',                                               
            loadChildren: () =>                                           
              import('./c11-ShopComment/c11-ShopComment.module').then((m) => m.C11ShopcommentModule), 
          },                                          
          {                                                               
            path: 'c12-ShopCategories',                                               
            loadChildren: () =>                                           
              import('./c12-ShopCategories/c12-ShopCategories.module').then((m) => m.C12ShopcategoriesModule), 
          },                                          
          {                                                               
            path: 'c13-MealPlanType',                                               
            loadChildren: () =>                                           
              import('./c13-MealPlanType/c13-MealPlanType.module').then((m) => m.C13MealplantypeModule), 
          },                                          
          {                                                               
            path: 'c14-BlogCategories',                                               
            loadChildren: () =>                                           
              import('./c14-BlogCategories/c14-BlogCategories.module').then((m) => m.C14BlogcategoriesModule), 
          },                                          
          {                                                               
            path: 'c15-Blog',                                               
            loadChildren: () =>                                           
              import('./c15-Blog/c15-Blog.module').then((m) => m.C15BlogModule), 
          },                                          
          {                                                               
            path: 'c16-ContactInfo',                                               
            loadChildren: () =>                                           
              import('./c16-ContactInfo/c16-ContactInfo.module').then((m) => m.C16ContactinfoModule), 
          },                                          
          {                                                               
            path: 'c17-ContactStatus',                                               
            loadChildren: () =>                                           
              import('./c17-ContactStatus/c17-ContactStatus.module').then((m) => m.C17ContactstatusModule), 
          },                                          
          {                                                               
            path: 'c18-ContactUs',                                               
            loadChildren: () =>                                           
              import('./c18-ContactUs/c18-ContactUs.module').then((m) => m.C18ContactusModule), 
          },                                          
          {                                                               
            path: 'c19-UserStatus',                                               
            loadChildren: () =>                                           
              import('./c19-UserStatus/c19-UserStatus.module').then((m) => m.C19UserstatusModule), 
          },                                          
          {                                                               
            path: 'c20-RoleUser',                                               
            loadChildren: () =>                                           
              import('./c20-RoleUser/c20-RoleUser.module').then((m) => m.C20RoleuserModule), 
          },                                          
          {                                                               
            path: 'c21-User',                                               
            loadChildren: () =>                                           
              import('./c21-User/c21-User.module').then((m) => m.C21UserModule), 
          },                                          
          {                                                               
            path: 'c22-Promotion',                                               
            loadChildren: () =>                                           
              import('./c22-Promotion/c22-Promotion.module').then((m) => m.C22PromotionModule), 
          },                                          
          {                                                               
            path: 'c23-OrderStatus',                                               
            loadChildren: () =>                                           
              import('./c23-OrderStatus/c23-OrderStatus.module').then((m) => m.C23OrderstatusModule), 
          },                                          
          {                                                               
            path: 'c24-PaymentStatus',                                               
            loadChildren: () =>                                           
              import('./c24-PaymentStatus/c24-PaymentStatus.module').then((m) => m.C24PaymentstatusModule), 
          },                                          
          {                                                               
            path: 'c25-PaymentType',                                               
            loadChildren: () =>                                           
              import('./c25-PaymentType/c25-PaymentType.module').then((m) => m.C25PaymenttypeModule), 
          },                                          
          {                                                               
            path: 'c26-City',                                               
            loadChildren: () =>                                           
              import('./c26-City/c26-City.module').then((m) => m.C26CityModule), 
          },                                          
          {                                                               
            path: 'c27-District',                                               
            loadChildren: () =>                                           
              import('./c27-District/c27-District.module').then((m) => m.C27DistrictModule), 
          },                                          
          {                                                               
            path: 'c28-ProductType',                                               
            loadChildren: () =>                                           
              import('./c28-ProductType/c28-ProductType.module').then((m) => m.C28ProducttypeModule), 
          },                                          
          {                                                               
            path: 'c29-OrderShop',                                               
            loadChildren: () =>                                           
              import('./c29-OrderShop/c29-OrderShop.module').then((m) => m.C29OrdershopModule), 
          },                                          
          // {                                                               
          //   path: 'c30-OrderDetail',                                               
          //   loadChildren: () =>                                           
          //     import('./c30-OrderDetail/c30-OrderDetail.module').then((m) => m.C30OrderdetailModule), 
          // }, 
          {                                                               
            path: 'c30-OrderDetail/:id',                                               
            loadChildren: () =>                                           
              import('./c30-OrderDetail/c30-OrderDetail.module').then((m) => m.C30OrderdetailModule), 
          },                                          
          {                                                               
            path: 'c31-CommentStatus',                                               
            loadChildren: () =>                                           
              import('./c31-CommentStatus/c31-CommentStatus.module').then((m) => m.C31CommentstatusModule), 
          },                                          
          {                                                               
            path: 'c32-MyPromotion',                                               
            loadChildren: () =>                                           
              import('./c32-MyPromotion/c32-MyPromotion.module').then((m) => m.C32MypromotionModule), 
          },                                          
          {                                                               
            path: 'c33-InputProduct',                                               
            loadChildren: () =>                                           
              import('./c33-InputProduct/c33-InputProduct.module').then((m) => m.C33InputproductModule), 
          },                                          
          {                                                               
            path: 'c34-Warehouse',                                               
            loadChildren: () =>                                           
              import('./c34-Warehouse/c34-Warehouse.module').then((m) => m.C34WarehouseModule), 
          },                                          


        ],
      },
    ]),
    IMyProfileModule,
  ],
  providers: [ApiService],
  entryComponents: [],
})
export class ContentModule { }
