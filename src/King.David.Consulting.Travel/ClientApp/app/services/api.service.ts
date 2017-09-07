import { Injectable, Inject } from '@angular/core';
import { Headers, Http, Response, URLSearchParams } from '@angular/http';
import { Observable } from 'rxjs/Rx';
//import { Observer } from "rxjs/Rx";
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
//import 'rxjs/add/observable/throw'

import { JwtService } from './jwt.service';

@Injectable()
export class ApiService {
    apiUrl: String;

    constructor(
        public http:Http,
        private jwtService: JwtService,
        @Inject('ORIGIN_URL') public originUrl: string
    ){
        this.apiUrl = `${originUrl}`;
    }

    private setHeaders(): Headers {
        const headerConfig = {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        };

        if (this.jwtService.getToken()) {
            headerConfig['Authorization'] = `Bearer ${this.jwtService.getToken()}`;
        }
        return new Headers(headerConfig);
    }

    private formatErrors(e: any) {
        console.log(e);
        //return Observable.throw(error.message || error);
        return Observable.throw(e.json());
    }

    get(path: string,
        params: URLSearchParams = new URLSearchParams()): Observable<any> {
        return this.http.get(`${this.apiUrl}${path}`, { headers: this.setHeaders(), search: params })
            .catch(this.formatErrors)
            .map((res: Response) => res.json());

    }

    put(path: string, body: Object = {}): Observable<any> {
        return this.http.put(
            `${this.apiUrl}${path}`,
            JSON.stringify(body),
            { headers: this.setHeaders() }
        )
            .catch(this.formatErrors)
            .map((res: Response) => res.json());
    }

    post(path: string, body: Object = {}): Observable<any> {
        return this.http.post(
            `${this.apiUrl}${path}`,
            JSON.stringify(body),
            { headers: this.setHeaders() }
        )
            .catch(this.formatErrors)
            .map((res: Response) => res.json());
    }

    delete(path): Observable<any> {
        return this.http.delete(
            `${this.apiUrl}${path}`,
            { headers: this.setHeaders() }
        )
            .catch(this.formatErrors)
            .map((res: Response) => res.json());
    }
}