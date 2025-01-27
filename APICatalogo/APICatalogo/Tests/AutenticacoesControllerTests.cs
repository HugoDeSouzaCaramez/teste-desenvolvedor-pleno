using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using APICatalogo.Controllers;
using APICatalogo.Models;
using APICatalogo.Repositories;
using APICatalogo.Services;
using Microsoft.AspNetCore.Http;

namespace APICatalogo.Tests
{
    public class AutenticacoesControllerTests
    {
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
        private readonly Mock<ITokenRevocationService> _tokenRevocationServiceMock;
        private readonly AutenticacoesController _controller;

        public AutenticacoesControllerTests()
        {
            _configurationMock = new Mock<IConfiguration>();
            _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
            _tokenRevocationServiceMock = new Mock<ITokenRevocationService>();

            _configurationMock.SetupGet(config => config["Jwt:Key"]).Returns("ChaveSeguraParaTesteDeJWT1234567890123456");
            _configurationMock.SetupGet(config => config["Jwt:Issuer"]).Returns("https://teste.com");
            _configurationMock.SetupGet(config => config["Jwt:Audience"]).Returns("https://teste.com");

            _controller = new AutenticacoesController(
                _configurationMock.Object,
                _usuarioRepositoryMock.Object,
                _tokenRevocationServiceMock.Object);
        }

        [Fact]
        public void Login_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            _controller.ModelState.AddModelError("Nome", "O nome é obrigatório");

            var result = _controller.Login(new Login());

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Dados de login inválidos.", badRequestResult.Value);
        }


        [Fact]
        public void Login_ReturnsUnauthorized_WhenUsuarioNaoExiste()
        {
            _usuarioRepositoryMock.Setup(repo => repo.GetUsuarioByNome("usuarioInvalido")).Returns((Usuario)null);

            var login = new Login { Nome = "usuarioInvalido", Senha = "senha123" };

            var result = _controller.Login(login);

            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Nome ou senha inválidos.", unauthorizedResult.Value);
        }


        [Fact]
        public void Login_ReturnsUnauthorized_WhenSenhaInvalida()
        {
            var usuario = new Usuario { Nome = "usuarioValido", Senha = BCrypt.Net.BCrypt.HashPassword("senhaCorreta") };
            _usuarioRepositoryMock.Setup(repo => repo.GetUsuarioByNome("usuarioValido")).Returns(usuario);

            var login = new Login { Nome = "usuarioValido", Senha = "senhaErrada" };

            var result = _controller.Login(login);

            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Nome ou senha inválidos.", unauthorizedResult.Value);
        }


        [Fact]
        public void Login_ReturnsOk_WithJwtToken()
        {
            var usuario = new Usuario { Nome = "usuarioValido", Senha = BCrypt.Net.BCrypt.HashPassword("senhaCorreta") };
            _usuarioRepositoryMock.Setup(repo => repo.GetUsuarioByNome("usuarioValido")).Returns(usuario);

            var login = new Login { Nome = "usuarioValido", Senha = "senhaCorreta" };

            var result = _controller.Login(login);

            var okResult = Assert.IsType<OkObjectResult>(result);
            dynamic tokenResponse = okResult.Value;
            Assert.False(string.IsNullOrEmpty(tokenResponse.token));
        }


        [Fact]
        public void Logout_ReturnsBadRequest_WhenTokenIsNotProvided()
        {
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _controller.Request.Headers["Authorization"] = "";

            var result = _controller.Logout();

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            dynamic response = badRequestResult.Value;
            Assert.Equal("Token não fornecido.", response.message);
        }
        

        [Fact]
        public void Logout_ReturnsOk_WhenTokenIsValid()
        {
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _controller.Request.Headers["Authorization"] = "Bearer tokenValido";

            var result = _controller.Logout();

            _tokenRevocationServiceMock.Verify(service => service.RevokeToken("tokenValido"), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(result);
            dynamic response = okResult.Value;
            Assert.Equal("Logout realizado com sucesso.", response.message);
        }
    }
}
