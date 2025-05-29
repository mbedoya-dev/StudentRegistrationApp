import { Routes } from '@angular/router';

import { StudentRegistrationComponent } from './student-registration/student-registration.component';
import { SubjectEnrollmentComponent } from './subject-enrollment/subject-enrollment.component';
import { SharedClassComponent } from './shared-class/shared-class.component';

export const PagesRoutes: Routes = [
  {
    path: '',
    component: StudentRegistrationComponent,
    data: {
      title: 'Registrar Estuiante',
    },
  },
  {
    path: 'subject-enrollment',
    component: SubjectEnrollmentComponent,
    data: {
      title: 'Registrar Materias',
    },
  },
  {
    path: 'shared-class',
    component: SharedClassComponent,
    data: {
      title: 'Registrar Materias',
    },
  },
];
