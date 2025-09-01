import { Component } from '@angular/core';
import { Task, TaskService } from '../../services/TaskService';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

export enum TaskStatus {
  New = 0,
  InProgress = 1,
  Done = 2
}

export enum TaskPriority {
  Low = 0,
  Medium = 1,
  High = 2
}

@Component({
  selector: 'app-create-task',
  imports: [FormsModule, CommonModule, ReactiveFormsModule],
  templateUrl: './create-task.html',
  styleUrls: ['./create-task.scss']
})
export class CreateTask {
  taskForm: FormGroup;
  isSubmitting = false;
  errorMessage = '';
  taskStatus = TaskStatus;
  taskPriority = TaskPriority;

  constructor(
    private fb: FormBuilder,
    private taskService: TaskService,
    private router: Router
  ) {
    this.taskForm = this.fb.group({
      title: ['', Validators.required],
      description: [''],
      status: [TaskStatus.New, Validators.required],
      priority: [TaskPriority.Medium, Validators.required],
      dueDate: ['', Validators.required]    });
  }

    onSubmit(): void {
    if (this.taskForm.invalid) {
      this.taskForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = '';

    this.taskService.createTask(this.taskForm.value).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.taskForm.reset({
          status: TaskStatus.New,
          priority: TaskPriority.Medium
        });
        alert('Task created successfully!');
      },
      error: (err) => {
        this.isSubmitting = false;
        this.errorMessage = 'Failed to create task. Please try again.';
        console.error(err);
      }
    });
  }
}