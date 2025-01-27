using Xunit;
using Moq;
using APICatalogo.Controllers;
using APICatalogo.Repositories;
using APICatalogo.Models;
using APICatalogo.DTOs;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.Tests
{
    public class ProdutosControllerTests
    {
        private readonly Mock<IProdutoRepository> _produtoRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ProdutosController _controller;

        public ProdutosControllerTests()
        {
            _produtoRepositoryMock = new Mock<IProdutoRepository>();
            _mapperMock = new Mock<IMapper>();
            _controller = new ProdutosController(_produtoRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Get_ReturnsOkResult_WithListOfProdutoDTOs()
        {
            var produtos = new List<Produto> { new Produto { ProdutoId = 1, Nome = "Produto 1" } };
            var produtosDto = new List<ProdutoDTO> { new ProdutoDTO { ProdutoId = 1, Nome = "Produto 1" } };

            _produtoRepositoryMock.Setup(repo => repo.GetProdutosAsync()).ReturnsAsync(produtos);
            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<ProdutoDTO>>(produtos)).Returns(produtosDto);

            var result = await _controller.Get();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedProdutos = Assert.IsAssignableFrom<IEnumerable<ProdutoDTO>>(okResult.Value);
            Assert.Single(returnedProdutos);
            Assert.Equal(produtosDto.First().Nome, returnedProdutos.First().Nome);
        }


        [Fact]
        public async Task Get_ReturnsNotFound_WhenNoProdutosExist()
        {
            _produtoRepositoryMock.Setup(repo => repo.GetProdutosAsync()).ReturnsAsync(new List<Produto>());

            var result = await _controller.Get();

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Produtos não encontrados...", notFoundResult.Value);
        }


        [Fact]
        public async Task GetById_ReturnsOkResult_WithProdutoDTO()
        {
            var produto = new Produto { ProdutoId = 1, Nome = "Produto 1" };
            var produtoDto = new ProdutoDTO { ProdutoId = 1, Nome = "Produto 1" };

            _produtoRepositoryMock.Setup(repo => repo.GetProdutoByIdAsync(1)).ReturnsAsync(produto);
            _mapperMock.Setup(mapper => mapper.Map<ProdutoDTO>(produto)).Returns(produtoDto);

            var result = await _controller.Get(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedProduto = Assert.IsType<ProdutoDTO>(okResult.Value);
            Assert.Equal(produtoDto.Nome, returnedProduto.Nome);
        }


        [Fact]
        public async Task GetById_ReturnsNotFound_WhenProdutoDoesNotExist()
        {
            _produtoRepositoryMock.Setup(repo => repo.GetProdutoByIdAsync(1)).ReturnsAsync((Produto)null);

            var result = await _controller.Get(1);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Produto não encontrado ou excluído...", notFoundResult.Value);
        }


        [Fact]
        public async Task Post_ReturnsCreatedAtRouteResult_WithProdutoDTO()
        {
            var produtoDto = new ProdutoDTO { ProdutoId = 1, Nome = "Produto 1" };
            var produto = new Produto { ProdutoId = 1, Nome = "Produto 1" };

            _mapperMock.Setup(mapper => mapper.Map<Produto>(produtoDto)).Returns(produto);
            _produtoRepositoryMock.Setup(repo => repo.AddProdutoAsync(produto)).ReturnsAsync(produto);
            _mapperMock.Setup(mapper => mapper.Map<ProdutoDTO>(produto)).Returns(produtoDto);

            var result = await _controller.Post(produtoDto);

            var actionResult = Assert.IsType<ActionResult<ProdutoDTO>>(result);
            var createdAtRouteResult = Assert.IsType<CreatedAtRouteResult>(actionResult.Result);
            var returnedProduto = Assert.IsType<ProdutoDTO>(createdAtRouteResult.Value);
            Assert.Equal(produtoDto.Nome, returnedProduto.Nome);
        }


        [Fact]
        public async Task Put_ReturnsBadRequest_WhenIdsDoNotMatch()
        {
            var produtoDto = new ProdutoDTO { ProdutoId = 2, Nome = "Produto" };

            var result = await _controller.Put(1, produtoDto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("IDs não correspondem.", badRequestResult.Value);
        }


        [Fact]
        public async Task Delete_ReturnsOkResult_WhenProdutoIsDeleted()
        {
            _produtoRepositoryMock.Setup(repo => repo.DeleteProdutoAsync(1)).ReturnsAsync(true);

            var result = await _controller.Delete(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("excluído com sucesso", okResult.Value.ToString());
        }
        

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenProdutoDoesNotExist()
        {
            _produtoRepositoryMock.Setup(repo => repo.DeleteProdutoAsync(1)).ReturnsAsync(false);

            var result = await _controller.Delete(1);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
