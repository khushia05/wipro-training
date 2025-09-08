import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService, User } from '../../services/auth.service';

@Component({
  selector: 'app-user-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './user-dashboard.html',
  styleUrl: './user-dashboard.css'
})
export class UserDashboardComponent implements OnInit {
  user: User | null = null;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit() {
    // Check if user is logged in and is a User
    if (!this.authService.isLoggedIn()) {
      this.router.navigate(['/login']);
      return;
    }

    this.user = this.authService.getCurrentUser();
    if (this.user?.userType !== 'User') {
      this.router.navigate(['/']);
      return;
    }
  }

  onSearchProperties() {
    this.router.navigate(['/']);
  }

  onViewBookings() {
    // TODO: Navigate to bookings page
    console.log('View bookings clicked');
  }

  onViewFavorites() {
    // TODO: Navigate to favorites page
    console.log('View favorites clicked');
  }

  onViewProfile() {
    this.router.navigate(['/profile']);
  }
}
