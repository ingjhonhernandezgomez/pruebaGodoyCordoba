import { Routes } from '@angular/router';
import { provideRouter } from '@angular/router';

import { LoginComponent } from './pages/login/login.component';
import { UsuariosComponent } from './pages/usuarios/usuarios.component';

import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: '', component: LoginComponent },
  { path: 'usuarios', component: UsuariosComponent,canActivate: [authGuard] },
  { path: '**', redirectTo: '' }
];

export const appRouter = provideRouter(routes);
