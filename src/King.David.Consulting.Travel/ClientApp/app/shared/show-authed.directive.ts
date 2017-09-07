import {
    Directive,
    Input,
    OnInit,
    TemplateRef,
    ViewContainerRef
} from '@angular/core';

import { UserService } from '../services/index';

@Directive({
    selector: '[showAuthed]'
})
export class ShowAuthedDirective implements OnInit {

    constructor(
        private templateRef: TemplateRef<any>,
        private userService: UserService,
        private viewContainer: ViewContainerRef
    ) { 

        console.log("This is ShowAuthedDirective directive");
    }

   condition: boolean;

    ngOnInit() {
        console.log(`${this.condition}`);
        this.userService.isAuthenticated.subscribe(
            (isAuthenticated) => {
                console.log(`${this.condition} ${isAuthenticated}`);
                if (isAuthenticated && this.condition || !isAuthenticated && !this.condition) {
                    this.viewContainer.createEmbeddedView(this.templateRef);
                } else {
                    this.viewContainer.clear();
                }
            }
        )
    }

    @Input() set showAuthed(condition: boolean) {
    this.condition = condition;
  }
}