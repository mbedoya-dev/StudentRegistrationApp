import { Routes } from '@angular/router';
import { StarterComponent } from './starter/starter.component';
import { StudentRegistrationComponent } from './student-registration/student-registration.component';

export const PagesRoutes: Routes = [
  {
    path: '',
    component: StudentRegistrationComponent,
    data: {
      title: 'Starter Page',
    },
  },
];
