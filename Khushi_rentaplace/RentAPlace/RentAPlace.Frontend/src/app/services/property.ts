import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Property as PropertyInterface, PropertySearch } from '../interfaces/property.interface';

@Injectable({
  providedIn: 'root'
})
export class PropertyService {
  private apiUrl = 'http://localhost:5158/api';

  constructor(private http: HttpClient) {}

  /**
   * Get properties by category name
   */
  getPropertiesByCategory(categoryName: string): Observable<PropertyInterface[]> {
    const url = `${this.apiUrl}/properties/category/${encodeURIComponent(categoryName)}`;
    return this.http.get<PropertyInterface[]>(url);
  }

  /**
   * Get properties filtered by category
   */
  getPropertiesByCategoryFilter(category: string): Observable<PropertyInterface[]> {
    const params = new HttpParams().set('category', category);
    const url = `${this.apiUrl}/properties`;
    console.log('Making API call to:', url, 'with category:', category);
    return this.http.get<PropertyInterface[]>(url, { params });
  }

  /**
   * Get all properties with optional search criteria
   */
  getProperties(searchCriteria?: PropertySearch): Observable<PropertyInterface[]> {
    let params = new HttpParams();
    
    if (searchCriteria) {
      if (searchCriteria.city) params = params.set('city', searchCriteria.city);
      if (searchCriteria.state) params = params.set('state', searchCriteria.state);
      if (searchCriteria.country) params = params.set('country', searchCriteria.country);
      if (searchCriteria.propertyTypeId) params = params.set('propertyTypeId', searchCriteria.propertyTypeId);
      if (searchCriteria.checkInDate) params = params.set('checkInDate', searchCriteria.checkInDate);
      if (searchCriteria.checkOutDate) params = params.set('checkOutDate', searchCriteria.checkOutDate);
      if (searchCriteria.maxGuests) params = params.set('maxGuests', searchCriteria.maxGuests.toString());
      if (searchCriteria.minPrice) params = params.set('minPrice', searchCriteria.minPrice.toString());
      if (searchCriteria.maxPrice) params = params.set('maxPrice', searchCriteria.maxPrice.toString());
      if (searchCriteria.propertyFeatureIds && searchCriteria.propertyFeatureIds.length > 0) {
        searchCriteria.propertyFeatureIds.forEach(id => {
          params = params.append('propertyFeatureIds', id);
        });
      }
      if (searchCriteria.pageNumber) params = params.set('pageNumber', searchCriteria.pageNumber.toString());
      if (searchCriteria.pageSize) params = params.set('pageSize', searchCriteria.pageSize.toString());
      if (searchCriteria.sortBy) params = params.set('sortBy', searchCriteria.sortBy);
      if (searchCriteria.sortOrder) params = params.set('sortOrder', searchCriteria.sortOrder);
    }

    return this.http.get<PropertyInterface[]>(`${this.apiUrl}/properties`, { params });
  }

  /**
   * Get a single property by ID
   */
  getPropertyById(id: string): Observable<PropertyInterface> {
    return this.http.get<PropertyInterface>(`${this.apiUrl}/properties/${id}`);
  }

  /**
   * Get featured properties
   */
  getFeaturedProperties(): Observable<PropertyInterface[]> {
    return this.http.get<PropertyInterface[]>(`${this.apiUrl}/properties/featured`);
  }

  /**
   * Get property types
   */
  getPropertyTypes(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/propertytypes`);
  }

  /**
   * Get property features
   */
  getPropertyFeatures(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/propertyfeatures`);
  }

  /**
   * Search properties with flexible parameters
   */
  searchProperties(searchParams: any): Observable<PropertyInterface[]> {
    let params = new HttpParams();
    
    // Add all search parameters to the HTTP params
    Object.keys(searchParams).forEach(key => {
      if (searchParams[key] !== null && searchParams[key] !== undefined && searchParams[key] !== '') {
        params = params.set(key, searchParams[key].toString());
      }
    });

    return this.http.get<PropertyInterface[]>(`${this.apiUrl}/properties`, { params });
  }
}
