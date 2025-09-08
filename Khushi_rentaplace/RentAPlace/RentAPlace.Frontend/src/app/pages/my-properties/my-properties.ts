import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

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
  isAvailable: boolean;
  images: string[];
  features: string[];
  createdAt: string;
  bookingCount: number;
}

@Component({
  selector: 'app-my-properties',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './my-properties.html',
  styleUrls: ['./my-properties.css']
})
export class MyPropertiesComponent implements OnInit {
  properties: Property[] = [];
  loading = false;
  error = '';
  filterStatus = 'all';

  constructor(
    private http: HttpClient,
    public router: Router
  ) {}

  ngOnInit() {
    this.loadMyProperties();
  }

  loadMyProperties() {
    this.loading = true;
    this.error = '';
    
    this.http.get<any[]>('http://localhost:5158/api/properties/owner').subscribe({
      next: (apiProperties) => {
        console.log('API Response received:', apiProperties);
        
        // Map API response to frontend Property interface
        this.properties = apiProperties.map(apiProp => ({
          id: apiProp.id,
          name: apiProp.name,
          description: apiProp.description,
          address: apiProp.address,
          city: apiProp.city,
          state: apiProp.state,
          zipCode: apiProp.zipCode,
          price: apiProp.price,
          propertyType: apiProp.propertyType,
          bedrooms: apiProp.bedrooms,
          bathrooms: apiProp.bathrooms,
          area: apiProp.area,
          isAvailable: apiProp.isAvailable,
          images: this.processImageUrls(apiProp.images || []),
          features: apiProp.features || [],
          createdAt: new Date().toISOString(),
          bookingCount: apiProp.totalBookings || 0
        }));
        
        console.log('Mapped properties:', this.properties);
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading properties:', error);
        this.error = 'Failed to load properties. Please try again.';
        this.loading = false;
      }
    });
  }

  private processImageUrls(images: string[]): string[] {
    return images.map(imageUrl => {
      // If it's already a full URL (http/https), return as is
      if (imageUrl.startsWith('http://') || imageUrl.startsWith('https://')) {
        return imageUrl;
      }
      
      // If it's a relative URL starting with /, prepend the API base URL
      if (imageUrl.startsWith('/')) {
        return `http://localhost:5158${imageUrl}`;
      }
      
      // If it's a relative URL without /, assume it's in the uploads folder
      return `http://localhost:5158/uploads/properties/${imageUrl}`;
    });
  }

  getMainImage(property: Property): string {
    if (property.images && property.images.length > 0) {
      return property.images[0];
    }
    return 'https://picsum.photos/400/300?random=placeholder';
  }

  onImageError(event: any) {
    event.target.src = 'data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMzAwIiBoZWlnaHQ9IjIwMCIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZjBmMGYwIi8+PHRleHQgeD0iNTAlIiB5PSI1MCUiIGZvbnQtZmFtaWx5PSJBcmlhbCwgc2Fucy1zZXJpZiIgZm9udC1zaXplPSIxNCIgZmlsbD0iIzk5OSIgdGV4dC1hbmNob3I9Im1pZGRsZSIgZHk9Ii4zZW0iPk5vIEltYWdlPC90ZXh0Pjwvc3ZnPg==';
  }

  get filteredProperties(): Property[] {
    if (this.filterStatus === 'all') {
      return this.properties;
    }
    return this.properties.filter(property => {
      if (this.filterStatus === 'available') {
        return property.isAvailable;
      } else if (this.filterStatus === 'unavailable') {
        return !property.isAvailable;
      }
      return true;
    });
  }

  formatPrice(price: number): string {
    return new Intl.NumberFormat('en-IN', {
      style: 'currency',
      currency: 'INR'
    }).format(price);
  }

  formatDate(dateString: string): string {
    return new Date(dateString).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    });
  }

  toggleAvailability(propertyId: number) {
    const property = this.properties.find(p => p.id === propertyId);
    if (property) {
      const newStatus = !property.isAvailable;
      
      this.http.put(`http://localhost:5158/api/properties/${propertyId}/availability`, {
        isAvailable: newStatus
      }).subscribe({
        next: () => {
          property.isAvailable = newStatus;
        },
        error: (error) => {
          console.error('Error updating availability:', error);
          this.error = 'Failed to update availability. Please try again.';
        }
      });
    }
  }

  editProperty(propertyId: number) {
    this.router.navigate(['/edit-property', propertyId]);
  }

  viewBookings(propertyId: number) {
    this.router.navigate(['/property-bookings', propertyId]);
  }

  deleteProperty(propertyId: number) {
    if (confirm('Are you sure you want to delete this property? This action cannot be undone.')) {
      this.loading = true;
      
      this.http.delete(`http://localhost:5158/api/properties/${propertyId}`).subscribe({
        next: () => {
          this.loadMyProperties(); // Reload properties
        },
        error: (error) => {
          console.error('Error deleting property:', error);
          this.error = 'Failed to delete property. Please try again.';
          this.loading = false;
        }
      });
    }
  }

  goBack() {
    this.router.navigate(['/owner-dashboard']);
  }

  onFilterChange() {
    // Filter is applied via getter
  }
}
