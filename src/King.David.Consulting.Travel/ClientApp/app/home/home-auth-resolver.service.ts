import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, Router, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs/Rx';

import { UserService } from '../services/index';

@Injectable()
export class HomeAuthResolver implements Resolve<boolean> {
    constructor(
        private router: Router,
        private useService: UserService
    ) { }

    resolve(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): Observable<boolean> {

        return this.useService.isAuthenticated.take(1);

    }
}