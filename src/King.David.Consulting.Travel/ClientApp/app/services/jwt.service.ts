import { Injectable, Inject } from '@angular/core';

@Injectable()
export class JwtService {
    tokenStr: string;

    constructor() {
        this.tokenStr = 'jwtToken';
    }
    getToken(): String {
        //return sessionStorage.getItem(this.tokenStr);
        return window.localStorage[this.tokenStr];
    }

    saveToken(token: string) {
        //sessionStorage.setItem(this.tokenStr,token);
        window.localStorage[this.tokenStr] = token;
    }

    destroyToken() {
        //sessionStorage.removeItem(this.tokenStr);
        window.localStorage.removeItem(this.tokenStr);
    }
}