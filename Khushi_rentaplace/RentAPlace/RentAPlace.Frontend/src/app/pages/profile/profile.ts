import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './profile.html',
  styleUrls: ['./profile.css']
})
export class ProfileComponent implements OnInit {
  profileForm!: FormGroup;
  user: any = null;
  loading = false;
  editing = false;
  error = '';

  constructor(
    private authService: AuthService,
    private fb: FormBuilder,
    private router: Router
  ) {
    this.initializeForm();
  }

  ngOnInit() {
    this.loadUserProfile();
  }

  initializeForm() {
    this.profileForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.minLength(2)]],
      lastName: ['', [Validators.required, Validators.minLength(2)]],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', [Validators.required, Validators.pattern(/^\+?[\d\s\-\(\)]+$/)]]
    });
  }

  loadUserProfile() {
    this.loading = true;
    this.error = '';
    
    this.authService.getUserProfile().subscribe({
      next: (user) => {
        this.user = user;
        this.profileForm.patchValue({
          firstName: user.firstName,
          lastName: user.lastName,
          email: user.email,
          phoneNumber: user.phoneNumber
        });
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading user profile:', error);
        this.error = 'Failed to load profile. Please try again.';
        this.loading = false;
      }
    });
  }

  toggleEdit() {
    this.editing = !this.editing;
    if (!this.editing) {
      // Reset form to original values
      this.profileForm.patchValue({
        firstName: this.user.firstName,
        lastName: this.user.lastName,
        email: this.user.email,
        phoneNumber: this.user.phoneNumber
      });
    }
  }

  onSubmit() {
    if (this.profileForm.valid) {
      this.loading = true;
      this.error = '';
      
      const updatedUser = {
        ...this.user,
        ...this.profileForm.value
      };

      this.authService.updateProfile(updatedUser).subscribe({
        next: (response) => {
          this.user = response;
          this.editing = false;
          this.loading = false;
          console.log('Profile updated successfully');
        },
        error: (error) => {
          console.error('Error updating profile:', error);
          this.error = 'Failed to update profile. Please try again.';
          this.loading = false;
        }
      });
    } else {
      this.markFormGroupTouched();
    }
  }

  cancelEdit() {
    this.editing = false;
    this.profileForm.patchValue({
      firstName: this.user.firstName,
      lastName: this.user.lastName,
      email: this.user.email,
      phoneNumber: this.user.phoneNumber
    });
  }

  getFieldError(fieldName: string): string {
    const field = this.profileForm.get(fieldName);
    if (field?.errors && field.touched) {
      if (field.errors['required']) {
        return `${this.getFieldLabel(fieldName)} is required`;
      }
      if (field.errors['email']) {
        return 'Please enter a valid email address';
      }
      if (field.errors['minlength']) {
        return `${this.getFieldLabel(fieldName)} must be at least ${field.errors['minlength'].requiredLength} characters`;
      }
      if (field.errors['pattern']) {
        return 'Please enter a valid phone number';
      }
    }
    return '';
  }

  private getFieldLabel(fieldName: string): string {
    const labels: { [key: string]: string } = {
      firstName: 'First Name',
      lastName: 'Last Name',
      email: 'Email',
      phoneNumber: 'Phone Number'
    };
    return labels[fieldName] || fieldName;
  }

  private markFormGroupTouched() {
    Object.keys(this.profileForm.controls).forEach(key => {
      const control = this.profileForm.get(key);
      control?.markAsTouched();
    });
  }

  goBack() {
    this.router.navigate(['/user-dashboard']);
  }
}