import { Routes } from '@angular/router';

import { Dashboard } from './features/dashboard/dashboard';
import { Landing } from './features/landing/landing';

export const routes: Routes = [
  { path: '', component: Landing },
  { path: 'auth/login', loadComponent: () => import('./features/auth/login').then((m) => m.Login) },
  { path: 'dashboard', component: Dashboard },
  {
    path: 'customers',
    loadComponent: () => import('./features/customers/customers').then((m) => m.Customers),
  },
  {
    path: 'workorders',
    loadComponent: () => import('./features/workorders/workorders').then((m) => m.WorkOrders),
  },
  {
    path: 'repairtasks',
    loadComponent: () => import('./features/repairtasks/repairtasks').then((m) => m.RepairTasks),
  },
  {
    path: 'schedules',
    loadComponent: () => import('./features/schedules/schedules').then((m) => m.Schedules),
  },
];
