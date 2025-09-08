import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface CreateBookingRequest {
  propertyId: number;
  checkInDate: string;
  checkOutDate: string;
}

export interface BookingConfirmation {
  bookingId: number;
  propertyName: string;
  propertyAddress: string;
  checkInDate: string;
  checkOutDate: string;
  totalPrice: number;
  status: string;
  confirmationNumber: string;
}

export interface Booking {
  id: number;
  propertyId: number;
  propertyName: string;
  propertyAddress: string;
  propertyImage: string;
  userId: string;
  userName: string;
  userEmail: string;
  checkInDate: string;
  checkOutDate: string;
  status: string;
  totalPrice: number;
  createdAt: string;
  propertyTypeId: number;
  propertyType: string;
  bedrooms: number;
  bathrooms: number;
  area: number;
}

@Injectable({
  providedIn: 'root'
})
export class BookingService {
  private apiUrl = 'http://localhost:5158/api';

  constructor(private http: HttpClient) {}

  createBooking(bookingData: CreateBookingRequest): Observable<BookingConfirmation> {
    return this.http.post<BookingConfirmation>(`${this.apiUrl}/bookings`, bookingData);
  }

  getMyBookings(): Observable<Booking[]> {
    return this.http.get<Booking[]>(`${this.apiUrl}/bookings/my`);
  }

  getOwnerBookings(): Observable<Booking[]> {
    return this.http.get<Booking[]>(`${this.apiUrl}/bookings/owner`);
  }

  confirmBooking(bookingId: number): Observable<{ message: string }> {
    return this.http.put<{ message: string }>(`${this.apiUrl}/bookings/${bookingId}/confirm`, {});
  }

  cancelBooking(bookingId: number): Observable<{ message: string }> {
    return this.http.put<{ message: string }>(`${this.apiUrl}/bookings/${bookingId}/cancel`, {});
  }
}
