using PruebaUsuarios.Application.DTOs;
using PruebaUsuarios.Application.Exceptions;
using PruebaUsuarios.Application.Interfaces;
using PruebaUsuarios.Domain.Entities;

namespace PruebaUsuarios.Application.Services;

public class UsuarioService
{
    private readonly IUsuarioRepository _repository;

    public UsuarioService(IUsuarioRepository repository)
    {
        _repository = repository;
    }

    public async Task<Usuario> LoginAsync(LoginRequestDto request)
    {
        var usuarios = await _repository.GetAllAsync();

        var usuario = usuarios.FirstOrDefault(u =>
            u.CorreoElectronico == request.Correo && u.Clave == request.Clave);

        if (usuario is null)
            throw new CredencialesInvalidasException("Credenciales invalidas");

        usuario.FechaUltimoAcceso = DateTime.Now;
        await _repository.UpdateAsync(usuario);

        return usuario;
    }

    #region CRUD
    public async Task<List<Usuario>> GetAllAsync() => await _repository.GetAllAsync();

    public async Task<Usuario?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

    public async Task<Usuario> CreateAsync(Usuario usuario)
    {
        await ValidarUnicidadAsync(usuario);

        usuario.FechaUltimoAcceso = DateTime.Now;
        usuario.Puntaje = CalcularPuntaje(usuario);
        return await _repository.CreateAsync(usuario);
    }

    public async Task UpdateAsync(Usuario usuario)
    {
        var existente = await _repository.GetByIdAsync(usuario.Id);
        if (existente is null)
            throw new EntidadNoEncontradaException("El usuario que intenta actualizar no existe.");

        await ValidarUnicidadAsync(usuario, esActualizacion: true);

        existente.Nombre = usuario.Nombre;
        existente.Apellidos = usuario.Apellidos;
        existente.Cedula = usuario.Cedula;
        existente.CorreoElectronico = usuario.CorreoElectronico;
        existente.FechaUltimoAcceso = usuario.FechaUltimoAcceso;
        existente.Clave = usuario.Clave;

        existente.Puntaje = CalcularPuntaje(usuario);

        await _repository.UpdateAsync(existente);
    }


    public async Task DeleteAsync(int id) => await _repository.DeleteAsync(id);

    #endregion

    #region Lógica de negocio

    private int CalcularPuntaje(Usuario u)
    {
        
        var longitud = (u.Nombre + u.Apellidos).Length;
        int puntaje = longitud switch
        {
            > 10 => 20,
            >= 5 => 10,
            _ => 0
        };

        puntaje += u.CorreoElectronico.EndsWith("@gmail.com") ? 40 :
                   u.CorreoElectronico.EndsWith("@hotmail.com") ? 20 : 10;

        return puntaje;
    }

    private async Task ValidarUnicidadAsync(Usuario usuario, bool esActualizacion = false)
    {
        var usuarios = await _repository.GetAllAsync();

        var duplicado = usuarios.FirstOrDefault(u =>
            (u.Cedula == usuario.Cedula || u.CorreoElectronico == usuario.CorreoElectronico) &&
            (!esActualizacion || u.Id != usuario.Id)); 

        if (duplicado != null)
        {
            throw new ValidacionException("Ya existe un usuario con la misma cédula o correo electrónico.");
        }
    }

    #endregion

    

}