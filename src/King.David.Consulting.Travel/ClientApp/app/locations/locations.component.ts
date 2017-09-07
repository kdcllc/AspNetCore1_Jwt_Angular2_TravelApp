import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';


import { UserService } from '../services';
import { User } from "../models";

@Component({
  selector: 'locations',
  templateUrl: './locations.component.html',
  styleUrls: ['./locations.component.css']
})
export class LocationsComponent implements OnInit {

  constructor(
    private route: ActivatedRoute,
    private userService: UserService) { }

  currentUser: User;

  ngOnInit() {

    this.userService.currentUser.subscribe(
      (userData: User) => {
        this.currentUser = userData;
      }
    );
  }

}
