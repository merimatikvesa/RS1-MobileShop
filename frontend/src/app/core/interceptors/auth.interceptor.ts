import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { switchMap, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from '../services/auth/auth.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  console.log('%cINTERCEPTOR RUNNING', 'color: red; font-size: 20px');

  const authService = inject(AuthService);
  const router = inject(Router);

  if (req.url.includes('/auth/refresh') || 
      req.url.includes('/auth/login') ||
      req.url.includes('/auth/register')) 
      {
        return next(req);
      }

  // Add access token
  const token = authService.getAccessToken();
  const authReq = token
    ? req.clone({
        setHeaders: { Authorization: `Bearer ${token}` }
      })
    : req;

  return next(authReq).pipe(
    catchError((err: HttpErrorResponse) => {

      // Access token expire 401
      if (err.status === 401) {
        console.log("%cSTEP 1: 401 DETECTED", "color:red; font-size:18px");
        const refreshToken = authService.getRefreshToken();

        if (!refreshToken) {
          authService.logout();
          router.navigate(['/login']);
          return throwError(() => err);
        }

        // Refresh request
        return authService.refresh(refreshToken).pipe(
          switchMap((newTokens) => {


            authService.storeTokens(newTokens);

            // Retry original request with New access token
            const retryReq = req.clone({
              setHeaders: {
                Authorization: `Bearer ${newTokens.token}`
              }
            });
            return next(retryReq);
          }),

          catchError((refreshErr) => {
           console.error('Refresh failed → logout');
           authService.logout();
           router.navigate(['/login']);
           return throwError(() => refreshErr);
          })
        );
      }

      return throwError(() => err);
    })
  );
};
