import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

interface RegisterDto {
  userName: string;
  email: string;
  password: string;
}

interface LoginDto {
  email: string;
  password: string;
}

@Injectable({
  providedIn: 'root'
})
export class Auth {
  private apiUrl = 'https://localhost:7073/api/auth'; // ← مسار مطابق للـ Controller

  constructor(private http: HttpClient) {}

  register(dto: RegisterDto): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, dto);
  }

  login(dto: LoginDto): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, dto);
  }

  saveToken(token: string) {
    localStorage.setItem('token', token);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  logout() {
    localStorage.removeItem('token');
  }

  getProfile(): Observable<any> {
    const token = this.getToken();
    const headers = token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : undefined;

    return this.http.get(`${this.apiUrl}/profile`, { headers });
  }
}
