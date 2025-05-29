import { Routes } from '@angular/router';
import { FullComponent } from './layouts/full/full.component';
import { AppErrorComponent } from './pages/error/error.component';

export const routes: Routes = [
  {
    path: '',
    component: FullComponent,
    children: [
      {
        path: '',
        redirectTo: '/dashboards/dashboard1',
        pathMatch: 'full',
      },
      {
        path: 'starter',
        loadChildren: () =>
          import('./pages/pages.routes').then((m) => m.PagesRoutes),
      },
      {
        path: 'apps',
        loadChildren: () =>
          import('./pages/apps/apps.routes').then((m) => m.AppsRoutes),
      },
    ],
  },
  {
    path: 'error',
    component: AppErrorComponent,
  },
  {
    path: '**',
    redirectTo: '/error',
  },
];
