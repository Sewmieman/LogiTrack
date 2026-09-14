import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

import {
  AuthService,
  LoginRequest
} from '../../services/auth';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    FormsModule,
    RouterLink
  ],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  email = '';
  password = '';

  errorMessage = '';
  loading = false;

  login(): void {
    this.errorMessage = '';

    if (!this.email || !this.password) {
      this.errorMessage =
        'Please enter your email and password.';
      return;
    }

    const request: LoginRequest = {
      email: this.email,
      password: this.password
    };

    this.loading = true;

    this.authService.login(request).subscribe({
      next: () => {
        this.loading = false;
        this.router.navigate(['/dashboard']);
      },
      error: (error) => {
        this.loading = false;

        this.errorMessage =
          error.error?.message ??
          'Invalid email or password.';
      }
    });
  }
}