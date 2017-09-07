import { ModuleWithProviders, NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";

import { AboutComponent } from "./about.component";

const aboutRouting: ModuleWithProviders = RouterModule.forChild([
    {
        path: 'about',
        component: AboutComponent
    }
]);

@NgModule({
    imports: [
        aboutRouting
    ],
    declarations: [
        AboutComponent
    ]
})
export class AboutModule{}