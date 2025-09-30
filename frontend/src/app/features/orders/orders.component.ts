import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../core/services/auth/auth.service';

@Component({
  selector: 'app-orders',
  imports: [],
  templateUrl: './orders.component.html',
  styleUrl: './orders.component.css'
})
export class OrdersComponent {
    constructor(private auth: AuthService, private router: Router){}

    logout(){
      this.auth.logout();
      this.router.navigate(['/login'])
    }
}
