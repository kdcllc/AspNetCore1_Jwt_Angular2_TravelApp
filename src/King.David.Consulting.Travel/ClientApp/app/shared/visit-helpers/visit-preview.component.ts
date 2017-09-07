import { Component, Input } from "@angular/core";

import { Visit } from "../../models/index";

@Component({
    selector: 'visit-preview',
    templateUrl: './visit-preview.component.html'
})
export class VisitPreviewComponent{
    @Input() visit: Visit;
}