import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

interface FavoriteProperty {
  id: number;
  propertyId: number;
  propertyName: string;
  propertyImage: string;
  price: number;
  address: string;
  propertyType: string;
  bedrooms: number;
  bathrooms: number;
  area: number;
  addedDate: string;
  isAvailable: boolean;
}

@Component({
  selector: 'app-favorites',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './favorites.html',
  styleUrls: ['./favorites.css']
})
export class FavoritesComponent implements OnInit {
  favorites: FavoriteProperty[] = [];
  loading = false;
  error = '';
  filterType = 'all';

  constructor(
    private http: HttpClient,
    public router: Router
  ) {}

  ngOnInit() {
    this.loadFavorites();
  }

  loadFavorites() {
    this.loading = true;
    this.error = '';
    
    this.http.get<FavoriteProperty[]>('http://localhost:5158/api/favorites/my').subscribe({
      next: (favorites) => {
        this.favorites = favorites;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading favorites:', error);
        this.error = 'Failed to load favorites. Please try again.';
        this.loading = false;
      }
    });
  }

  get filteredFavorites(): FavoriteProperty[] {
    if (this.filterType === 'all') {
      return this.favorites;
    }
    return this.favorites.filter(favorite => 
      favorite.propertyType.toLowerCase() === this.filterType.toLowerCase()
    );
  }

  formatPrice(price: number): string {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD'
    }).format(price);
  }

  formatDate(dateString: string): string {
    return new Date(dateString).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    });
  }

  removeFromFavorites(favoriteId: number) {
    if (confirm('Are you sure you want to remove this property from your favorites?')) {
      this.loading = true;
      
      this.http.delete(`http://localhost:5158/api/favorites/${favoriteId}`).subscribe({
        next: () => {
          this.loadFavorites(); // Reload favorites
        },
        error: (error) => {
          console.error('Error removing favorite:', error);
          this.error = 'Failed to remove favorite. Please try again.';
          this.loading = false;
        }
      });
    }
  }

  viewProperty(propertyId: number) {
    this.router.navigate(['/properties', propertyId]);
  }

  goBack() {
    this.router.navigate(['/user-dashboard']);
  }

  onFilterChange() {
    // Filter is applied via getter
  }

  getPropertyTypeIcon(propertyType: string): string {
    switch (propertyType.toLowerCase()) {
      case 'apartment':
        return '🏢';
      case 'house':
        return '🏠';
      case 'villa':
        return '🏡';
      case 'studio':
        return '🏢';
      case 'loft':
        return '🏭';
      case 'townhouse':
        return '🏘️';
      default:
        return '🏠';
    }
  }
}
