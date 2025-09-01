import { HttpClientModule } from '@angular/common/http';
import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Register } from "./auth/register/register";
import { Login } from "./auth/login/login";
import { Home } from "./home/home";
import { Header } from "./header/header";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, HttpClientModule, Header],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('task-management-frontend');
}
