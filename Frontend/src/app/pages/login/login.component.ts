import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { UsuarioService, Usuario } from '../../services/usuario.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { LoaderComponent } from '../../shared/loader/loader.component';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule, LoaderComponent]
})
export class LoginComponent {
  correo = '';
  clave = '';
  error = '';
  cargando = false;

  constructor(private router: Router, private service: UsuarioService) {}

  login() {
    this.error = '';
    
    if (!this.correo || !this.clave) {
      this.error = 'Debes ingresar tu correo y contraseña';
      return;
    }

    this.cargando = true;

    this.service.login(this.correo, this.clave).subscribe({
      next: usuario => {
        localStorage.setItem('usuario', JSON.stringify(usuario));
        this.router.navigate(['/usuarios']);
        this.cargando = false;
      },
      error: err => {
        console.log(err);
        if (err.status === 0) {
          this.error = 'No se pudo conectar con el servidor. Revisa tu conexión.';
        } else if (err.status === 401) {
          this.error = err.error?.mensaje || 'Credenciales incorrectas.';
        } else {
          this.error = 'Ha ocurrido un error inesperado. Intenta de nuevo.';
        }

        this.cargando = false;
      }
    });
  }
}

