using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using APICatalogo.Controllers;
using APICatalogo.Models;
using APICatalogo.Repositories;

namespace APICatalogo.Tests
{
    public class UsuariosControllerTests
    {
        private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
        private readonly UsuariosController _controller;

        public UsuariosControllerTests()
        {
            _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
            _controller = new UsuariosController(_usuarioRepositoryMock.Object);
        }

        [Fact]
        public void GetAllUsuarios_ReturnsOk_WithListOfUsuarios()
        {
            var usuarios = new List<Usuario>
            {
                new Usuario { UsuarioId = 1, Nome = "Usuario1", Senha = "Senha1" },
                new Usuario { UsuarioId = 2, Nome = "Usuario2", Senha = "Senha2" }
            };
            _usuarioRepositoryMock.Setup(repo => repo.GetAllUsuarios()).Returns(usuarios);

            var result = _controller.GetAllUsuarios();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUsuarios = Assert.IsType<List<Usuario>>(okResult.Value);
            Assert.Equal(2, returnedUsuarios.Count);
        }


        [Fact]
        public void GetUsuarioById_ReturnsOk_WhenUsuarioExists()
        {
            var usuario = new Usuario { UsuarioId = 1, Nome = "Usuario1", Senha = "Senha1" };
            _usuarioRepositoryMock.Setup(repo => repo.GetUsuarioById(1)).Returns(usuario);

            var result = _controller.GetUsuarioById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUsuario = Assert.IsType<Usuario>(okResult.Value);
            Assert.Equal("Usuario1", returnedUsuario.Nome);
        }


        [Fact]
        public void GetUsuarioById_ReturnsNotFound_WhenUsuarioDoesNotExist()
        {
            _usuarioRepositoryMock.Setup(repo => repo.GetUsuarioById(1)).Returns((Usuario)null);

            var result = _controller.GetUsuarioById(1);

            Assert.IsType<NotFoundResult>(result);
        }


        [Fact]
        public void CreateUsuario_ReturnsCreatedAtAction_WhenUsuarioIsValid()
        {
            var usuario = new Usuario { UsuarioId = 1, Nome = "NovoUsuario", Senha = "Senha123" };

            var result = _controller.CreateUsuario(usuario);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnedUsuario = Assert.IsType<Usuario>(createdAtActionResult.Value);
            Assert.Equal("NovoUsuario", returnedUsuario.Nome);
            _usuarioRepositoryMock.Verify(repo => repo.AddUsuario(It.IsAny<Usuario>()), Times.Once);
        }


        [Fact]
        public void CreateUsuario_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            _controller.ModelState.AddModelError("Nome", "O nome é obrigatório");

            var result = _controller.CreateUsuario(new Usuario());

            Assert.IsType<BadRequestObjectResult>(result);
        }


        [Fact]
        public void UpdateUsuario_ReturnsNoContent_WhenUsuarioIsValid()
        {
            var usuario = new Usuario { UsuarioId = 1, Nome = "UsuarioAtualizado", Senha = "SenhaNova" };
            _usuarioRepositoryMock.Setup(repo => repo.GetUsuarioById(1)).Returns(usuario);

            var result = _controller.UpdateUsuario(1, usuario);

            Assert.IsType<NoContentResult>(result);
            _usuarioRepositoryMock.Verify(repo => repo.UpdateUsuario(It.IsAny<Usuario>()), Times.Once);
        }


        [Fact]
        public void UpdateUsuario_ReturnsNotFound_WhenUsuarioDoesNotExist()
        {
            var usuario = new Usuario { UsuarioId = 1, Nome = "UsuarioAtualizado", Senha = "SenhaNova" };
            _usuarioRepositoryMock.Setup(repo => repo.GetUsuarioById(1)).Returns((Usuario)null);

            var result = _controller.UpdateUsuario(1, usuario);

            Assert.IsType<NotFoundResult>(result);
        }


        [Fact]
        public void UpdateUsuario_ReturnsBadRequest_WhenIdMismatch()
        {
            var usuario = new Usuario { UsuarioId = 2, Nome = "UsuarioAtualizado", Senha = "SenhaNova" };

            var result = _controller.UpdateUsuario(1, usuario);

            Assert.IsType<BadRequestObjectResult>(result);
        }


        [Fact]
        public void DeleteUsuario_ReturnsNoContent_WhenUsuarioExists()
        {
            var usuario = new Usuario { UsuarioId = 1, Nome = "UsuarioParaDeletar", Senha = "Senha123" };
            _usuarioRepositoryMock.Setup(repo => repo.GetUsuarioById(1)).Returns(usuario);

            var result = _controller.DeleteUsuario(1);

            Assert.IsType<NoContentResult>(result);
            _usuarioRepositoryMock.Verify(repo => repo.DeleteUsuario(It.IsAny<Usuario>()), Times.Once);
        }
        

        [Fact]
        public void DeleteUsuario_ReturnsNotFound_WhenUsuarioDoesNotExist()
        {
            _usuarioRepositoryMock.Setup(repo => repo.GetUsuarioById(1)).Returns((Usuario)null);

            var result = _controller.DeleteUsuario(1);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
