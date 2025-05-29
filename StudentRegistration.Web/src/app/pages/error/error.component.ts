import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';

@Component({
    selector: 'app-error',
    imports: [RouterModule, MatButtonModule],
    templateUrl: './error.component.html'
})
export class AppErrorComponent {}
