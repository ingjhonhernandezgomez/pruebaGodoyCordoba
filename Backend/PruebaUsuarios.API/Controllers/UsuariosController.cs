using Microsoft.AspNetCore.Mvc;
using PruebaUsuarios.Application.DTOs;
using PruebaUsuarios.Application.Services;
using PruebaUsuarios.Domain.Entities;

namespace PruebaUsuarios.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly UsuarioService _service;

    public UsuariosController(UsuarioService service)
    {
        _service = service;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var usuario = await _service.LoginAsync(request);
        return Ok(usuario);  
    }


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var usuarios = await _service.GetAllAsync();
        return Ok(usuarios);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var usuario = await _service.GetByIdAsync(id);
        return Ok(usuario);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Usuario usuario)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { mensaje = "Datos inválidos", errores = ModelState });
        
        var creado = await _service.CreateAsync(usuario);
        return CreatedAtAction(nameof(Get), new { id = creado.Id }, creado);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Usuario usuario)
    {
        if (id != usuario.Id)
            return BadRequest(new { mensaje = "El ID del usuario no coincide con la URL." });

        await _service.UpdateAsync(usuario);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existente = await _service.GetByIdAsync(id);
        if (existente is null)
            return NotFound(new { mensaje = "Usuario no encontrado" });

        await _service.DeleteAsync(id);
        return NoContent();
    }
}
