import { ModuleWithProviders, NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { AuthComponent } from './auth.component';
import { NoAuthGuard } from '../services/index';
import { SharedModule } from '../shared/index';

const authRoutring: ModuleWithProviders = RouterModule.forChild([
    {
        path: 'login',
        component: AuthComponent,
        canActivate: [NoAuthGuard]
    },
    {
        path: 'register',
        component: AuthComponent,
        canActivate: [NoAuthGuard]
    }
]);

@NgModule({
    imports: [
        authRoutring,
        SharedModule
    ],
    declarations: [
        AuthComponent
    ],
    providers: [
        NoAuthGuard
    ]
})
export class AuthModule { }