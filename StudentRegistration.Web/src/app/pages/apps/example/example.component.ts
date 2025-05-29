import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import { MaterialModule } from 'src/app/material.module';

@Component({
  selector: 'app-example',
  imports: [MaterialModule, CommonModule, ToastrModule],
  providers: [ToastrService],
  templateUrl: './example.component.html',
  styleUrl: './example.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ExampleComponent { 
  constructor(private toastr: ToastrService) {} 

  showSuccess() {
    this.toastr.success('You are awesome!', 'Success!');
  }

  showError() {
    this.toastr.error('This is not good!', 'Oops!');
  }

  showWarning() {
    this.toastr.warning('You are being warned.', 'Alert!');
  }

  showInfo() {
    this.toastr.info('Just some information for you.');
  }
}

