import { Routes } from '@angular/router';
import { StarterComponent } from './starter/starter.component';
import { StudentRegistrationComponent } from './student-registration/student-registration.component';
import { SubjectEnrollmentComponent } from './subject-enrollment/subject-enrollment.component';

export const PagesRoutes: Routes = [
  {
    path: '',
    children: [
      {
        path: 'starter',
        component: StarterComponent,
        data: {
          title: 'Dashboard',
          urls: [
            { title: 'Home', url: '/dashboards/dashboard1' },
            { title: 'Dashboard' },
          ],
        },
      },
      {
        path: 'student-registration',
        component: StudentRegistrationComponent,
        data: {
          title: 'Dashboard',
          urls: [
            { title: 'Home', url: '/dashboards/dashboard1' },
            { title: 'Dashboard' },
          ],
        },
      },
      {
        path: 'subject-enrollment',
        component: SubjectEnrollmentComponent,
        data: {
          title: 'Dashboard',
          urls: [
            { title: 'Home', url: '/dashboards/dashboard1' },
            { title: 'Dashboard' },
          ],
        },
      },
    ],
  },
];
