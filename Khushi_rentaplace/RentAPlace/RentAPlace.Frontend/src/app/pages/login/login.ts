import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService, LoginRequest } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  isLoading = false;
  error = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  ngOnInit() {
    // Redirect if already logged in
    if (this.authService.isLoggedIn()) {
      this.redirectAfterLogin();
    }
  }

  onSubmit() {
    if (this.loginForm.valid) {
      this.isLoading = true;
      this.error = '';

      const loginRequest: LoginRequest = {
        email: this.loginForm.value.email,
        password: this.loginForm.value.password
      };

      this.authService.login(loginRequest).subscribe({
        next: (response) => {
          this.isLoading = false;
          this.redirectAfterLogin();
        },
        error: (error) => {
          this.isLoading = false;
          console.error('Login error:', error);
          
          if (error.status === 401) {
            this.error = 'Invalid email or password';
          } else if (error.status === 400) {
            this.error = 'Please check your input and try again';
          } else {
            this.error = 'Login failed. Please try again later.';
          }
        }
      });
    } else {
      this.markFormGroupTouched();
    }
  }

  // User type selection removed - role is determined by backend

  private markFormGroupTouched() {
    Object.keys(this.loginForm.controls).forEach(key => {
      const control = this.loginForm.get(key);
      control?.markAsTouched();
    });
  }

  getFieldError(fieldName: string): string {
    const field = this.loginForm.get(fieldName);
    if (field?.errors && field.touched) {
      if (field.errors['required']) {
        return `${fieldName} is required`;
      }
      if (field.errors['email']) {
        return 'Please enter a valid email address';
      }
      if (field.errors['minlength']) {
        return 'Password must be at least 6 characters';
      }
    }
    return '';
  }

  onRegisterClick() {
    this.router.navigate(['/register']);
  }

  private redirectAfterLogin() {
    const returnUrl = this.route.snapshot.queryParams['returnUrl'];
    const propertyId = this.route.snapshot.queryParams['propertyId'];
    
    if (returnUrl) {
      this.router.navigateByUrl(returnUrl);
    } else {
      const user = this.authService.getCurrentUser();
      if (user) {
        this.router.navigate([user.userType === 'User' ? '/user-dashboard' : '/owner-dashboard']);
      }
    }
  }
}