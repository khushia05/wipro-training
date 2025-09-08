import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService, RegisterRequest } from '../../services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  isLoading = false;
  error = '';
  userTypes = [
    { value: 'User', label: 'User', description: 'Find and book properties' },
    { value: 'Owner', label: 'Owner', description: 'List and manage properties' }
  ];

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.registerForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.minLength(2)]],
      lastName: ['', [Validators.required, Validators.minLength(2)]],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', [Validators.required, Validators.pattern(/^\+?[\d\s\-\(\)]+$/)]],
      password: ['', [Validators.required, Validators.minLength(8), this.passwordStrengthValidator]],
      confirmPassword: ['', [Validators.required]],
      userType: ['User', [Validators.required]]
    }, { validators: this.passwordMatchValidator });
  }

  ngOnInit() {
    // Redirect if already logged in
    if (this.authService.isLoggedIn()) {
      const user = this.authService.getCurrentUser();
      if (user) {
        this.redirectAfterLogin(user.userType);
      }
    }
  }

  onSubmit() {
    console.log('Form submitted. Valid:', this.registerForm.valid);
    console.log('Form errors:', this.registerForm.errors);
    console.log('Form value:', this.registerForm.value);
    
    if (this.registerForm.valid) {
      this.isLoading = true;
      this.error = '';

      const registerRequest: RegisterRequest = {
        firstName: this.registerForm.value.firstName,
        lastName: this.registerForm.value.lastName,
        email: this.registerForm.value.email,
        phoneNumber: this.registerForm.value.phoneNumber,
        password: this.registerForm.value.password,
        userType: this.registerForm.value.userType
      };
      
      console.log('Sending registration request:', registerRequest);

      this.authService.register(registerRequest).subscribe({
        next: (response) => {
          console.log('Registration successful:', response);
          this.isLoading = false;
          // Navigation is handled in the auth service
        },
        error: (err) => {
          console.error('Registration error:', err);
          this.isLoading = false;
          this.error = err.error?.message || err.message || 'Registration failed. Please try again.';
        }
      });
    } else {
      this.markFormGroupTouched();
    }
  }

  onUserTypeChange(userType: string) {
    this.registerForm.patchValue({ userType });
  }

  onLoginClick() {
    this.router.navigate(['/login']);
  }

  getFieldError(fieldName: string): string {
    const field = this.registerForm.get(fieldName);
    if (field?.errors && field.touched) {
      if (field.errors['required']) {
        return `${this.getFieldLabel(fieldName)} is required`;
      }
      if (field.errors['email']) {
        return 'Please enter a valid email address';
      }
      if (field.errors['minlength']) {
        const requiredLength = field.errors['minlength'].requiredLength;
        return `${this.getFieldLabel(fieldName)} must be at least ${requiredLength} characters`;
      }
      if (field.errors['pattern']) {
        return 'Please enter a valid phone number';
      }
      if (field.errors['passwordStrength']) {
        return 'Password must contain uppercase, lowercase, digit, and special character';
      }
    }
    return '';
  }

  getPasswordStrength(): string {
    const password = this.registerForm.get('password')?.value || '';
    if (!password) return '';
    
    const hasUpper = /[A-Z]/.test(password);
    const hasLower = /[a-z]/.test(password);
    const hasDigit = /\d/.test(password);
    const hasSpecial = /[!@#$%^&*(),.?":{}|<>]/.test(password);
    const isLongEnough = password.length >= 8;
    
    const strength = [hasUpper, hasLower, hasDigit, hasSpecial, isLongEnough].filter(Boolean).length;
    
    if (strength < 3) return 'weak';
    if (strength < 5) return 'medium';
    return 'strong';
  }

  getPasswordStrengthColor(): string {
    const strength = this.getPasswordStrength();
    switch (strength) {
      case 'weak': return '#e53e3e';
      case 'medium': return '#ed8936';
      case 'strong': return '#38a169';
      default: return '#a0aec0';
    }
  }

  hasUppercase(): boolean {
    const password = this.registerForm.get('password')?.value || '';
    return /[A-Z]/.test(password);
  }

  hasLowercase(): boolean {
    const password = this.registerForm.get('password')?.value || '';
    return /[a-z]/.test(password);
  }

  hasNumber(): boolean {
    const password = this.registerForm.get('password')?.value || '';
    return /\d/.test(password);
  }

  hasSpecialCharacter(): boolean {
    const password = this.registerForm.get('password')?.value || '';
    return /[!@#$%^&*(),.?":{}|<>]/.test(password);
  }

  private getFieldLabel(fieldName: string): string {
    const labels: { [key: string]: string } = {
      firstName: 'First Name',
      lastName: 'Last Name',
      email: 'Email',
      phoneNumber: 'Phone Number',
      password: 'Password',
      confirmPassword: 'Confirm Password',
      userType: 'User Type'
    };
    return labels[fieldName] || fieldName;
  }

  private passwordStrengthValidator(control: any) {
    const password = control.value;
    if (!password) return null;
    
    const hasUpper = /[A-Z]/.test(password);
    const hasLower = /[a-z]/.test(password);
    const hasDigit = /\d/.test(password);
    const hasSpecial = /[!@#$%^&*(),.?":{}|<>]/.test(password);
    const isLongEnough = password.length >= 8;
    
    if (!hasUpper || !hasLower || !hasDigit || !hasSpecial || !isLongEnough) {
      return { passwordStrength: true };
    }
    
    return null;
  }

  private passwordMatchValidator(form: FormGroup) {
    const password = form.get('password');
    const confirmPassword = form.get('confirmPassword');
    
    if (password && confirmPassword && password.value !== confirmPassword.value) {
      confirmPassword.setErrors({ passwordMismatch: true });
      return { passwordMismatch: true };
    }
    
    return null;
  }

  private markFormGroupTouched() {
    Object.keys(this.registerForm.controls).forEach(key => {
      const control = this.registerForm.get(key);
      control?.markAsTouched();
    });
  }

  private redirectAfterLogin(userType: string) {
    if (userType === 'User') {
      this.router.navigate(['/user-dashboard']);
    } else if (userType === 'Owner') {
      this.router.navigate(['/owner-dashboard']);
    } else {
      this.router.navigate(['/']);
    }
  }
}