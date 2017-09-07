import { ModuleWithProviders, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component'

import { HeaderComponent, FooterComponent } from './layout';
import { HomeModule } from './home';
import { AuthModule } from './auth';
import { LocationsModule } from "./locations";
import { AboutModule } from "./about";
import { ApiService, UserService, JwtService, AuthGuard, VisitsService, LocationsService } from './services';
import { SharedModule } from './shared/shared.module';

const rootRouting: ModuleWithProviders = RouterModule.forRoot([{
    path: '',
    redirectTo: 'home',
    pathMatch: 'prefix'
}], { useHash: true });

//https://www.codementor.io/christiannwamba/build-custom-directives-in-angular-2-jlqrk7dpw
@NgModule({
    bootstrap: [AppComponent],
    declarations: [
        AppComponent,
        HeaderComponent,
        FooterComponent
    ],
    imports: [
        BrowserModule,
        FormsModule,
        HttpModule,
        BrowserModule,
        HomeModule,
        RouterModule,
        rootRouting,
        SharedModule,
        AuthModule,
        AboutModule,
        LocationsModule
    ],
    providers: [
        { provide: 'ORIGIN_URL', useValue: location.origin },
        [ApiService,
         UserService,
         JwtService,
         AuthGuard,
         VisitsService,
         LocationsService]
    ]
})
export class AppModule {
}
