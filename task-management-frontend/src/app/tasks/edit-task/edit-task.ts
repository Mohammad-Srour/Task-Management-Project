import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { TaskPriority, TaskStatus } from '../create-task/create-task';
import { ActivatedRoute, Router } from '@angular/router';
import { Task, TaskService } from '../../services/TaskService';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-edit-task',
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './edit-task.html',
  styleUrl: './edit-task.scss'
})
export class EditTask implements OnInit {
  taskForm!: FormGroup;
  isSubmitting = false;
  errorMessage = '';
  taskId!: string;
  taskStatus = TaskStatus;
  taskPriority = TaskPriority;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private taskService: TaskService
  ) {}

  ngOnInit(): void {
    this.taskId = this.route.snapshot.paramMap.get('id')!;

    this.taskForm = this.fb.group({
      title: ['', Validators.required],
      description: [''],
      status: [TaskStatus.New, Validators.required],
      priority: [TaskPriority.Medium, Validators.required],
      dueDate: ['']
    });

    this.taskService.getTask(this.taskId).subscribe(task => {
      this.taskForm.patchValue(task);
    });
  }

  onSubmit(): void {
    if (this.taskForm.invalid) return;

    this.isSubmitting = true;

    const updatedTask: Task = {
      ...this.taskForm.value,
      dueDate: this.taskForm.value.dueDate || null
    };

    this.taskService.updateTask(this.taskId, updatedTask).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.router.navigate(['/tasks']);
      },
      error: (err) => {
        this.isSubmitting = false;
        this.errorMessage = 'Failed to update the task.';
        console.error(err);
      }
    });
  }
}