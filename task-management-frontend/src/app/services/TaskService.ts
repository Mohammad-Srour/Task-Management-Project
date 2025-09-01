import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Task {
  id?: string;
  title: string;
  description?: string;
  status: number;  
  priority: number;
  dueDate?: string;
}

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private apiUrl = 'https://localhost:7073/api/tasks';

  constructor(private http: HttpClient) {}

  private getAuthHeaders() {
    const token = localStorage.getItem('token');
    return {
      headers: new HttpHeaders({
        Authorization: `Bearer ${token}`
      })
    };
  }

  getTasks(): Observable<Task[]> {
    return this.http.get<Task[]>(this.apiUrl, this.getAuthHeaders());
  }

  getTask(id: string): Observable<Task> {
    return this.http.get<Task>(`${this.apiUrl}/${id}`, this.getAuthHeaders());
  }

  createTask(task: Task): Observable<Task> {
    return this.http.post<Task>(this.apiUrl, task, this.getAuthHeaders());
  }

  updateTask(id: string, task: Task): Observable<Task> {
    return this.http.put<Task>(`${this.apiUrl}/${id}`, task, this.getAuthHeaders());
  }

  deleteTask(id: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`, this.getAuthHeaders());
  }

  markComplete(id: string): Observable<Task> {
    return this.http.patch<Task>(`${this.apiUrl}/${id}/complete`, {}, this.getAuthHeaders());
  }

  markIncomplete(id: string): Observable<Task> {
    return this.http.patch<Task>(`${this.apiUrl}/${id}/incomplete`, {}, this.getAuthHeaders());
  }

  getTasksFiltered(filters?: { status?: number; priority?: number; dueDate?: string }): Observable<Task[]> {
  let url = this.apiUrl;

  const params: any = {};
  if (filters?.status !== undefined) params.status = filters.status;
  if (filters?.priority !== undefined) params.priority = filters.priority;
  if (filters?.dueDate !== undefined) params.dueDate = filters.dueDate;

  return this.http.get<Task[]>(url, {
    ...this.getAuthHeaders(),
    params: params
  });
}

}
