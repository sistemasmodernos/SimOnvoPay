import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', redirectTo: '/checkout', pathMatch: 'full' },
  {
    path: 'checkout/:token',
    loadComponent: () => import('./pages/checkout/checkout.component').then(m => m.CheckoutComponent)
  },
  {
    path: 'success',
    loadComponent: () => import('./pages/success/success.component').then(m => m.SuccessComponent)
  },
  {
    path: 'cancelled',
    loadComponent: () => import('./pages/cancelled/cancelled.component').then(m => m.CancelledComponent)
  },
  { path: '**', redirectTo: '/checkout' }
];
