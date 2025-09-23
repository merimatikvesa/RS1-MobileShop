import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const authGuard: CanActivateFn = (route, state) => {
   const router = inject(Router);
  const token = localStorage.getItem('token');

  if (token) {
    return true; 
  }

  // no token - redirect on login i zapamti gdje je htio ici
  inject(Router).navigate(['/login'], { queryParams: { returnUrl: state.url } });
  return false;
};
