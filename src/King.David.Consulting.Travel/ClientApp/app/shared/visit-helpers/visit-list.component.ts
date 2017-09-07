import { Component, Input } from "@angular/core";

import { Visit, VisitListConfig } from "../../models";
import { VisitsService } from "../../services";

@Component({
    selector: 'visit-list',
    templateUrl: './visit-list.component.html'
})
export class VisitListComponent {

    query: VisitListConfig;
    results: Visit[];
    loading = false;
    currentPage = 1;
    totalPages: Array<number> = [1];

    constructor(
        private visitsService: VisitsService
    ) { }

    //
    @Input() limit: number;
    @Input()
    set config(config: VisitListConfig) {
        if (config) {
            this.query = config;
            this.currentPage = 1;
            this.runQuery();
        }
    }

    setPageTo(pageNumber:number){
        this.currentPage = pageNumber;
        this.runQuery();
    }

    runQuery(){
        this.loading = true;
        this.results = [];

        //create limit and offset filter
        if(this.limit){
            this.query.filters.limit = this.limit;
            this.query.filters.offset = (this.limit *(this.currentPage -1));
        }

        this.visitsService.query(this.query)
        .subscribe(data => {
            this.loading = false;
            this.results = data.visits

            this.totalPages = Array.from(new Array(Math.ceil(data.visitsTotalCount / this.limit)), (val, index) => index + 1);

            console.log(this.totalPages + 'Pages' + data.visitsCount);
        });
    }
}