import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { User, City, LocationListConfig } from "../models";
import { LocationsService } from "../services";

@Component({
    selector: 'locations-cities',
    templateUrl: './cities.component.html'
})
export class CitiesComponent implements OnInit {

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private locationService: LocationsService) {
            this.limit = 10;
            this.currentPage = 1;
         }

    limit: number =10;
    user: User;
    cities: City[];
    query: LocationListConfig = new LocationListConfig();
    results: City[];
    loading = false;
    currentPage = 1;
    totalPages: Array<number> = [1];

    ngOnInit() {
       
        this.runQuery();
    }

    setPageTo(pageNumber) {
        this.currentPage = pageNumber;
        this.runQuery();
    }

    runQuery() {
        this.loading = true;
        this.results = [];

        // Create limit and offset filter (if necessary)
        if (this.limit) {
            this.query.filters.limit = this.limit;
            this.query.filters.offset = (this.limit * (this.currentPage - 1))
        }

        this.locationService.queryCity(this.query)
            .subscribe(data => {
                this.loading = false;
                this.results = data.cities;

                // Used from http://www.jstips.co/en/create-range-0...n-easily-using-one-line/
                this.totalPages = Array.from(new Array(Math.ceil(data.citiesTotalCount / this.limit)), (val, index) => index + 1);
            });
    }
}
