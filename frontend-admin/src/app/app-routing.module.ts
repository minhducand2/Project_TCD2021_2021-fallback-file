import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
    {
        path: '',
        loadChildren: () =>
            import('./login/login.module').then((m) => m.LoginModule),
    },
    {
        path: 'register',
        loadChildren: () =>
            import('./register/register.module').then((m) => m.RegisterModule),
    },
    {
        path: 'manager',
        loadChildren: () =>
            import('./home-page/home-page.module').then((m) => m.HomePageModule),
    },
];

@NgModule({
    imports: [RouterModule.forRoot(routes, {
        initialNavigation: 'enabled',
        useHash: true
    })],
    exports: [RouterModule]
})
export class AppRoutingModule { }
