import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { AuthService } from '../../../core/services/auth/auth.service';
import { LoginRequestDto } from '../../../core/models/auth/login-request.dto';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  private fb = inject(FormBuilder);
  private router = inject(Router);
  private route = inject(ActivatedRoute)
  private authService = inject(AuthService);
  

  loading = false;
  errorMsg = '';

  form: FormGroup = this.fb.group({
    username: ['', Validators.required],
    password: ['', [Validators.required, Validators.minLength(6)]],
  });

  submit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.loading = true;
    const payload: LoginRequestDto = this.form.value;

    this.authService.login(payload).subscribe({
      next: (res) => {
        console.log('Login success:', res);
      
        //saving token and username in LocalStorage
      localStorage.setItem('token', res.token);
      localStorage.setItem('username', res.username);

        const returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
        this.router.navigateByUrl(returnUrl);

      this.loading = false;
      },
      error: (err) => {
        console.error('logina failed: ', err)
        this.errorMsg = 'Invalid username or password';
        this.loading = false;
      }
    });

  }

  get f() { return this.form.controls; }
}
