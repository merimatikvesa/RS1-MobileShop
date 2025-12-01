import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../core/services/auth/auth.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-orders',
  imports: [],
  templateUrl: './orders.component.html',
  styleUrl: './orders.component.css'
})
export class OrdersComponent {
    constructor(
      private auth: AuthService, 
      private router: Router,
      private http: HttpClient){}

    ngOnInit(): void {
    this.http.get('https://localhost:7275/api/test/protected').subscribe({
      next: (res) => console.log('Uspjeh:', res),
      error: (err) => console.log('Greška:', err)
    });
    }


    logout(){
      this.auth.logout();
      this.router.navigate(['/login'])
    }
}
