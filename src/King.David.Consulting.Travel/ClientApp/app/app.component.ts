import { Component, OnInit } from '@angular/core';

import { UserService } from './services/index';

@Component({
    selector: 'app',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

    constructor(
        private userService: UserService
    ) { }

    //runs only once on start of the application load jwt token an call api for user info
    ngOnInit() {
        this.userService.populate();
    }
}
