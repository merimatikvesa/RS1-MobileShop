import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, AbstractControl, ValidationErrors} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth/auth.service';
import { RegisterRequestDto } from '../../../core/models/auth/register-request.dto';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { TranslateModule, TranslateService } from '@ngx-translate/core';


@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatSnackBarModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatIconModule, RouterLink, TranslateModule],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private snack = inject(MatSnackBar);
  private router = inject(Router);

  loading = false;
  passwordStrenght =0;
  passwordStrenghtLabel='';

  get showPasswordMismatch(): boolean {
    return this.form.hasError('passwordMismatch') && this.form.touched;
  }

  form: FormGroup = this.fb.group(
    {
      fullName: ['', [Validators.required,
      Validators.minLength(3),
      Validators.pattern(/^[A-Za-zČĆŽĐŠčćžđš\s]+$/)]
        ],
      email: ['', [
        Validators.required,
        Validators.email,
        Validators.pattern(/^[^\s@]+@[^\s@]+\.[A-Za-z]{2,}$/)

      ]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', [Validators.required, this.confirmPasswordValidator()]],
    },
    { validators: passwordMatchValidator }
  );
  checkPasswordStrength(password: string): void {
    let strength = 0;

    if (password.length >= 8) {
      strength++;
    }

    if (/\d/.test(password)) {
      strength++;
    }

    if (/[A-Z!@#$%^&*(),.?":{}|<>]/.test(password)) {
      strength++;
    }

    this.passwordStrenght = strength;

    if (strength <= 1) {
      this.passwordStrenghtLabel = 'Weak';
    } else if (strength === 2) {
      this.passwordStrenghtLabel = 'Medium';
    } else {
      this.passwordStrenghtLabel= 'Strong';
    }
  }
  confirmPasswordValidator() {
    return (control: AbstractControl) => {
      if (!control.parent) return null;

      const password = control.parent.get('password')?.value;
      const confirm = control.value;

      return password === confirm ? null : { passwordMismatch: true };
    };
  }

  private translate = inject(TranslateService);

  currentLang: 'bs' | 'en' =
    (localStorage.getItem('lang') as 'bs' | 'en') || 'bs';

  constructor() {
    this.translate.setDefaultLang('bs');
    this.translate.use(this.currentLang);
  }

  setLang(lang: 'bs' | 'en') {
    this.currentLang = lang;
    localStorage.setItem('lang', lang);
    this.translate.use(lang);
  }

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


