import { Component, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PropertySearch as PropertySearchInterface } from '../../interfaces/property.interface';

@Component({
  selector: 'app-property-search',
  imports: [CommonModule, FormsModule],
  templateUrl: './property-search.html',
  styleUrl: './property-search.css'
})
export class PropertySearchComponent {
  @Output() search = new EventEmitter<PropertySearchInterface>();

  // Separate properties for date inputs (HTML date format)
  checkInDateInput: string = '';
  checkOutDateInput: string = '';

  searchCriteria: PropertySearchInterface = {
    city: '',
    checkInDate: '',
    checkOutDate: '',
    maxGuests: undefined,
    pageNumber: 1,
    pageSize: 10,
    sortBy: 'createdAt',
    sortOrder: 'desc'
  };

  onSearch() {
    // Only search if there's meaningful input
    if (!this.searchCriteria.city && !this.checkInDateInput && !this.checkOutDateInput && !this.searchCriteria.maxGuests) {
      console.log('No search criteria provided, skipping search');
      return;
    }

    // Convert HTML date inputs to ISO strings for the search criteria
    if (this.checkInDateInput) {
      this.searchCriteria.checkInDate = new Date(this.checkInDateInput).toISOString();
    }
    if (this.checkOutDateInput) {
      this.searchCriteria.checkOutDate = new Date(this.checkOutDateInput).toISOString();
    }

    console.log('Search criteria:', this.searchCriteria);
    this.search.emit(this.searchCriteria);
  }

  onSubmit(event: Event) {
    event.preventDefault(); // Prevent default form submission
    this.onSearch();
  }
}
