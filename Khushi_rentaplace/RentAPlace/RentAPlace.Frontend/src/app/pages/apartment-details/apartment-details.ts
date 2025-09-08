import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../services/auth.service';
import { BookingService, CreateBookingRequest } from '../../services/booking.service';

interface Property {
  id: number;
  name: string;
  description: string;
  address: string;
  city: string;
  state: string;
  zipCode: string;
  price: number;
  propertyType: string;
  bedrooms: number;
  bathrooms: number;
  area: number;
  maxGuests: number;
  images: string[];
  features: string[];
  isAvailable: boolean;
  owner: {
    id: string;
    firstName: string;
    lastName: string;
    email: string;
  };
}

interface BookingConfirmation {
  bookingId: number;
  propertyName: string;
  propertyAddress: string;
  checkInDate: string;
  checkOutDate: string;
  totalPrice: number;
  status: string;
  confirmationNumber: string;
}

@Component({
  selector: 'app-apartment-details',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './apartment-details.html',
  styleUrls: ['./apartment-details.css']
})
export class ApartmentDetailsComponent implements OnInit {
  property: Property | null = null;
  loading = false;
  error = '';
  bookingLoading = false;
  currentImageIndex = 0;
  checkInDate = '';
  checkOutDate = '';
  guests = 1;
  showBookingForm = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private http: HttpClient,
    private authService: AuthService,
    private bookingService: BookingService
  ) {}

  ngOnInit() {
    const propertyId = this.route.snapshot.paramMap.get('id');
    if (propertyId) {
      this.loadProperty(parseInt(propertyId));
    }

    // Get dates from query params
    this.route.queryParams.subscribe(params => {
      this.checkInDate = params['checkIn'] || '';
      this.checkOutDate = params['checkOut'] || '';
      this.guests = params['guests'] ? parseInt(params['guests']) : 1;
    });
  }

  loadProperty(id: number) {
    this.loading = true;
    this.error = '';

    this.http.get<Property>(`http://localhost:5158/api/properties/${id}`).subscribe({
      next: (property) => {
        this.property = property;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading property:', error);
        this.error = 'Failed to load property details. Please try again.';
        this.loading = false;
      }
    });
  }

  onBookNow() {
    if (!this.authService.isLoggedIn()) {
      // Store current URL and property ID to return after login
      const returnUrl = this.router.url;
      this.router.navigate(['/login'], { 
        queryParams: { returnUrl, propertyId: this.property?.id } 
      });
      return;
    }

    if (!this.checkInDate || !this.checkOutDate) {
      alert('Please select check-in and check-out dates first.');
      return;
    }

    this.showBookingForm = true;
  }

  onConfirmBooking() {
    if (!this.property) return;

    this.bookingLoading = true;

    const bookingData: CreateBookingRequest = {
      propertyId: this.property.id,
      checkInDate: this.checkInDate,
      checkOutDate: this.checkOutDate
    };

    this.bookingService.createBooking(bookingData).subscribe({
      next: (confirmation) => {
        this.bookingLoading = false;
        this.showBookingConfirmation(confirmation);
      },
      error: (error) => {
        this.bookingLoading = false;
        console.error('Error creating booking:', error);
        
        let errorMessage = 'Failed to create booking. Please try again.';
        if (error.error?.message) {
          errorMessage = error.error.message;
        }
        
        alert(errorMessage);
      }
    });
  }

  showBookingConfirmation(confirmation: BookingConfirmation) {
    const message = `
Booking Confirmed! 🎉

Confirmation Number: ${confirmation.confirmationNumber}
Property: ${confirmation.propertyName}
Address: ${confirmation.propertyAddress}
Check-in: ${new Date(confirmation.checkInDate).toLocaleDateString()}
Check-out: ${new Date(confirmation.checkOutDate).toLocaleDateString()}
Total Price: ${this.formatPrice(confirmation.totalPrice)}
Status: ${confirmation.status}

Thank you for choosing RentAPlace!
    `;
    
    alert(message);
    this.router.navigate(['/bookings']);
  }

  nextImage() {
    if (this.property && this.property.images.length > 0) {
      this.currentImageIndex = (this.currentImageIndex + 1) % this.property.images.length;
    }
  }

  previousImage() {
    if (this.property && this.property.images.length > 0) {
      this.currentImageIndex = this.currentImageIndex === 0 
        ? this.property.images.length - 1 
        : this.currentImageIndex - 1;
    }
  }

  goToImage(index: number) {
    this.currentImageIndex = index;
  }

  formatPrice(price: number): string {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD'
    }).format(price);
  }

  getPropertyImage(): string {
    if (this.property && this.property.images.length > 0) {
      return this.property.images[this.currentImageIndex];
    }
    return '/assets/placeholder-property.jpg';
  }

  onImageError(event: any) {
    event.target.src = '/assets/placeholder-property.jpg';
  }

  goBack() {
    this.router.navigate(['/search'], {
      queryParams: {
        location: this.route.snapshot.queryParams['location'],
        checkIn: this.checkInDate,
        checkOut: this.checkOutDate,
        guests: this.guests
      }
    });
  }

  calculateTotalPrice(): number {
    if (!this.property || !this.checkInDate || !this.checkOutDate) return 0;
    
    const checkIn = new Date(this.checkInDate);
    const checkOut = new Date(this.checkOutDate);
    const nights = Math.ceil((checkOut.getTime() - checkIn.getTime()) / (1000 * 60 * 60 * 24));
    
    return this.property.price * nights;
  }

  getAvailableDates(): string[] {
    // In a real app, you'd fetch available dates from the API
    // For now, return a simple array of next 30 days
    const dates: string[] = [];
    const today = new Date();
    
    for (let i = 1; i <= 30; i++) {
      const date = new Date(today);
      date.setDate(today.getDate() + i);
      dates.push(date.toISOString().split('T')[0]);
    }
    
    return dates;
  }
}
