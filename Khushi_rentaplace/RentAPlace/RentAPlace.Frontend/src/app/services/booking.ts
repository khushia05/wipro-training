import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface CreateBookingRequest {
  propertyId: number;
  checkInDate?: Date;
  checkOutDate?: Date;
}

export interface BookingConfirmation {
  bookingId: number;
  propertyName: string;
  propertyAddress: string;
  checkInDate: Date;
  checkOutDate: Date;
  totalPrice: number;
  status: string;
  confirmationNumber: string;
}

@Injectable({
  providedIn: 'root'
})
export class BookingService {
  private apiUrl = 'http://localhost:5158/api';

  constructor(private http: HttpClient) {}

  /**
   * Create a simple booking for testing (no authentication required)
   */
  createSimpleBooking(request: CreateBookingRequest): Observable<BookingConfirmation> {
    return this.http.post<BookingConfirmation>(`${this.apiUrl}/bookings/simple`, request);
  }

  /**
   * Create a booking (requires authentication)
   */
  createBooking(request: CreateBookingRequest): Observable<BookingConfirmation> {
    return this.http.post<BookingConfirmation>(`${this.apiUrl}/bookings`, request);
  }

  /**
   * Get user's bookings (requires authentication)
   */
  getMyBookings(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/bookings/my`);
  }

  /**
   * Get simple bookings for testing (no authentication required)
   */
  getSimpleBookings(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/bookings/simple`);
  }

  /**
   * Get owner's bookings (requires authentication)
   */
  getOwnerBookings(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/bookings/owner`);
  }
}
