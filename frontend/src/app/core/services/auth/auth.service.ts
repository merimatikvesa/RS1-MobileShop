import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap, Observable, BehaviorSubject } from 'rxjs';
import { LoginRequestDto } from '../../models/auth/login-request.dto';
import { LoginResponseDto } from '../../models/auth/login-response.dto';
import { environment } from '../../../../evnvironments/environment';
import { MatSnackBar } from '@angular/material/snack-bar';
import {RegisterRequestDto} from '../../models/auth/register-request.dto';



@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = environment.apiBaseUrl;
  private snack = inject(MatSnackBar);
  private loggedIn$ = new BehaviorSubject<boolean>(!!localStorage.getItem('token'));
  private username$ = new BehaviorSubject<string>(localStorage.getItem('username') ?? '');

  constructor(private http: HttpClient) {}

  login(dto: LoginRequestDto): Observable<LoginResponseDto> {
    return this.http.post<LoginResponseDto>(`${this.apiUrl}/auth/login`, dto).pipe(
      tap((response: LoginResponseDto) => {
        this.storeTokens(response);
      })
    );
  }
  register(dto: RegisterRequestDto) {
    return this.http.post(
      `${this.apiUrl}/auth/register`,
      dto
    );
  }



  storeTokens(tokens: LoginResponseDto): void {
    localStorage.setItem('token', tokens.token);
    localStorage.setItem('refreshToken', tokens.refreshToken);
    localStorage.setItem('username', tokens.username);

    this.loggedIn$.next(true);
    this.username$.next(tokens.username);
  }


  getAccessToken(): string | null {
    return localStorage.getItem('token');
  }

  getRefreshToken(): string | null {
    return localStorage.getItem('refreshToken');
  }

  refresh(refreshToken?: string): Observable<LoginResponseDto> {
  return this.http.post<LoginResponseDto>(`${this.apiUrl}/auth/refresh`, {
    refreshToken: refreshToken || this.getRefreshToken()
  });
}



  logout():void {
  localStorage.removeItem('token');
  localStorage.removeItem('refreshToken');
  localStorage.removeItem('username');

    this.loggedIn$.next(false);
    this.username$.next('');
    this.snack.open('You have been successfully logged out.', 'Close', {
      duration: 3000,
      panelClass: ['snack-success']
    });
}
  isLoggedIn(): boolean {
  return this.loggedIn$.value; 
}

  getUsername(): string {
  return this.username$.value;
}


  loggedInChanges() {
  return this.loggedIn$.asObservable();
}

  usernameChanges() {
  return this.username$.asObservable();
}

}
