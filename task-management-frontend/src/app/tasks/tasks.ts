import { Component, OnInit } from '@angular/core';
import { Task, TaskService } from '../services/TaskService';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { TaskPriority } from './create-task/create-task';

@Component({
  selector: 'app-tasks',
  imports: [FormsModule, CommonModule],
  templateUrl: './tasks.html',
  styleUrl: './tasks.scss'
})
export class Tasks implements OnInit {
  tasks: Task[] = [];

constructor(private taskService: TaskService, private router: Router) {}

  ngOnInit() {
    this.loadTasks();
  }

  loadTasks() {
    this.taskService.getTasks().subscribe(tasks => {
      this.tasks = tasks;
    });
  }

  deleteTask(id: string) {
    this.taskService.deleteTask(id).subscribe(() => {
      this.tasks = this.tasks.filter(t => t.id !== id);
    });
  }

  toggleComplete(task: Task) {
    if (task.status === 2) { // Done
      this.taskService.markIncomplete(task.id!).subscribe(updated => task.status = updated.status);
    } else {
      this.taskService.markComplete(task.id!).subscribe(updated => task.status = updated.status);
    }
  }
  editTask(id: string) {
  this.router.navigate(['/tasks/edit', id]);
}
  TaskPriority = TaskPriority;

getPriorityLabel(priority: number): string {
  switch (priority) {
    case 0: return 'Low';
    case 1: return 'Medium';
    case 2: return 'High';
    default: return '';
  }
}


getStatusLabel(status: number): string {
  switch (status) {
    case 0: return 'New';
    case 1: return 'In Progress';
    case 2: return 'Done';
    default: return '';
  }
}
filterTasks() {
  this.taskService.getTasksFiltered({ status: 1, priority: 2 }).subscribe(tasks => {
    this.tasks = tasks;
  });
}

filters: { status?: number; priority?: number; dueDate?: string } = {};

applyFilter() {
  this.taskService.getTasksFiltered(this.filters).subscribe(tasks => {
    this.tasks = tasks;
  });
}

resetFilter() {
  this.filters = {};
  this.loadTasks();
}



}