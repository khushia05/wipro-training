export interface User {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber?: string;
  userType: 'Renter' | 'Owner';
  isActive: boolean;
  createdAt: string;
}

export interface UserRegistration {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  phoneNumber?: string;
  userType: 'Renter' | 'Owner';
}

export interface UserLogin {
  email: string;
  password: string;
}

export interface UserUpdate {
  firstName?: string;
  lastName?: string;
  phoneNumber?: string;
}
