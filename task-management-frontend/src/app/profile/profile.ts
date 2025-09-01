import { Component, OnInit } from '@angular/core';
import { Auth } from '../services/auth';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './profile.html',
  styleUrl: './profile.scss'
})
export class Profile implements OnInit {
  user:any= null;
  constructor(private authService: Auth) {}

  ngOnInit() {
    this.loadProfile();
  }
loadProfile() {
    this.authService.getProfile().subscribe({
      next: (res) => this.user = res,
      error: (err) => console.error(err)
    });
  }
}
