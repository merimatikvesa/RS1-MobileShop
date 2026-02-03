import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { AuthService } from '../../../core/services/auth/auth.service';
import { LoginRequestDto } from '../../../core/models/auth/login-request.dto';
import { ActivatedRoute, Router, RouterLink} from '@angular/router';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatStepperModule } from '@angular/material/stepper';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { TranslateModule, TranslateService } from '@ngx-translate/core';


@Component({
  selector: 'app-login',
  standalone: true,
  imports: 
  [CommonModule, 
  ReactiveFormsModule, 
  MatSnackBarModule, 
  RouterLink,  
  MatStepperModule,
  MatInputModule,
  MatButtonModule,
  TranslateModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})

export class LoginComponent {
  private fb = inject(FormBuilder);
  private router = inject(Router);
  private route = inject(ActivatedRoute)
  private authService = inject(AuthService);
  private snack = inject(MatSnackBar);

  private translate = inject(TranslateService);

  currentLang: 'bs' | 'en' = (localStorage.getItem('lang') as 'bs' | 'en') || 'bs';

  constructor() {
    this.translate.setDefaultLang('bs');
    this.translate.use(this.currentLang);
  }

  setLang(lang: 'bs' | 'en') {
    this.currentLang = lang;
    localStorage.setItem('lang', lang);
    this.translate.use(lang);
  }

  usernameForm = this.fb.group({
    username: ['', Validators.required]
  });

  passwordForm = this.fb.group({
    password: ['', Validators.required]
  });

  loading = false;
  errorMsg = '';

  loginWizard(): void {
    if (this.usernameForm.invalid || this.passwordForm.invalid) {
      this.usernameForm.markAllAsTouched();
      this.passwordForm.markAllAsTouched();
      return;
    }


 // form: FormGroup = this.fb.group({
 //   username: ['', Validators.required],
 //   password: ['', [Validators.required, Validators.minLength(6)]],
 // });

 // submit() {
 //   if (this.form.invalid) {
 //     this.form.markAllAsTouched();
 //     return;
 //   }

    this.loading = true;

    const payload: LoginRequestDto = {
      username: this.usernameForm.value.username!,
      password: this.passwordForm.value.password!
    };
   
    // const payload: LoginRequestDto = this.form.value;

    this.authService.login(payload).subscribe({
      next: (res) => {
        console.log('Login success:', res);

        //saving token and username in LocalStorage
      localStorage.setItem('token', res.token);
      localStorage.setItem('username', res.username);

        this.snack.open('Uspješna prijava! Dobrodošli.', 'Zatvori', { duration: 3000, panelClass: ['snack-success'] });

        const returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
        this.router.navigateByUrl(returnUrl);

      this.loading = false;
      },
      error: (err) => {
        this.loading = false;
        if (err?.status === 401) {
          this.snack.open(' Pogrešan username ili lozinka.', 'Zatvori', { duration: 4000, panelClass: ['snack-error'] });
        } else if (err?.status === 0) {
          this.snack.open(' Server nedostupan. Provjerite vezu.', 'Zatvori', { duration: 4000, panelClass: ['snack-error'] });
        } else {
          const msg = err?.error?.title || err?.error?.message || ' Greška prilikom prijave.';
          this.snack.open(msg, 'Zatvori', { duration: 4000 , panelClass: ['snack-error']});
        }
      }
    });

  }

  //get f() { return this.form.controls; }
}
