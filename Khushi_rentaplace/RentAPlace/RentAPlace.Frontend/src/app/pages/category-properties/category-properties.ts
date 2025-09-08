import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
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
  maxGuests: number;
  images: string[];
  features: string[];
  isAvailable: boolean;
}

@Component({
  selector: 'app-category-properties',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './category-properties.html',
  styleUrls: ['./category-properties.css']
})
export class CategoryPropertiesComponent implements OnInit {
  properties: Property[] = [];
  category: string = '';
  loading = false;
  error = '';
  currentImageIndex: { [key: number]: number } = {};

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private http: HttpClient
  ) {}

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.category = params['category'];
      this.loadPropertiesByCategory();
    });
  }

  loadPropertiesByCategory() {
    this.loading = true;
    this.error = '';

    this.http.get<Property[]>(`http://localhost:5158/api/properties?category=${this.category}`).subscribe({
      next: (properties) => {
        this.properties = properties;
        // Initialize image indices
        properties.forEach(property => {
          this.currentImageIndex[property.id] = 0;
        });
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading properties:', error);
        this.error = 'Failed to load properties. Please try again.';
        this.loading = false;
      }
    });
  }

  nextImage(propertyId: number, property: Property) {
    if (property.images.length > 0) {
      this.currentImageIndex[propertyId] = (this.currentImageIndex[propertyId] + 1) % property.images.length;
    }
  }

  previousImage(propertyId: number, property: Property) {
    if (property.images.length > 0) {
      this.currentImageIndex[propertyId] = this.currentImageIndex[propertyId] === 0 
        ? property.images.length - 1 
        : this.currentImageIndex[propertyId] - 1;
    }
  }

  goToImage(propertyId: number, index: number) {
    this.currentImageIndex[propertyId] = index;
  }

  getCurrentImage(property: Property): string {
    if (property.images && property.images.length > 0) {
      return property.images[this.currentImageIndex[property.id]];
    }
    return '/assets/placeholder-property.jpg';
  }

  onImageError(event: any) {
    event.target.src = '/assets/placeholder-property.jpg';
  }

  formatPrice(price: number): string {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD'
    }).format(price);
  }

  getCategoryDisplayName(): string {
    return this.category.charAt(0).toUpperCase() + this.category.slice(1) + 's';
  }

  onBookNow(property: Property) {
    this.router.navigate(['/property', property.id]);
  }

  goBack() {
    this.router.navigate(['/']);
  }
}
