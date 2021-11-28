import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { ApiService } from '../api-service/api.service';


@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
    constructor(
        private router: Router,
        private api: ApiService
    ) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        // auth for staff
        const currentStaff = this.api.getStaffValue;
        console.log('phuongoicurrentStaff', currentStaff);

        if (currentStaff) {
            // check if route is restricted by role
            if (route.data.roles && route.data.roles.indexOf(currentStaff.role) === -1) {
                // role not authorised so redirect to home page
                this.router.navigate(['/login']);
                return false;
            }

            // authorised so return true
            return true;
        } 

        console.log('cabc', this.router.url);
        

        // not logged in so redirect to login page with the return url
        this.router.navigate(['/']);
        return false;
    }
}