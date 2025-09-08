import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../services/auth.service';
import { BookingService, Booking } from '../../services/booking.service';

interface OwnerProperty {
  id: number;
  name: string;
  description: string;
  address: string;
  city: string;
  state: string;
  price: number;
  propertyType: string;
  bedrooms: number;
  bathrooms: number;
  area: number;
  isAvailable: boolean;
  images: string[];
  totalBookings: number;
  totalRevenue: number;
}

@Component({
  selector: 'app-owner-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './owner-dashboard.html',
  styleUrls: ['./owner-dashboard.css']
})
export class OwnerDashboardComponent implements OnInit {
  properties: OwnerProperty[] = [];
  bookings: Booking[] = [];
  loading = false;
  error = '';
  currentUser: any = null;
  stats = {
    totalProperties: 0,
    totalBookings: 0,
    totalRevenue: 0,
    availableProperties: 0
  };

  constructor(
    private router: Router,
    private http: HttpClient,
    private authService: AuthService,
    private bookingService: BookingService
  ) {}

  ngOnInit() {
    this.currentUser = this.authService.getCurrentUser();
    if (!this.currentUser || this.currentUser.userType !== 'Owner') {
      this.router.navigate(['/login']);
      return;
    }

    this.loadDashboardData();
  }

  loadDashboardData() {
    this.loading = true;
    this.error = '';

    // Load properties and bookings in parallel
    Promise.all([
      this.loadMyProperties(),
      this.loadOwnerBookings()
    ]).finally(() => {
      this.loading = false;
    });
  }

  loadMyProperties() {
    return new Promise<void>((resolve, reject) => {
      this.http.get<OwnerProperty[]>(`http://localhost:5158/api/properties/owner/${this.currentUser.id}`).subscribe({
        next: (properties) => {
          this.properties = properties;
          this.calculateStats();
          resolve();
        },
        error: (error) => {
          console.error('Error loading properties:', error);
          this.error = 'Failed to load properties. Please try again.';
          reject(error);
        }
      });
    });
  }

  loadOwnerBookings() {
    return new Promise<void>((resolve, reject) => {
      this.bookingService.getOwnerBookings().subscribe({
        next: (bookings) => {
          this.bookings = bookings;
          this.calculateStats();
          resolve();
        },
        error: (error) => {
          console.error('Error loading bookings:', error);
          this.error = 'Failed to load bookings. Please try again.';
          reject(error);
        }
      });
    });
  }

  calculateStats() {
    this.stats.totalProperties = this.properties.length;
    this.stats.availableProperties = this.properties.filter(p => p.isAvailable).length;
    this.stats.totalBookings = this.bookings.length;
    this.stats.totalRevenue = this.bookings.reduce((sum, booking) => sum + booking.totalPrice, 0);
  }

  onAddProperty() {
    this.router.navigate(['/add-property']);
  }

  onManageProperties() {
    this.router.navigate(['/my-properties']);
  }

  onViewBookings() {
    this.router.navigate(['/owner-bookings']);
  }

  onViewAnalytics() {
    alert('Analytics feature coming soon!');
  }

  onPropertyClick(property: OwnerProperty) {
    this.router.navigate(['/property', property.id]);
  }

  onBookingClick(booking: Booking) {
    this.router.navigate(['/owner-bookings']);
  }

  formatPrice(price: number): string {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD'
    }).format(price);
  }

  getPropertyImage(property: OwnerProperty): string {
    return property.images && property.images.length > 0 
      ? property.images[0] 
      : '/assets/placeholder-property.jpg';
  }

  onImageError(event: any) {
    event.target.src = '/assets/placeholder-property.jpg';
  }

  getStatusColor(status: string): string {
    switch (status.toLowerCase()) {
      case 'confirmed':
        return '#48bb78';
      case 'pending':
        return '#ed8936';
      case 'cancelled':
        return '#e53e3e';
      case 'completed':
        return '#4299e1';
      default:
        return '#6b7280';
    }
  }

  getRecentBookings(): Booking[] {
    return this.bookings
      .sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime())
      .slice(0, 5);
  }

  getTopProperties(): OwnerProperty[] {
    return this.properties
      .sort((a, b) => b.totalBookings - a.totalBookings)
      .slice(0, 3);
  }
}