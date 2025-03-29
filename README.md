# Prueba Técnica - Godoy Córdoba | Littler

Este repositorio contiene el desarrollo de una prueba técnica para la empresa **Godoy Córdoba | Littler**, que consiste en una aplicación web para la gestión de usuarios. Incluye un frontend en Angular y un backend en .NET 7 con arquitectura limpia (Clean Architecture).

---

## 💡 Tecnologías utilizadas

### Backend

- ASP.NET Core 7
- Entity Framework Core
- SQL Server
- Clean Architecture (Domain, Application, Infrastructure, API)

### Frontend

- Angular 18 (Standalone Components)
- CSS

---

## 🚀 Características principales

### Funcionalidades

- Registro, edición y eliminación de usuarios
- Login con sesión simulada (almacenamiento en `localStorage`)
- Clasificación de usuarios según fecha de último acceso:
  - Hechicero, Luchador, Explorador, Olvidado
- Asignación de puntaje según dominio de correo electrónico
- Validaciones de correo y cédula únicos
- Protección de rutas (auth guard)
- Carga condicional (`loading`) en cada acción
- Mensajes de error claros desde el backend hacia el frontend

### Usuario administrador por defecto

- **Correo**: `admin@admin.com`
- **Clave**: `admin123`

> ⬆️ Se crea automáticamente al aplicar la migración `SeedAdminUser` en la base de datos.

---

## 📚 Estructura del repositorio

```
/prueba-godoy-cordoba/
├── backend/                  # Proyecto .NET con Clean Architecture
│   ├── PruebaUsuarios.API/
│   ├── PruebaUsuarios.Application/
│   ├── PruebaUsuarios.Domain/
│   ├── PruebaUsuarios.Infrastructure/
│   └── PruebaUsuarios.sln
├── frontend/                 # Proyecto Angular
│   ├── src/
│   └── angular.json
├── README.md
```

---

## ⚙️ Instalación

### Backend (.NET 7)

1. Ubícate en la carpeta `backend/`
2. Configura la cadena de conexión en `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=PruebaUsuariosDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

> Si usas autenticación por usuario y contraseña:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=PruebaUsuariosDB;User Id=sa;Password=TuPassword123;TrustServerCertificate=True;"
}
```

3. Ejecuta las migraciones:

```bash
dotnet ef database update --project PruebaUsuarios.Infrastructure --startup-project PruebaUsuarios.API
```

4. Ejecuta la API:

```bash
dotnet run --project PruebaUsuarios.API
```

### Frontend (Angular)

1. Ubícate en la carpeta `frontend/`
2. Instala dependencias:

```bash
npm install
```

3. Ejecuta la app:

```bash
ng serve
```

> 🔗 En el archivo `usuario.service.ts`, la URL de la API está configurada como:
>
> ```ts
> private apiUrl = 'http://localhost:5166/api/usuarios';
> ```
>
> Si es necesario puedes cambiar el puerto donde se ejecuta el API de manera local

---

## 🚫 Rutas protegidas

- El acceso a `/usuarios` está protegido mediante un **AuthGuard**
- Se requiere sesión activa (usuario en `localStorage`) para navegar

---

## ✉️ Contacto

Desarrollado por Jhon Jairo Hernandez Gomez

LinkedIn: (https://www.linkedin.com/in/jhon-gomez-139a2a1a2/) 
GitHub: ingjhonhernandezgomez 
Correo: ing.jhonhernandezgomez@gmail.com

---

> ✅ Este proyecto fue desarrollado con enfoque en principios SOLID, buenas prácticas de arquitectura y experiencia de usuario. 

