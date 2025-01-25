using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using APICatalogo.Models;
using APICatalogo.Repositories;


namespace APICatalogo.Controllers;


[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsuariosController : Controller
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuariosController(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    [HttpGet]
    public IActionResult GetAllUsuarios()
    {
        var usuarios = _usuarioRepository.GetAllUsuarios();
        return Ok(usuarios);
    }

    [HttpGet("{id}")]
    public IActionResult GetUsuarioById(int id)
    {
        var usuario = _usuarioRepository.GetUsuarioById(id);
        if (usuario == null) return NotFound();
        return Ok(usuario);
    }

    [HttpPost]
    public IActionResult CreateUsuario([FromBody] Usuario usuario)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        _usuarioRepository.AddUsuario(usuario);
        return CreatedAtAction(nameof(GetUsuarioById), new { id = usuario.UsuarioId }, usuario);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateUsuario(int id, [FromBody] Usuario usuario)
    {
        if (id != usuario.UsuarioId) return BadRequest("Usuario ID mismatch");

        var existingUsuario = _usuarioRepository.GetUsuarioById(id);
        if (existingUsuario == null) return NotFound();

        _usuarioRepository.UpdateUsuario(usuario);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteUsuario(int id)
    {
        var usuario = _usuarioRepository.GetUsuarioById(id);
        if (usuario == null) return NotFound();

        _usuarioRepository.DeleteUsuario(usuario);
        return NoContent();
    }
}