import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { ReplaySubject } from 'rxjs/ReplaySubject';

import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch'

import { ApiService, JwtService } from './index';
import { User } from '../models/index';

@Injectable()
export class UserService {

    private currentUserSubject = new BehaviorSubject<User>(new User());
    public currentUser = this.currentUserSubject.asObservable().distinctUntilChanged();

    private isAuthenticatedSubject = new ReplaySubject<boolean>(1);
    public isAuthenticated = this.isAuthenticatedSubject.asObservable();

    constructor(
        private apiService: ApiService,
        private http: Http,
        private jwtService: JwtService
    ) {}

    // verify jwt in storage with server and load user's info
    // this method runs only on application startup.
    populate() {
        //if jwt detected, attempt to get & store user's info
        if (this.jwtService.getToken()){
            this.apiService.get('/user')
            .subscribe(
                data => this.setAuth(data.user),
                err => this.purgeAuth()
            );
        } else {
            //remove any remnants of the auth states
            this.purgeAuth();
        }
    }

    setAuth(user:User){
        // save jwt sent from server in storage
        this.jwtService.saveToken(user.token);
        // set current user data into observable
        this.currentUserSubject.next(user);
        // set isAuthenticated to true
        this.isAuthenticatedSubject.next(true);
    }

    purgeAuth(){
        // remove jwt from storage
        this.jwtService.destroyToken();
        // set current user to an empty object
        this.currentUserSubject.next(new User());
        // set isAuthenticated to false
        this.isAuthenticatedSubject.next(false);
    }

    attemptAuth(type, credentials): Observable<User> {
        const route = (type === 'login') ? '/login' : '';

        return this.apiService.post('/users' + route, {user: credentials})
            .map(
                data => {
                    this.setAuth(data.user);
                    return data;
                }
            );
    }

    getCurrentUser(): User {
        return this.currentUserSubject.value;
    }

    update(user: User) : Observable<User>{
        return this.apiService
            .put('/user', {user})
            .map(data => {
                //update observable
                this.currentUserSubject.next(data.user);
                return data.user;
            })
    }
}