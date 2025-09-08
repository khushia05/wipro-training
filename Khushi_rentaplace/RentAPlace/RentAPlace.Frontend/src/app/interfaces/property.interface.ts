export interface Property {
  id: string;
  ownerId: string;
  ownerName: string;
  propertyType: string;
  title: string;
  description: string;
  address: string;
  city: string;
  state: string;
  country: string;
  postalCode?: string;
  latitude?: number;
  longitude?: number;
  pricePerNight: number;
  maxGuests: number;
  bedrooms: number;
  bathrooms: number;
  squareFootage?: number;
  isActive: boolean;
  isAvailable: boolean;
  createdAt: string;
  images: PropertyImage[];
  features: string[];
  averageRating?: number;
  reviewCount: number;
}

export interface PropertyImage {
  id: string;
  imagePath: string;
  imageName: string;
  isPrimary: boolean;
  displayOrder: number;
}

export interface PropertyType {
  id: string;
  name: string;
  description?: string;
  isActive: boolean;
}

export interface PropertyFeature {
  id: string;
  name: string;
  description?: string;
  isActive: boolean;
}

export interface PropertySearch {
  city?: string;
  state?: string;
  country?: string;
  propertyTypeId?: string;
  checkInDate?: string;
  checkOutDate?: string;
  maxGuests?: number;
  minPrice?: number;
  maxPrice?: number;
  propertyFeatureIds?: string[];
  pageNumber: number;
  pageSize: number;
  sortBy: string;
  sortOrder: string;
}

export interface PropertyCreate {
  propertyTypeId: string;
  title: string;
  description: string;
  address: string;
  city: string;
  state: string;
  country: string;
  postalCode?: string;
  latitude?: number;
  longitude?: number;
  pricePerNight: number;
  maxGuests: number;
  bedrooms: number;
  bathrooms: number;
  squareFootage?: number;
  propertyFeatureIds: string[];
}
