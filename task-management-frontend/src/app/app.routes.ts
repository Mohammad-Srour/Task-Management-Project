import { Routes } from '@angular/router';
import { Register } from './auth/register/register';
import { Login } from './auth/login/login';
import { Home } from './home/home';
import { Profile } from './profile/profile';
import { AuthGuard } from './services/authGuard';
import { Tasks } from './tasks/tasks';
import { CreateTask } from './tasks/create-task/create-task';
import { EditTask } from './tasks/edit-task/edit-task';
import { Categories } from './categories/categories';

export const routes: Routes = [ { path: 'login', component: Login },
  { path: 'register', component: Register },
  { path: 'login', component: Login },
   { path: '', component: Home },
   { path: 'profile', component: Profile,canActivate:[AuthGuard] }, 
   {path:"tasks",component:Tasks},   
   { path: 'tasks/create', component: CreateTask, canActivate: [AuthGuard] },
   { path: 'tasks/edit/:id', component: EditTask },
   { path: 'categories', component: Categories, canActivate: [AuthGuard] },
   { path: '**', redirectTo: '' }]
