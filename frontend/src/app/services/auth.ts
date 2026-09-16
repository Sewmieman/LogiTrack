import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

<<<<<<< HEAD
export interface AuthResponse {
  token: string;
  userId: string;
  email: string;
  firstName: string;
  lastName: string;
  roles: string[];
}

export interface RegisterRequest {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private readonly http = inject(HttpClient);

  private readonly apiUrl = 'http://localhost:5148/api/auth';

=======
export interface AuthResponse { token: string; userId: string; email: string; firstName: string; lastName: string; roles: string[]; }
export interface RegisterRequest { firstName: string; lastName: string; email: string; password: string; }
export interface LoginRequest { email: string; password: string; }

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = 'http://localhost:5148/api/auth';
>>>>>>> 592b43e4cecbcc7d3b7f7dd849a7a59b40749c97
  private readonly tokenKey = 'logitrack_token';
  private readonly userKey = 'logitrack_user';

  register(request: RegisterRequest): Observable<AuthResponse> {
<<<<<<< HEAD
    return this.http.post<AuthResponse>(
      `${this.apiUrl}/register`,
      request
    );
  }

  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(`${this.apiUrl}/login`, request)
      .pipe(
        tap(response => this.saveSession(response))
      );
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.userKey);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }

  getUser(): AuthResponse | null {
    const raw = localStorage.getItem(this.userKey);

    if (!raw) {
      return null;
    }

    try {
      return JSON.parse(raw) as AuthResponse;
    } catch {
      return null;
    }
  }

  getRoles(): string[] {
    const user = this.getUser();

    if (!user || !Array.isArray(user.roles)) {
      return [];
    }

    return user.roles;
  }

  hasRole(role: string): boolean {
    return this.getRoles().some(
      r => r.toLowerCase() === role.toLowerCase()
    );
  }

  hasAnyRole(roles: string[]): boolean {
    const userRoles = this.getRoles().map(r => r.toLowerCase());

    return roles.some(
      role => userRoles.includes(role.toLowerCase())
    );
  }

  private saveSession(response: AuthResponse): void {
    localStorage.setItem(this.tokenKey, response.token);
    localStorage.setItem(
      this.userKey,
      JSON.stringify(response)
    );
  }
}
=======
    return this.http.post<AuthResponse>(`${this.apiUrl}/register`, request);
  }
  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, request).pipe(tap(r => this.saveSession(r)));
  }
  logout(): void { localStorage.removeItem(this.tokenKey); localStorage.removeItem(this.userKey); }
  getToken(): string | null { return localStorage.getItem(this.tokenKey); }
  isAuthenticated(): boolean { return !!this.getToken(); }
  getUser(): AuthResponse | null {
    const raw = localStorage.getItem(this.userKey);
    if (!raw) return null;
    try { return JSON.parse(raw) as AuthResponse; } catch { return null; }
  }
  private saveSession(response: AuthResponse): void {
    localStorage.setItem(this.tokenKey, response.token);
    localStorage.setItem(this.userKey, JSON.stringify(response));
  }
}
>>>>>>> 592b43e4cecbcc7d3b7f7dd849a7a59b40749c97
