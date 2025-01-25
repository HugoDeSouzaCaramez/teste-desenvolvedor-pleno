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
public class AutenticacoesController : Controller
{
    private readonly IConfiguration _configuration;

    public AutenticacoesController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost("login")]
    public IActionResult Login(Login login)
    {
        if (login.Nome == "admin" && login.Senha == "password")
        {
            var token = GenerateJwtToken(login.Nome);
            return Ok(new { token });
        }

        return Unauthorized("Senha ou nome invalidos");
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
}