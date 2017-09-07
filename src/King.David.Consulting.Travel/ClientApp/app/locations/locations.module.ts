import { CommonModule } from "@angular/common";
import { ModuleWithProviders, NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";

import { LocationsComponent  } from "./locations.component";
import { CitiesComponent } from "./cities.component";

const locationRouting: ModuleWithProviders = RouterModule.forChild([
    {
        path: 'locations/:username',
        component: CitiesComponent,
        // children: [
        //     {
        //         path: '',
        //         component: CitiesComponent
        //     },
        //     {}
        // ]
    }
]);

@NgModule({
    imports: [
        locationRouting,
            CommonModule

    ],
    declarations: [
        LocationsComponent,
        CitiesComponent
    ]
})
export class LocationsModule { }