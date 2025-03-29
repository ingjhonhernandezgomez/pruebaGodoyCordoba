using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using PruebaUsuarios.Application.DTOs;
using PruebaUsuarios.Application.Exceptions;
using PruebaUsuarios.Application.Interfaces;
using PruebaUsuarios.Application.Services;
using PruebaUsuarios.Domain.Entities;
using PruebaUsuarios.Infrastructure.Data;
using PruebaUsuarios.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()  
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors();

app.UseExceptionHandler(appBuilder =>
{
    appBuilder.Run(async context =>
    {
        context.Response.ContentType = "application/json";

        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

        var statusCode = exception switch
        {
            ValidacionException => StatusCodes.Status400BadRequest,
            EntidadNoEncontradaException => StatusCodes.Status404NotFound,
            CredencialesInvalidasException => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };

        logger.LogError(exception, "Error capturado por el middleware");

        var error = new ErrorResponseDto
        {
            Mensaje = statusCode switch
            {
                400 => exception?.Message ?? "Error de validación.",
                404 => exception?.Message ?? "Recurso no encontrado.",
                401 => exception?.Message ?? "Credenciales invalidas.",
                _ => "Ocurrió un error interno en el servidor."
            },
            Detalle = exception?.Message
        };

        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(error);
    });
});

//Crear usuario admin de prueba
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!db.Usuarios.Any(u => u.CorreoElectronico == "admin@admin.com"))
    {
        db.Usuarios.Add(new Usuario
        {
            Nombre = "Admin",
            Apellidos = "Principal",
            Cedula = "0000000000",
            CorreoElectronico = "admin@admin.com",
            Clave = "admin123",
            FechaUltimoAcceso = DateTime.Now,
            Puntaje = 50
        });

        db.SaveChanges();
    }
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();