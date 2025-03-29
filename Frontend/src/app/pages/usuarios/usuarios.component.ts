import { Component, OnInit } from '@angular/core';
import { UsuarioService, Usuario } from '../../services/usuario.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { LoaderComponent } from '../../shared/loader/loader.component';

@Component({
  selector: 'app-usuarios',
  templateUrl: './usuarios.component.html',
  imports: [CommonModule, FormsModule, LoaderComponent],
  styleUrls: ['./usuarios.component.css'],
  standalone: true
})
export class UsuariosComponent implements OnInit {
  usuarios: Usuario[] = [];
  nuevo: Partial<Usuario> = {};
  editando = false;
  mensaje = '';
  error = '';
  cargando = true;

  constructor(private service: UsuarioService, private router: Router) {}

  ngOnInit() {
    this.cargar();
  }

  cargar() {
    this.service.getUsuarios().subscribe({
      next: data => {
        this.usuarios = data;
        this.limpiarMensajes();
        this.cargando = false;
      },
      error: err => {
        this.error = err.status === 0
          ? 'No se pudo conectar con el servidor.'
          : 'No se pudo obtener la lista de usuarios.';
        
        this.cargando = false;
      }
    });
  }

  guardar() {
    if (!this.validarFormulario()) return;

    this.limpiarMensajes();

    if (this.editando && this.nuevo.id) {
      this.actualizarUsuario(this.nuevo as Usuario);
    } else {
      this.crearUsuario(this.nuevo as Usuario);
    }
  }

  crearUsuario(usuario: Usuario) {
    this.cargando = true;
    this.service.crearUsuario(usuario).subscribe({
      next: () => {
        this.mensaje = 'Usuario creado exitosamente';
        this.nuevo = {};
        this.cargar();
      },
      error: err => {
        this.error = this.obtenerMensajeError(err, 'crear');
        this.cargando = false;
      }
    });
  }

  actualizarUsuario(usuario: Usuario) {
    this.service.actualizarUsuario(usuario).subscribe({
      next: () => {
        this.mensaje = 'Usuario actualizado con éxito';
        this.cancelar();
        this.cargar();
      },
      error: err => {
        this.error = this.obtenerMensajeError(err, 'actualizar');
      }
    });
  }

  eliminar(id: number) {
    if (!confirm('¿Estás seguro de eliminar este usuario?')) return;

    this.cargando = true;

    this.service.eliminarUsuario(id).subscribe({
      next: () => {
        this.mensaje = 'Usuario eliminado correctamente';
        this.cargar();
      },
      error: err => {
        this.error = this.obtenerMensajeError(err, 'eliminar');
        this.mensaje = '';
        this.cargando = false;
      }
    });
  }

  editar(usuario: Usuario) {
    this.nuevo = { ...usuario };
    this.editando = true;
    this.limpiarMensajes();
  }

  cancelar() {
    this.nuevo = {};
    this.editando = false;
    this.limpiarMensajes();
  }

  logout() {
    localStorage.removeItem('usuario');
    this.router.navigate(['/']);
  }

  private validarFormulario(): boolean {
    if (
      !this.nuevo.nombre ||
      !this.nuevo.apellidos ||
      !this.nuevo.cedula ||
      !this.nuevo.correoElectronico ||
      !this.nuevo.clave
    ) {
      this.error = 'Todos los campos son obligatorios';
      return false;
    }
    return true;
  }

  private limpiarMensajes() {
    this.error = '';
    this.mensaje = '';
  }

  private obtenerMensajeError(err: any, accion: string): string {
    if (err.status === 0) return 'No se pudo conectar con el servidor.';
    return err.error?.mensaje || `Error al ${accion} el usuario`;
  }

  getClasificacion(fecha: string | Date): string {
    const ultimoAcceso = new Date(fecha);
    const ahora = new Date();
    const diffHoras = (ahora.getTime() - ultimoAcceso.getTime()) / (1000 * 60 * 60);
  
    if (diffHoras <= 12) return 'Hechicero';
    if (diffHoras <= 48) return 'Luchador';
    if (diffHoras <= 168) return 'Explorador'; // 7 días = 168h
    return 'Olvidado';
  }
}
