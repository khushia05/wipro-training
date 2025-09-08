import { Property } from './property.interface';
import { User } from './user.interface';

export interface Reservation {
  id: string;
  propertyId: string;
  property: Property;
  renterId: string;
  renter: User;
  checkInDate: string;
  checkOutDate: string;
  totalNights: number;
  totalAmount: number;
  status: 'Pending' | 'Confirmed' | 'Cancelled' | 'Completed';
  specialRequests?: string;
  createdAt: string;
}

export interface ReservationCreate {
  propertyId: string;
  checkInDate: string;
  checkOutDate: string;
  specialRequests?: string;
}

export interface ReservationUpdate {
  checkInDate?: string;
  checkOutDate?: string;
  status?: string;
  specialRequests?: string;
}
