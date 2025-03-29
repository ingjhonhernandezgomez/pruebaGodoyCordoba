import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface Usuario {
  id: number;
  nombre: string;
  apellidos: string;
  cedula: string;
  correoElectronico: string;
  fechaUltimoAcceso: string;
  clasificacion: string;
  puntaje: number;
  clave: string;
}

@Injectable({ providedIn: 'root' })
export class UsuarioService { 
  private apiUrl = 'http://localhost:5166/api/usuarios'; 
  
  constructor(private http: HttpClient) {}

  login(correo: string, clave: string): Observable<Usuario> {
    return this.http.post<Usuario>(`${this.apiUrl}/login`, { correo, clave });
  }  

  getUsuarios(): Observable<Usuario[]> {
    return this.http.get<Usuario[]>(this.apiUrl);
  }

  crearUsuario(usuario: Usuario): Observable<Usuario> {
    return this.http.post<Usuario>(this.apiUrl, usuario);
  }

  actualizarUsuario(usuario: Usuario): Observable<void> {
    console.log(usuario);
    return this.http.put<void>(`${this.apiUrl}/${usuario.id}`, usuario);
  }

  eliminarUsuario(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
