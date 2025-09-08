import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
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
  images: string[];
  features: string[];
  isAvailable: boolean;
}

interface SearchParams {
  location?: string;
  checkIn?: string;
  checkOut?: string;
  guests?: number;
}

@Component({
  selector: 'app-search-results',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './search-results.html',
  styleUrls: ['./search-results.css']
})
export class SearchResultsComponent implements OnInit {
  properties: Property[] = [];
  loading = false;
  error = '';
  searchParams: SearchParams = {};
  bookingLoading: { [key: number]: boolean } = {};

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private http: HttpClient,
    private authService: AuthService,
    private bookingService: BookingService
  ) {}

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.searchParams = {
        location: params['location'],
        checkIn: params['checkIn'],
        checkOut: params['checkOut'],
        guests: params['guests'] ? parseInt(params['guests']) : undefined
      };
      this.searchProperties();
    });
  }

  searchProperties() {
    this.loading = true;
    this.error = '';

    // Build query parameters for the API call
    let queryParams = new URLSearchParams();
    
    if (this.searchParams.location) {
      queryParams.set('search', this.searchParams.location);
    }
    
    if (this.searchParams.guests) {
      queryParams.set('minGuests', this.searchParams.guests.toString());
    }

    // Always show only available properties
    queryParams.set('isAvailable', 'true');

    const apiUrl = `http://localhost:5158/api/properties?${queryParams.toString()}`;
    console.log('Search API URL:', apiUrl);

    this.http.get<Property[]>(apiUrl).subscribe({
      next: (properties) => {
        this.properties = properties;
        this.loading = false;
        console.log('Search results:', properties.length, 'properties found');
      },
      error: (error) => {
        console.error('Error searching properties:', error);
        this.error = 'Failed to search properties. Please try again.';
        this.loading = false;
      }
    });
  }

  viewPropertyDetails(property: Property) {
    this.router.navigate(['/property', property.id], {
      queryParams: {
        location: this.searchParams.location,
        checkIn: this.searchParams.checkIn,
        checkOut: this.searchParams.checkOut,
        guests: this.searchParams.guests
      }
    });
  }

  onBookNow(property: Property) {
    if (!this.authService.isLoggedIn()) {
      // Store the search params and property ID to return after login
      const returnUrl = this.router.url;
      this.router.navigate(['/login'], { 
        queryParams: { returnUrl, propertyId: property.id } 
      });
      return;
    }

    if (!this.searchParams.checkIn || !this.searchParams.checkOut) {
      alert('Please select check-in and check-out dates first.');
      return;
    }

    this.bookingLoading[property.id] = true;

    const bookingData: CreateBookingRequest = {
      propertyId: property.id,
      checkInDate: this.searchParams.checkIn!,
      checkOutDate: this.searchParams.checkOut!
    };

    this.bookingService.createBooking(bookingData).subscribe({
      next: (confirmation) => {
        this.bookingLoading[property.id] = false;
        alert(`Booking confirmed! Confirmation number: ${confirmation.confirmationNumber}`);
        this.router.navigate(['/bookings']);
      },
      error: (error) => {
        this.bookingLoading[property.id] = false;
        console.error('Error creating booking:', error);
        
        let errorMessage = 'Failed to create booking. Please try again.';
        if (error.error?.message) {
          errorMessage = error.error.message;
        }
        
        alert(errorMessage);
      }
    });
  }

  formatPrice(price: number): string {
    return new Intl.NumberFormat('en-IN', {
      style: 'currency',
      currency: 'INR'
    }).format(price);
  }

  getPropertyImage(property: Property): string {
    return property.images && property.images.length > 0 
      ? property.images[0] 
      : '/assets/placeholder-property.jpg';
  }

  onImageError(event: any) {
    event.target.src = '/assets/placeholder-property.jpg';
  }

  clearSearch() {
    this.router.navigate(['/']);
  }

  goBack() {
    this.router.navigate(['/']);
  }
}
