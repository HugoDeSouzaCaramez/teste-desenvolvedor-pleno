using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using APICatalogo.Controllers;
using APICatalogo.Models;
using APICatalogo.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace APICatalogo.Tests
{
    public class CategoriasControllerTests
    {
        private readonly Mock<ICategoriaRepository> _categoriaRepositoryMock;
        private readonly CategoriasController _controller;

        public CategoriasControllerTests()
        {
            _categoriaRepositoryMock = new Mock<ICategoriaRepository>();
            _controller = new CategoriasController(_categoriaRepositoryMock.Object);
        }

        [Fact]
        public void GetAllCategorias_ReturnsOkResult_WithListOfCategorias()
        {
            var categorias = new List<Categoria>
            {
                new Categoria { CategoriaId = 1, Nome = "Categoria 1", Descricao = "Descrição 1" },
                new Categoria { CategoriaId = 2, Nome = "Categoria 2", Descricao = "Descrição 2" }
            };
            _categoriaRepositoryMock.Setup(repo => repo.GetAllCategorias()).Returns(categorias);

            var result = _controller.GetAllCategorias();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCategorias = Assert.IsAssignableFrom<IEnumerable<Categoria>>(okResult.Value);
            Assert.Equal(2, returnedCategorias.Count());
        }


        [Fact]
        public void GetCategoriaById_ReturnsOkResult_WithCategoria()
        {
            var categoria = new Categoria { CategoriaId = 1, Nome = "Categoria 1", Descricao = "Descrição 1" };
            _categoriaRepositoryMock.Setup(repo => repo.GetCategoriaById(1)).Returns(categoria);

            var result = _controller.GetCategoriaById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCategoria = Assert.IsType<Categoria>(okResult.Value);
            Assert.Equal("Categoria 1", returnedCategoria.Nome);
        }
        

        [Fact]
        public void GetCategoriaById_ReturnsNotFound_WhenCategoriaDoesNotExist()
        {
            _categoriaRepositoryMock.Setup(repo => repo.GetCategoriaById(1)).Returns((Categoria)null);

            var result = _controller.GetCategoriaById(1);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var message = Assert.IsType<string>(notFoundResult.Value.ToString());
            Assert.Contains("Categoria com ID 1 não encontrada", message);
        }
    }
}
