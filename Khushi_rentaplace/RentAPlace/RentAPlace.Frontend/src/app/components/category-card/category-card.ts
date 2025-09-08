import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';

export interface Category {
  name: string;
  image: string;
  description?: string;
}

@Component({
  selector: 'app-category-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './category-card.html',
  styleUrl: './category-card.css'
})
export class CategoryCardComponent {
  @Input() category!: Category;
  @Output() categoryClick = new EventEmitter<string>();

  onCategoryClick() {
    this.categoryClick.emit(this.category.name);
  }
}
