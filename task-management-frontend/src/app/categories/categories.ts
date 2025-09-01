import { Component, OnInit } from '@angular/core';
import { Category, CategoryService, CreateCategory } from '../services/CategoryService';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-categories',
  imports: [FormsModule,CommonModule],
  templateUrl: './categories.html',
  styleUrl: './categories.scss'
})
export class Categories implements OnInit{
categories: Category[] = [];
  newCategory: CreateCategory = { name: '', description: '' };

  constructor(private categoryService: CategoryService) {}

  ngOnInit(): void {
    this.loadCategories();
  }

  loadCategories() {
    this.categoryService.getCategories().subscribe(data => {
      this.categories = data;
    });
  }

  addCategory() {
    if (!this.newCategory.name) return;

    this.categoryService.createCategory(this.newCategory).subscribe(cat => {
      this.categories.push(cat);
      this.newCategory = { name: '', description: '' }; 
    });
  }

  deleteCategory(id: string) {
    this.categoryService.deleteCategory(id).subscribe(() => {
      this.categories = this.categories.filter(c => c.id !== id);
    });
  }

}
