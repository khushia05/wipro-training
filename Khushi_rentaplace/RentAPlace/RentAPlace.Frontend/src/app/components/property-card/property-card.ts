import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Property } from '../../interfaces/property.interface';

@Component({
  selector: 'app-property-card',
  imports: [CommonModule],
  templateUrl: './property-card.html',
  styleUrl: './property-card.css'
})
export class PropertyCardComponent {
  @Input() property!: Property;
  @Output() propertyClick = new EventEmitter<Property>();
  @Output() bookNow = new EventEmitter<Property>();

  onPropertyClick() {
    this.propertyClick.emit(this.property);
  }

  onBookNow(event: Event) {
    // Prevent event bubbling to avoid triggering property click
    event.stopPropagation();
    console.log('Book Now clicked for property:', this.property.title);
    this.bookNow.emit(this.property);
  }

  getMainImage(): string {
    // Handle both string array and object array formats
    if (this.property.images && this.property.images.length > 0) {
      const firstImage = this.property.images[0];
      
      // If it's an object with imagePath property
      if (typeof firstImage === 'object' && firstImage.imagePath) {
        return this.getFullImageUrl(firstImage.imagePath);
      }
      
      // If it's a string
      if (typeof firstImage === 'string') {
        return this.getFullImageUrl(firstImage);
      }
    }
    
    // Default placeholder
    return 'https://picsum.photos/400/300?random=placeholder';
  }

  getFullImageUrl(imageUrl: string): string {
    // If it's already a full URL (http/https), return as is
    if (imageUrl.startsWith('http://') || imageUrl.startsWith('https://')) {
      return imageUrl;
    }
    
    // If it's the placeholder path, use a real placeholder image
    if (imageUrl === '/assets/images/placeholder.jpg') {
      return 'https://picsum.photos/400/300?random=placeholder';
    }
    
    // If it's a relative URL starting with /, prepend the API base URL
    if (imageUrl.startsWith('/')) {
      return `http://localhost:5158${imageUrl}`;
    }
    
    // If it's a relative URL without /, assume it's in the uploads folder
    return `http://localhost:5158/uploads/properties/${imageUrl}`;
  }

  getImageUrl(image: any): string {
    if (typeof image === 'object' && image.imagePath) {
      return this.getFullImageUrl(image.imagePath);
    }
    if (typeof image === 'string') {
      return this.getFullImageUrl(image);
    }
    return this.getMainImage();
  }

  onImageError(event: any) {
    // Use a data URL for a simple placeholder to avoid infinite loops
    event.target.src = 'data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMzAwIiBoZWlnaHQ9IjIwMCIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZjBmMGYwIi8+PHRleHQgeD0iNTAlIiB5PSI1MCUiIGZvbnQtZmFtaWx5PSJBcmlhbCwgc2Fucy1zZXJpZiIgZm9udC1zaXplPSIxNCIgZmlsbD0iIzk5OSIgdGV4dC1hbmNob3I9Im1pZGRsZSIgZHk9Ii4zZW0iPk5vIEltYWdlPC90ZXh0Pjwvc3ZnPg==';
  }
}