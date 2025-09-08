import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { BookingService } from '../../services/booking';
import { AuthService } from '../../services/auth.service';

interface Booking {
  id: number;
  propertyId: number;
  propertyName: string;
  propertyImage: string;
  checkInDate: string;
  checkOutDate: string;
  totalPrice: number;
  status: 'Pending' | 'Confirmed' | 'Cancelled' | 'Completed';
  createdAt: string;
  propertyAddress: string;
}

@Component({
  selector: 'app-bookings',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './bookings.html',
  styleUrls: ['./bookings.css']
})
export class BookingsComponent implements OnInit {
  bookings: Booking[] = [];
  loading = false;
  error = '';
  filterStatus = 'all';

  constructor(
    private http: HttpClient,
    public router: Router,
    private bookingService: BookingService,
    private authService: AuthService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.loadBookings();
  }

  loadBookings() {
    this.loading = true;
    this.error = '';
    
    console.log('Loading bookings...');
    console.log('User logged in:', this.authService.isLoggedIn());
    console.log('Current user:', this.authService.getCurrentUser());
    
    // Try the simple endpoint first (no authentication required)
    this.bookingService.getSimpleBookings().subscribe({
      next: (bookings) => {
        console.log('Bookings loaded successfully:', bookings);
        this.bookings = bookings.map(booking => ({
          id: booking.id,
          propertyId: booking.propertyId,
          propertyName: booking.propertyName,
          propertyImage: this.getValidImageUrl(booking.propertyImage),
          checkInDate: booking.checkInDate,
          checkOutDate: booking.checkOutDate,
          totalPrice: booking.totalPrice,
          status: this.mapStatus(booking.status),
          createdAt: booking.createdAt,
          propertyAddress: booking.propertyAddress
        }));
        this.loading = false;
        this.cdr.detectChanges();
        console.log('Mapped bookings:', this.bookings);
      },
      error: (error) => {
        console.error('Error loading bookings:', error);
        this.error = 'Failed to load bookings. Please try again.';
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  private getValidImageUrl(imageUrl: string): string {
    // If it's already a full URL (http/https), return as is
    if (imageUrl.startsWith('http://') || imageUrl.startsWith('https://')) {
      return imageUrl;
    }
    
    // If it's the placeholder path, use a real placeholder image
    if (imageUrl === '/assets/images/placeholder.jpg') {
      return 'https://picsum.photos/400/300?random=placeholder';
    }
    
    // If it's a relative URL starting with /, prepend the API base URL
    if (imageUrl.startsWith('/')) {
      return `http://localhost:5158${imageUrl}`;
    }
    
    // If it's a relative URL without /, assume it's in the uploads folder
    return `http://localhost:5158/uploads/properties/${imageUrl}`;
  }

  private mapStatus(status: string): 'Pending' | 'Confirmed' | 'Cancelled' | 'Completed' {
    switch (status?.toLowerCase()) {
      case 'pending':
        return 'Pending';
      case 'confirmed':
        return 'Confirmed';
      case 'cancelled':
        return 'Cancelled';
      case 'completed':
        return 'Completed';
      default:
        return 'Pending';
    }
  }

  get filteredBookings(): Booking[] {
    if (this.filterStatus === 'all') {
      return this.bookings;
    }
    return this.bookings.filter(booking => booking.status.toLowerCase() === this.filterStatus.toLowerCase());
  }

  getStatusClass(status: string): string {
    switch (status.toLowerCase()) {
      case 'confirmed':
        return 'status-confirmed';
      case 'pending':
        return 'status-pending';
      case 'cancelled':
        return 'status-cancelled';
      case 'completed':
        return 'status-completed';
      default:
        return 'status-default';
    }
  }

  formatDate(dateString: string): string {
    return new Date(dateString).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    });
  }

  formatPrice(price: number): string {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD'
    }).format(price);
  }

  cancelBooking(bookingId: number) {
    if (confirm('Are you sure you want to cancel this booking?')) {
      this.loading = true;
      
      this.http.put(`http://localhost:5158/api/bookings/${bookingId}/cancel`, {}).subscribe({
        next: () => {
          this.loadBookings(); // Reload bookings
        },
        error: (error) => {
          console.error('Error cancelling booking:', error);
          this.error = 'Failed to cancel booking. Please try again.';
          this.loading = false;
        }
      });
    }
  }

  viewProperty(propertyId: number) {
    this.router.navigate(['/properties', propertyId]);
  }

  goBack() {
    this.router.navigate(['/user-dashboard']);
  }

  onFilterChange() {
    // Filter is applied via getter
  }

  // Helper method to get the full image URL
  getImageUrl(imageUrl: string): string {
    return this.getValidImageUrl(imageUrl);
  }

  // Handle image loading errors
  onImageError(event: any) {
    // Use a data URL for a simple placeholder to avoid infinite loops
    event.target.src = 'data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMzAwIiBoZWlnaHQ9IjIwMCIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZjBmMGYwIi8+PHRleHQgeD0iNTAlIiB5PSI1MCUiIGZvbnQtZmFtaWx5PSJBcmlhbCwgc2Fucy1zZXJpZiIgZm9udC1zaXplPSIxNCIgZmlsbD0iIzk5OSIgdGV4dC1hbmNob3I9Im1pZGRsZSIgZHk9Ii4zZW0iPk5vIEltYWdlPC90ZXh0Pjwvc3ZnPg==';
  }
}