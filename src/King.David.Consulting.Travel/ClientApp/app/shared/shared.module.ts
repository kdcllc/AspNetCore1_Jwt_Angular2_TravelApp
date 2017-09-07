import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { ListErrorsComponent, ShowAuthedDirective } from './index';
//import { ListErrorsComponent } from "./list-errors.component";
//import { ShowAuthedDirective } from "./show-authed.directive";

import { VisitListComponent, VisitPreviewComponent } from "./visit-helpers/index";

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        HttpModule,
        RouterModule
    ],
    declarations: [
        ListErrorsComponent,
        ShowAuthedDirective,
        VisitListComponent,
        VisitPreviewComponent
    ],
    exports: [
        CommonModule,
        FormsModule,
        HttpModule,
        RouterModule,
        ReactiveFormsModule,
        ListErrorsComponent,
        ShowAuthedDirective,
        VisitListComponent,
        VisitPreviewComponent
    ]
})
export class SharedModule {}