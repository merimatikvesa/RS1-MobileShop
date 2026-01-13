import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
  AbstractControl,
  ValidationErrors
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth/auth.service';
import { RegisterRequestDto } from '../../../core/models/auth/register-request.dto';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatSnackBarModule, RouterLink],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private snack = inject(MatSnackBar);
  private router = inject(Router);

  loading = false;

  get showPasswordMismatch(): boolean {
    return this.form.hasError('passwordMismatch') && this.form.touched;
  }

  // form must be public, so it can be accessed in the template
  form: FormGroup = this.fb.group(
    {
      fullName: ['', Validators.required],
      email: ['', [
        Validators.required,
        Validators.email,
        allowedEmailDomainValidator
      ]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required],
    },
    { validators: passwordMatchValidator }
  );

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const payload: RegisterRequestDto = {
      fullName: this.form.value.fullName,
      email: this.form.value.email,
      password: this.form.value.password
    };

    this.loading = true;

    this.authService.register(payload).subscribe({
      next: () => {
        this.snack.open(
          'Registration successful. Please log in.',
          'Close',
          { duration: 3000, panelClass: ['snack-success'] }
        );
        this.router.navigate(['/login']);
        this.loading = false;
      },
      error: (err) => {
        this.loading = false;
        const msg = err?.error || 'An error occurred during registration.';
        this.snack.open(msg, 'Close', {
          duration: 4000,
          panelClass: ['snack-error']
        });
      }
    });
  }
}

/* custom validator */
function passwordMatchValidator(
  control: AbstractControl
): ValidationErrors | null {
  const password = control.get('password')?.value;
  const confirmPassword = control.get('confirmPassword')?.value;

  if (!password || !confirmPassword) {
    return null;
  }

  return password === confirmPassword
    ? null
    : { passwordMismatch: true };
}
function allowedEmailDomainValidator(control: AbstractControl): ValidationErrors | null {
  const email = control.value;
  if (!email) return null;

  const allowedDomains = ['gmail.com', 'outlook.com', 'hotmail.com'];
  const domain = email.split('@')[1];

  return allowedDomains.includes(domain)
    ? null
    : { invalidDomain: true };
}
