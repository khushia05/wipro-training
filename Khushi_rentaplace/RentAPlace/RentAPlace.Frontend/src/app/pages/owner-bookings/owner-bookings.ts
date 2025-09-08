import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

interface OwnerBooking {
  id: number;
  propertyId: number;
  propertyName: string;
  propertyImage: string;
  customerName: string;
  customerEmail: string;
  customerPhone: string;
  checkInDate: string;
  checkOutDate: string;
  totalPrice: number;
  status: 'Pending' | 'Confirmed' | 'Cancelled' | 'Completed';
  createdAt: string;
  propertyAddress: string;
}

@Component({
  selector: 'app-owner-bookings',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './owner-bookings.html',
  styleUrls: ['./owner-bookings.css']
})
export class OwnerBookingsComponent implements OnInit {
  bookings: OwnerBooking[] = [];
  loading = false;
  error = '';
  filterStatus = 'all';

  constructor(
    private http: HttpClient,
    public router: Router
  ) {}

  ngOnInit() {
    this.loadOwnerBookings();
  }

  loadOwnerBookings() {
    this.loading = true;
    this.error = '';
    
    this.http.get<OwnerBooking[]>('http://localhost:5158/api/bookings/owner').subscribe({
      next: (bookings) => {
        this.bookings = bookings;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading owner bookings:', error);
        this.error = 'Failed to load property bookings. Please try again.';
        this.loading = false;
      }
    });
  }

  get filteredBookings(): OwnerBooking[] {
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

  updateBookingStatus(bookingId: number, newStatus: 'Confirmed' | 'Cancelled') {
    const action = newStatus === 'Confirmed' ? 'confirm' : 'cancel';
    if (confirm(`Are you sure you want to ${action} this booking?`)) {
      this.loading = true;
      
      this.http.put(`http://localhost:5158/api/bookings/${bookingId}/${action}`, {}).subscribe({
        next: () => {
          this.loadOwnerBookings(); // Reload bookings
        },
        error: (error) => {
          console.error(`Error ${action}ing booking:`, error);
          this.error = `Failed to ${action} booking. Please try again.`;
          this.loading = false;
        }
      });
    }
  }

  viewProperty(propertyId: number) {
    this.router.navigate(['/properties', propertyId]);
  }

  goBack() {
    this.router.navigate(['/owner-dashboard']);
  }

  onFilterChange() {
    // Filter is applied via getter
  }
}
