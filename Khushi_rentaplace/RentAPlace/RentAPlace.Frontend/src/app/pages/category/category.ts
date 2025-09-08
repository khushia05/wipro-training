import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { PropertyCardComponent } from '../../components/property-card/property-card';
import { Property } from '../../interfaces/property.interface';
import { PropertyService } from '../../services/property';
import { BookingService } from '../../services/booking';

@Component({
  selector: 'app-category',
  standalone: true,
  imports: [CommonModule, PropertyCardComponent],
  templateUrl: './category.html',
  styleUrl: './category.css'
})
export class CategoryComponent implements OnInit {
  categoryName: string = '';
  properties: Property[] = [];
  isLoading: boolean = false;
  error: string = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private propertyService: PropertyService,
    private bookingService: BookingService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.categoryName = params['categoryName'];
      console.log('Category component initialized with categoryName:', this.categoryName);
      if (this.categoryName) {
        this.loadPropertiesByCategory();
      }
    });
  }

  loadPropertiesByCategory() {
    this.isLoading = true;
    this.error = '';
    
    console.log(`Searching for category: ${this.categoryName}`);
    
    // Test direct HTTP call first
    console.log('Testing direct HTTP call...');
    fetch(`http://localhost:5158/api/properties?category=${this.categoryName}`)
      .then(response => {
        console.log('Direct fetch response status:', response.status);
        return response.json();
      })
      .then(data => {
        console.log('Direct fetch response data:', data);
      })
      .catch(error => {
        console.error('Direct fetch error:', error);
      });
    
    // Use the correct method that calls /api/properties?category={categoryName}
    this.propertyService.getPropertiesByCategoryFilter(this.categoryName).subscribe({
      next: (apiProperties: any[]) => {
        console.log('API Response received:', apiProperties);
        
        try {
          // Map API response to frontend Property interface
          this.properties = apiProperties.map(apiProp => ({
            id: apiProp.id.toString(), // Convert int to string
            ownerId: apiProp.owner?.id || '',
            ownerName: apiProp.owner ? `${apiProp.owner.firstName} ${apiProp.owner.lastName}` : 'Unknown',
            propertyType: apiProp.propertyType,
            title: apiProp.name, // Map name to title
            description: apiProp.description,
            address: apiProp.address,
            city: apiProp.city,
            state: apiProp.state,
            country: apiProp.country || 'India', // Use API country or default to India
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
              : [], // Handle empty images array
            features: apiProp.features || [],
            reviewCount: 0
          }));
          
          this.isLoading = false;
          console.log(`Loaded ${this.properties.length} properties for category: ${this.categoryName}`);
          console.log('Mapped properties:', this.properties);
          this.cdr.detectChanges(); // Force change detection
        } catch (error) {
          console.error('Error mapping properties:', error);
          this.isLoading = false;
          this.error = 'Error processing property data';
          this.cdr.detectChanges(); // Force change detection
        }
      },
      error: (error) => {
        console.error('Error loading properties:', error);
        this.error = 'Failed to load properties. Please try again.';
        this.isLoading = false;
        this.cdr.detectChanges(); // Force change detection
      }
    });
  }

  onPropertyClick(property: Property) {
    console.log('Property clicked:', property);
    // TODO: Navigate to property details page
    // this.router.navigate(['/properties', property.id]);
  }

  onBookNow(property: Property) {
    console.log('Booking property:', property);
    
    if (!property.id) {
      alert('Property ID not found. Cannot proceed with booking.');
      return;
    }

    // Create booking request
    const bookingRequest = {
      propertyId: parseInt(property.id), // Convert string to number
      checkInDate: new Date(Date.now() + 24 * 60 * 60 * 1000), // Tomorrow
      checkOutDate: new Date(Date.now() + 3 * 24 * 60 * 60 * 1000) // 3 days from now
    };

    // Show loading state
    const propertyName = property.title || 'this property';
    
    // Call booking API
    this.bookingService.createSimpleBooking(bookingRequest).subscribe({
      next: (confirmation) => {
        console.log('Booking confirmed:', confirmation);
        alert(`🎉 Booking Confirmed!\n\nProperty: ${confirmation.propertyName}\nConfirmation Number: ${confirmation.confirmationNumber}\nCheck-in: ${new Date(confirmation.checkInDate).toLocaleDateString()}\nCheck-out: ${new Date(confirmation.checkOutDate).toLocaleDateString()}\nTotal Price: ₹${confirmation.totalPrice}\n\nThank you for booking with us!`);
      },
      error: (error) => {
        console.error('Booking failed:', error);
        let errorMessage = 'Failed to book the property. Please try again.';
        if (error.error?.message) {
          errorMessage = error.error.message;
        } else if (error.status === 404) {
          errorMessage = 'Property not found.';
        } else if (error.status === 400) {
          errorMessage = 'Invalid booking request. Please check the dates.';
        }
        alert(`❌ Booking Failed\n\n${errorMessage}`);
      }
    });
  }

  goBack() {
    this.router.navigate(['/']);
  }
}
