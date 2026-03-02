import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { LandingComponent } from './landing/landing.component';

export const routes: Routes = [
  //public
  { path: '', component: LandingComponent },

  { path: 'products', loadComponent: () => import('./features/products/products.component').then(m => m.ProductsComponent) },

  { path: 'login', loadComponent: () => import('./features/auth/login/login.component').then(m => m.LoginComponent) },
  { path: 'register', loadComponent: () => import('./features/auth/register/register.component').then(m => m.RegisterComponent) },

  { path: 'inventory', loadComponent: () => import('./features/inventory/inventory.component').then(m => m.InventoryComponent) },

  { path: 'orders', canActivate: [authGuard], loadComponent: () => import('./features/orders/orders.component').then(m => m.OrdersComponent) },

  { path: '**', redirectTo: '' }
];
