import { Component, ViewEncapsulation } from '@angular/core';
import { MaterialModule } from 'src/app/material.module';

@Component({
    selector: 'app-starter',
    templateUrl: './starter.component.html',
    imports: [MaterialModule],
    encapsulation: ViewEncapsulation.None
})
export class StarterComponent {
}
