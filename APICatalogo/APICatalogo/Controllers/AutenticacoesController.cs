using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using APICatalogo.Models;
using APICatalogo.Repositories;
using APICatalogo.Services;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class AutenticacoesController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ITokenRevocationService _tokenRevocationService;

    public AutenticacoesController(
    IConfiguration configuration,
    IUsuarioRepository usuarioRepository,
    ITokenRevocationService tokenRevocationService)
    {
        _configuration = configuration;
        _usuarioRepository = usuarioRepository;
        _tokenRevocationService = tokenRevocationService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] Login login)
    {
        if (!ModelState.IsValid)
            return BadRequest("Dados de login inválidos.");

        var usuario = _usuarioRepository.GetUsuarioByNome(login.Nome);

        if (usuario == null || !BCrypt.Net.BCrypt.Verify(login.Senha, usuario.Senha))
        {
            return Unauthorized("Nome ou senha inválidos.");
        }

        var token = GenerateJwtToken(usuario.Nome);
        return Ok(new { token });
    }

    private string GenerateJwtToken(string nome)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, nome)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }


    [HttpPost("logout")]
    public IActionResult Logout()
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        if (string.IsNullOrEmpty(token))
        {
            return BadRequest(new { message = "Token não fornecido." });
        }

        _tokenRevocationService.RevokeToken(token);
        return Ok(new { message = "Logout realizado com sucesso." });
    }
}