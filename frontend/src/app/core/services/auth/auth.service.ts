import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap, Observable } from 'rxjs';
import { LoginRequestDto } from '../../models/auth/login-request.dto';
import { LoginResponseDto } from '../../models/auth/login-response.dto';
import { environment } from '../../../../evnvironments/environment';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = environment.apiBaseUrl; // prilagodi svom backendu
  private snack = inject(MatSnackBar);
  constructor(private http: HttpClient) {}

  login(dto: LoginRequestDto): Observable<LoginResponseDto> {
    return this.http.post<LoginResponseDto>(`${this.apiUrl}/auth/login`, dto).pipe(
      tap((response: LoginResponseDto) => {
        this.storeTokens(response);  
      })
    );
  }

   private storeTokens(tokens: LoginResponseDto): void {
    localStorage.setItem('token', tokens.token);        // ACCESS TOKEN
    localStorage.setItem('username', tokens.username);         // USERNAME
  }

  getAccessToken(): string | null {
    return localStorage.getItem('token');
  }

  getRefreshToken(): string | null {
    return localStorage.getItem('refreshToken');
  }

  logout():void {
  localStorage.removeItem('token');
  localStorage.removeItem('username');

   this.snack.open('Uspješno ste se odjavili.', 'Zatvori', {
      duration: 3000,
      panelClass: ['snack-success']
    });
}
}
