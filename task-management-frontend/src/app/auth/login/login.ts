import { Component } from '@angular/core';
import { Auth } from '../../services/auth';
import { Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, HttpClientModule, CommonModule, RouterModule],
  templateUrl: './login.html',
  styleUrls: ['./login.scss'] 
})
export class Login {
  email = '';
  password = '';

  constructor(private authService: Auth, private router: Router) {}

 login() {
  this.authService.login({ email: this.email, password: this.password }).subscribe({
    next: (res: any) => {
      this.authService.saveToken(res.token); 
      alert('Login successful!');
      this.router.navigate(['/profile']);
    },
    error: (err) => {
      alert('Login failed: ' + err.error);
    }
  });
}

}
