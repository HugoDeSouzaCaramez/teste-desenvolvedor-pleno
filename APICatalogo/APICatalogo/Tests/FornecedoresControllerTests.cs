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
    public class FornecedoresControllerTests
    {
        private readonly Mock<IFornecedorRepository> _fornecedorRepositoryMock;
        private readonly FornecedoresController _controller;

        public FornecedoresControllerTests()
        {
            _fornecedorRepositoryMock = new Mock<IFornecedorRepository>();
            _controller = new FornecedoresController(_fornecedorRepositoryMock.Object);
        }

        [Fact]
        public void GetAllFornecedores_ReturnsOkResult_WithListOfFornecedores()
        {
            var fornecedores = new List<Fornecedor>
            {
                new Fornecedor { FornecedorId = 1, Nome = "Fornecedor 1", Cnpj = "12345678000199" },
                new Fornecedor { FornecedorId = 2, Nome = "Fornecedor 2", Cnpj = "98765432000111" }
            };
            _fornecedorRepositoryMock.Setup(repo => repo.GetAllFornecedores()).Returns(fornecedores);

            var result = _controller.GetAllFornecedores();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedFornecedores = Assert.IsAssignableFrom<IEnumerable<Fornecedor>>(okResult.Value);
            Assert.Equal(2, returnedFornecedores.Count());
        }


        [Fact]
        public void GetFornecedorById_ReturnsOkResult_WithFornecedor()
        {
            var fornecedor = new Fornecedor { FornecedorId = 1, Nome = "Fornecedor 1", Cnpj = "12345678000199" };
            _fornecedorRepositoryMock.Setup(repo => repo.GetFornecedorById(1)).Returns(fornecedor);

            var result = _controller.GetFornecedorById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedFornecedor = Assert.IsType<Fornecedor>(okResult.Value);
            Assert.Equal("Fornecedor 1", returnedFornecedor.Nome);
        }
        

        [Fact]
        public void GetFornecedorById_ReturnsNotFound_WhenFornecedorDoesNotExist()
        {
            _fornecedorRepositoryMock.Setup(repo => repo.GetFornecedorById(1)).Returns((Fornecedor)null);

            var result = _controller.GetFornecedorById(1);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var message = Assert.IsType<string>(notFoundResult.Value.ToString());
            Assert.Contains("Fornecedor com ID 1 não encontrado", message);
        }
    }
}
