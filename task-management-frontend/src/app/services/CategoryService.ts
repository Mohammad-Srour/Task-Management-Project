import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Auth } from './auth'; 

export interface Category {
  id: string;
  name: string;
  description?: string;
}

export interface CreateCategory {
  name: string;
  description?: string;
}

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private apiUrl = 'https://localhost:7073/api/categories';

  constructor(private http: HttpClient, private auth: Auth) {}

  private getAuthHeaders() {
    const token = this.auth.getToken();
    return {
      headers: new HttpHeaders({
        Authorization: `Bearer ${token}`
      })
    };
  }

  getCategories(): Observable<Category[]> {
  return this.http.get<Category[]>(this.apiUrl, {
    headers: {
      Authorization: `Bearer ${localStorage.getItem('token')}`
    }
  });
}

  getCategory(id: string): Observable<Category> {
    return this.http.get<Category>(`${this.apiUrl}/${id}`, this.getAuthHeaders());
  }

  createCategory(category: CreateCategory): Observable<Category> {
    return this.http.post<Category>(this.apiUrl, category, this.getAuthHeaders());
  }

  updateCategory(id: string, category: CreateCategory): Observable<Category> {
    return this.http.put<Category>(`${this.apiUrl}/${id}`, category, this.getAuthHeaders());
  }

  deleteCategory(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`, this.getAuthHeaders());
  }
}
