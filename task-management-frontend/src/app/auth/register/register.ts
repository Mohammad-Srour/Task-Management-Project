import { Component } from '@angular/core';
import { Auth } from '../../services/auth';
import { Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-register',
  imports: [FormsModule, HttpClientModule, RouterModule],
  templateUrl: './register.html',
  styleUrl: './register.scss'
})
export class Register {
  userName = '';
  email = '';
  password = '';

  constructor(private authService: Auth, private router: Router) {}

register(form?: any) {
  if (!this.userName || !this.email || !this.password) {
    alert('⚠️ Please fill in all fields');
    return;
  }

  this.authService.register({
    userName: this.userName,
    email: this.email,
    password: this.password
  }).subscribe({
    next: () => {
      alert('Registration successful!');
      this.router.navigate(['/login']);
    },
    error: (err) => {
      if (err.error && typeof err.error === 'string') {
        alert('Registration failed: ' + err.error);
      } else if (err.error && err.error.message) {
        alert('Registration failed: ' + err.error.message);
      } else {
        alert('Registration failed: ' + err.status + ' ' + err.statusText);
      }
    }
  });
}

}
