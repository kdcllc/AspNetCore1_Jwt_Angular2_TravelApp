import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { UserService } from '../services/index';
import { VisitListConfig } from "../models";

@Component({
    selector: 'home',
    templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit {

    constructor(
        private router: Router,
        private userService: UserService
    ) { }

    isAuthenticated: boolean;
    listConfig: VisitListConfig = new VisitListConfig();

    ngOnInit() {
        this.userService.isAuthenticated.subscribe(
            (authenticated) => {
                this.isAuthenticated = authenticated;
                if (authenticated) {
                    this.setListTo('user');
                } else {
                    this.setListTo('all');
                }
            }
        )
    }
    setListTo(type: string = '', filters: Object = {}) {
        // If user visits are requested but user is not authenticated, redirect to login
        if (type === 'user' && !this.isAuthenticated) {
            this.router.navigateByUrl('/login');
            return;  
        }
        
        this.listConfig = { type: type, filters: filters };
        if (type === 'user' && this.isAuthenticated){
            let user = this.userService.getCurrentUser();
            //this.listConfig.type = type;
            this.listConfig.filters.username = user.username;
        }                 
    }
}
