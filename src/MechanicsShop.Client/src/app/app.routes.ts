import { Routes } from '@angular/router';

import { Dashboard } from './features/dashboard/dashboard';
import { Landing } from './features/landing/landing';

export const routes: Routes = [
  { path: '', component: Landing },
  { path: 'auth/login', loadComponent: () => import('./features/auth/login').then(m => m.Login) },
  { path: 'dashboard', component: Dashboard },
];
