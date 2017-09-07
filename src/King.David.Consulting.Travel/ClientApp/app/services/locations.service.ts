import { Injectable, Inject } from '@angular/core';
import { URLSearchParams } from "@angular/http";
import { Observable } from 'rxjs/Rx';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';

import { ApiService } from './api.service';
import { City, State, LocationListConfig } from "../models";
@Injectable()
export class LocationsService {

    constructor(private apiService: ApiService) { }
    //cities
    //state/{state}/cities
    //cities/{user}
    queryCity(config: LocationListConfig): Observable<{ cities: City[], citiesTotalCount: number }> {
        const params: URLSearchParams = new URLSearchParams();

        Object.keys(config.filters)
            .forEach((key) => {
                params.set(key, config.filters[key]);
            });
        
        let uri = '';

        if (config.state == 'all')
            uri = '/cities';
        else if (config.username)
            uri = `/cities/${config.username}`;
        else
            uri = `/state/${config.state}/cities`;

        return this.apiService
            .get(uri, params).map(data => data);

    }

    
    //states
    //user/{user}/visits/states
    queryState(config: LocationListConfig): Observable<{ states: State[], statesCount: number }> {
        const params: URLSearchParams = new URLSearchParams();

        Object.keys(config.filters)
            .forEach((key) => {
                params.set(key, config.filters[key]);
            });
        
        let uri = '';

        if (config.state == 'all')
            uri = '/state';
        else (config.username)
            uri = `/user/${config.username}/visits/states`;
   
        return this.apiService
            .get(uri, params).map(data => data);

    }
}