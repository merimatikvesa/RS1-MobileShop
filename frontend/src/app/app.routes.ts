import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { LandingComponent } from './landing/landing.component';
import { adminGuard } from './core/guards/admin.guard';

export const routes: Routes = [
  //public
  { path: '', component: LandingComponent },

  { path: 'products', canActivate: [adminGuard], loadComponent: () => import('./features/products/products.component').then(m => m.ProductsComponent) },

  { path: 'login', loadComponent: () => import('./features/auth/login/login.component').then(m => m.LoginComponent) },
  { path: 'register', loadComponent: () => import('./features/auth/register/register.component').then(m => m.RegisterComponent) },

  { path: 'inventory',canActivate: [adminGuard], loadComponent: () => import('./features/inventory/inventory.component').then(m => m.InventoryComponent) },
  {path: 'shop', loadComponent: () => import('./features/shop/shop.component').then(m => m.ShopComponent) },

  { path: 'orders', canActivate: [authGuard], loadComponent: () => import('./features/orders/orders.component').then(m => m.OrdersComponent) },

  { path: '**', redirectTo: '' },

];
