import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { Router } from '@angular/router';

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
  phoneNumber: string;
  userType: 'User' | 'Owner';
}

export interface LoginResponse {
  token: string;
  role: 'User' | 'Owner';
  email: string;
  userId: string;
  firstName: string;
  lastName: string;
}

export interface RegisterResponse {
  token: string;
  role: 'User' | 'Owner';
  email: string;
  userId: string;
  firstName: string;
  lastName: string;
  message: string;
}

export interface User {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  phoneNumber?: string;
  userType: 'User' | 'Owner';
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5158/api';
  private currentUserSubject = new BehaviorSubject<User | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(
    private http: HttpClient,
    private router: Router
  ) {
    // Check if user is already logged in
    this.loadUserFromStorage();
  }

  login(loginRequest: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/auth/login`, loginRequest)
      .pipe(
        tap(response => {
          this.handleAuthResponse(response);
        })
      );
  }

  register(registerRequest: RegisterRequest): Observable<RegisterResponse> {
    return this.http.post<RegisterResponse>(`${this.apiUrl}/auth/register`, registerRequest)
      .pipe(
        tap(response => {
          this.handleAuthResponse(response);
        })
      );
  }

  private handleAuthResponse(response: LoginResponse | RegisterResponse): void {
    // Check if we're in the browser environment
    if (typeof window !== 'undefined' && typeof localStorage !== 'undefined') {
      // Store token and user data
      localStorage.setItem('token', response.token);
      localStorage.setItem('role', response.role);
      
      const user: User = {
        id: response.userId,
        email: response.email,
        firstName: response.firstName,
        lastName: response.lastName,
        userType: response.role
      };
      
      localStorage.setItem('user', JSON.stringify(user));
      this.currentUserSubject.next(user);
    }
    
    // Redirect based on role
    this.redirectAfterLogin(response.role);
  }

  logout(): void {
    // Check if we're in the browser environment
    if (typeof window !== 'undefined' && typeof localStorage !== 'undefined') {
      // Clear stored data
      localStorage.removeItem('token');
      localStorage.removeItem('user');
      localStorage.removeItem('role');
    }
    
    this.currentUserSubject.next(null);
    
    // Redirect to home
    this.router.navigate(['/']);
  }

  getCurrentUser(): User | null {
    return this.currentUserSubject.value;
  }

  isLoggedIn(): boolean {
    return this.getCurrentUser() !== null;
  }

  getToken(): string | null {
    // Check if we're in the browser environment
    if (typeof window === 'undefined' || typeof localStorage === 'undefined') {
      return null;
    }
    
    return localStorage.getItem('token');
  }

  getUserProfile(): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/auth/profile`);
  }

  updateProfile(user: User): Observable<User> {
    return this.http.put<User>(`${this.apiUrl}/auth/profile`, user);
  }

  private loadUserFromStorage(): void {
    // Check if we're in the browser environment
    if (typeof window === 'undefined' || typeof localStorage === 'undefined') {
      return;
    }
    
    const userStr = localStorage.getItem('user');
    if (userStr) {
      try {
        const user = JSON.parse(userStr);
        this.currentUserSubject.next(user);
      } catch (error) {
        console.error('Error parsing user from storage:', error);
        this.logout();
      }
    }
  }

  private redirectAfterLogin(userType: 'User' | 'Owner'): void {
    if (userType === 'User') {
      this.router.navigate(['/user-dashboard']);
    } else if (userType === 'Owner') {
      this.router.navigate(['/owner-dashboard']);
    }
  }
}
