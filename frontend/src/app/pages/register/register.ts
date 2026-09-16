import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

import {
  AuthService,
  RegisterRequest
} from '../../services/auth';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    FormsModule,
    RouterLink
  ],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class Register {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  firstName = '';
  lastName = '';
  email = '';
  password = '';

  errorMessage = '';
  successMessage = '';
  loading = false;
  registeredSuccessfully = false;

  get passwordRequirements() {
    return {
      minLength: this.password.length >= 8,
      uppercase: /[A-Z]/.test(this.password),
      lowercase: /[a-z]/.test(this.password),
      number: /\d/.test(this.password),
      specialCharacter: /[^A-Za-z0-9]/.test(this.password)
    };
  }

  get isPasswordValid(): boolean {
    const requirements = this.passwordRequirements;

    return requirements.minLength &&
      requirements.uppercase &&
      requirements.lowercase &&
      requirements.number &&
      requirements.specialCharacter;
  }

  get canSubmit(): boolean {
    return !!this.firstName.trim() &&
      !!this.lastName.trim() &&
      !!this.email.trim() &&
      this.isPasswordValid &&
      !this.loading;
  }

  register(): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (
      !this.firstName.trim() ||
      !this.lastName.trim() ||
      !this.email.trim() ||
      !this.password
    ) {
      this.errorMessage = 'Please fill in all fields.';
      return;
    }

    if (!this.isPasswordValid) {
      this.errorMessage =
        'Password must be at least 8 characters and include an uppercase letter, a lowercase letter, a number, and a special character.';
      return;
    }

    const request: RegisterRequest = {
      firstName: this.firstName.trim(),
      lastName: this.lastName.trim(),
      email: this.email.trim(),
      password: this.password
    };

    this.loading = true;

    this.authService.register(request).subscribe({
      next: () => {
        this.loading = false;
        this.registeredSuccessfully = true;
        this.successMessage =
          'Registration successful! Your account has been created. Please sign in to continue.';

        // Registration does NOT automatically log the user in or open the dashboard.
        // The user must explicitly go to the login page.
      },
      error: (error) => {
        this.loading = false;
        this.registeredSuccessfully = false;

        if (Array.isArray(error.error)) {
          this.errorMessage = error.error
            .map((item: { description?: string; message?: string }) =>
              item.description ?? item.message ?? 'Registration failed.'
            )
            .join(' ');
        } else if (Array.isArray(error.error?.errors)) {
          this.errorMessage = error.error.errors
            .map((item: string | { description?: string }) =>
              typeof item === 'string'
                ? item
                : item.description ?? 'Registration failed.'
            )
            .join(' ');
        } else {
          this.errorMessage =
            error.error?.message ??
            'Registration failed. Please try again.';
        }
      }
    });
  }

  goToLogin(): void {
    this.router.navigate(['/login']);
  }
}
