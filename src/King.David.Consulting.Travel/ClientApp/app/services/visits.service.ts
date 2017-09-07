import { Injectable } from "@angular/core";
import { URLSearchParams } from "@angular/http";
import { Observable } from "rxjs/Rx";
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';

import { ApiService } from "./api.service";

import { Visit, VisitListConfig } from "../models";

@Injectable()
export class VisitsService {

    constructor(private apiService: ApiService) { }

    query(config: VisitListConfig): Observable<{ visits: Visit[], visitsCount: number, visitsTotalCount: number }> {
        const params: URLSearchParams = new URLSearchParams();
        Object.keys(config.filters)
            .forEach((key) => {
                if (config.filters[key] !== config.filters.username)
                    params.set(key, config.filters[key]);
            });
        return this.apiService
            .get(
            '/visits' + ((config.type === 'user') ? `/user/${config.filters.username}` : ''),
            params
            ).map(data => data);
    }

}