import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { PropertyCardComponent } from '../../components/property-card/property-card';
import { Property } from '../../interfaces/property.interface';
import { PropertyService } from '../../services/property';
import { BookingService } from '../../services/booking';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, PropertyCardComponent, FormsModule],
  templateUrl: './home.html',
  styleUrls: ['./home.css']
})
export class HomeComponent implements OnInit {
  featuredProperties: Property[] = [];
  searchResults: Property[] = [];
  showSearchResults: boolean = false;
  searchCriteria: any = null;
  isSearching: boolean = false;
  searchQuery: string = '';

  // Categories for the home page
  categories = [
    { name: 'Apartments', icon: '🏢', count: 0 },
    { name: 'Houses', icon: '🏠', count: 0 },
    { name: 'Villas', icon: '🏰', count: 0 },
    { name: 'Studios', icon: '🏡', count: 0 },
    { name: 'Townhouses', icon: '🏘️', count: 0 }
  ];

  constructor(
    private propertyService: PropertyService,
    private bookingService: BookingService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    // Load featured properties from API
    this.loadFeaturedProperties();
  }

  private loadFeaturedProperties() {
    // Load all available properties as featured properties
    this.propertyService.getProperties().subscribe({
      next: (apiProperties: any[]) => {
        // Map API response to frontend Property interface
        this.featuredProperties = apiProperties.slice(0, 6).map(apiProp => ({
          id: apiProp.id.toString(), // Convert int to string
          ownerId: apiProp.owner?.id || '',
          ownerName: apiProp.owner ? `${apiProp.owner.firstName} ${apiProp.owner.lastName}` : 'Unknown',
          propertyType: apiProp.propertyType,
          title: apiProp.name, // Map name to title
          description: apiProp.description,
          address: apiProp.address,
          city: apiProp.city,
          state: apiProp.state,
          country: 'USA', // Default since API doesn't return country
          postalCode: apiProp.zipCode,
          pricePerNight: apiProp.price, // Map price to pricePerNight
          maxGuests: apiProp.maxGuests,
          bedrooms: apiProp.bedrooms,
          bathrooms: apiProp.bathrooms,
          squareFootage: apiProp.area,
          isActive: true,
          isAvailable: apiProp.isAvailable,
          createdAt: new Date().toISOString(),
          images: (apiProp.images && apiProp.images.length > 0) 
            ? apiProp.images.map((img: string) => ({ id: '', imagePath: img, imageName: '', isPrimary: false, displayOrder: 0 }))
            : [], // Handle empty or null images array
          features: apiProp.features || [],
          reviewCount: 0
        }));
        console.log('Loaded featured properties:', this.featuredProperties);
        this.cdr.detectChanges();
      },
      error: (error: any) => {
        console.error('Error loading featured properties:', error);
        this.featuredProperties = [];
      }
    });
  }

  onSearch(searchCriteria: any) {
    console.log('Search criteria received:', searchCriteria);
    this.searchCriteria = searchCriteria;
    this.isSearching = true;
    
    // Call the search API
    this.propertyService.searchProperties(searchCriteria.query).subscribe({
      next: (apiProperties: any[]) => {
        console.log('Search API response:', apiProperties);
        
        // Map API response to frontend Property interface
        this.searchResults = apiProperties.map(apiProp => ({
          id: apiProp.id.toString(), // Convert int to string
          ownerId: apiProp.owner?.id || '',
          ownerName: apiProp.owner ? `${apiProp.owner.firstName} ${apiProp.owner.lastName}` : 'Unknown',
          propertyType: apiProp.propertyType,
          title: apiProp.name, // Map name to title
          description: apiProp.description,
          address: apiProp.address,
          city: apiProp.city,
          state: apiProp.state,
          country: 'USA', // Default since API doesn't return country
          postalCode: apiProp.zipCode,
          pricePerNight: apiProp.price, // Map price to pricePerNight
          maxGuests: apiProp.maxGuests,
          bedrooms: apiProp.bedrooms,
          bathrooms: apiProp.bathrooms,
          squareFootage: apiProp.area,
          isActive: true,
          isAvailable: apiProp.isAvailable,
          createdAt: new Date().toISOString(),
          images: (apiProp.images && apiProp.images.length > 0) 
            ? apiProp.images.map((img: string) => ({ id: '', imagePath: img, imageName: '', isPrimary: false, displayOrder: 0 }))
            : [], // Handle empty or null images array
          features: apiProp.features || [],
          reviewCount: 0
        }));
        
        this.showSearchResults = true;
        this.isSearching = false;
        console.log('Search results mapped:', this.searchResults);
        this.cdr.detectChanges();
      },
      error: (error: any) => {
        console.error('Error searching properties:', error);
        this.searchResults = [];
        this.showSearchResults = true;
        this.isSearching = false;
        this.cdr.detectChanges();
      }
    });
  }

  onCategoryClick(category: string) {
    console.log('Category clicked:', category);
    
    // Map frontend category names to database property type names
    const categoryMapping: { [key: string]: string } = {
      'apartments': 'Apartment',
      'houses': 'House', 
      'villas': 'Villa',
      'studios': 'Studio',
      'townhouses': 'Townhouse'
    };
    
    const mappedCategory = categoryMapping[category.toLowerCase()] || category.toLowerCase();
    this.router.navigate(['/properties/category', mappedCategory]);
  }

  onPropertyClick(property: Property) {
    console.log('Property clicked:', property);
    this.router.navigate(['/properties', property.id]);
  }

  onBookNow(property: Property) {
    console.log('Book Now clicked for property:', property);
    
    // Create a simple booking request
    const bookingRequest = {
      propertyId: parseInt(property.id), // Convert string to int
      checkInDate: new Date(Date.now() + 24 * 60 * 60 * 1000), // Tomorrow
      checkOutDate: new Date(Date.now() + 3 * 24 * 60 * 60 * 1000) // Day after tomorrow
    };
    
    this.bookingService.createSimpleBooking(bookingRequest).subscribe({
      next: (confirmation) => {
        console.log('Booking confirmed:', confirmation);
        alert(`Booking confirmed! Confirmation number: ${confirmation.confirmationNumber}`);
      },
      error: (error) => {
        console.error('Booking failed:', error);
        alert('Booking failed. Please try again.');
      }
    });
  }

  clearSearch() {
    this.showSearchResults = false;
    this.searchResults = [];
    this.searchCriteria = null;
  }

  // Simple search method for the search bar
  performSearch() {
    if (this.searchQuery.trim()) {
      this.onSearch({ query: this.searchQuery.trim() });
    }
  }
}