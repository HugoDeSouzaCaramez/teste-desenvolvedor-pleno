using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Xunit;

public class ProdutoRepositoryTests
{
    private readonly ProdutoRepository _repository;
    private readonly Mock<IDistributedCache> _cacheMock;
    private readonly AppDbContext _context;

    public ProdutoRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new AppDbContext(options);

        _cacheMock = new Mock<IDistributedCache>();
        _repository = new ProdutoRepository(_context, _cacheMock.Object);
    }

    private Produto CriarProdutoPadrao(
        int produtoId = 1,
        string nome = "Produto Padrão",
        decimal preco = 10m,
        int categoriaId = 1,
        string descricao = "Descrição padrão",
        int fornecedorId = 1,
        string imagemUrl = "http://teste.com/imagem.jpg")
    {
        return new Produto
        {
            ProdutoId = produtoId,
            Nome = nome,
            Preco = preco,
            CategoriaId = categoriaId,
            Descricao = descricao,
            FornecedorId = fornecedorId,
            ImagemUrl = imagemUrl
        };
    }


    [Fact]
    public async Task AddProdutoAsync_ShouldAddProdutoToDatabase_AndClearCache()
    {
        var produto = CriarProdutoPadrao(produtoId: 3, nome: "Produto Novo", preco: 50);

        var result = await _repository.AddProdutoAsync(produto);

        Assert.NotNull(result);
        Assert.Equal("Produto Novo", result.Nome);
        Assert.Single(_context.Produtos);
        _cacheMock.Verify(c => c.RemoveAsync("ProdutosCache", It.IsAny<CancellationToken>()), Times.Once);
    }


    [Fact]
    public async Task UpdateProdutoAsync_ShouldUpdateProdutoInDatabase_AndRefreshCache()
    {
        var produto = CriarProdutoPadrao(produtoId: 1, nome: "Produto Atualizado", preco: 100);
        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();

        produto.Nome = "Produto Editado";

        var result = await _repository.UpdateProdutoAsync(produto);

        Assert.NotNull(result);
        Assert.Equal("Produto Editado", result.Nome);
        _cacheMock.Verify(c => c.RemoveAsync("ProdutosCache", It.IsAny<CancellationToken>()), Times.Once);
    }
    

    [Fact]
    public async Task DeleteProdutoAsync_ShouldMarkProdutoAsDeleted_AndClearCache()
    {
        var produto = CriarProdutoPadrao(produtoId: 1, nome: "Produto Deletado", preco: 30);
        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();

        var result = await _repository.DeleteProdutoAsync(produto.ProdutoId);

        Assert.True(result);
        var deletedProduto = await _context.Produtos.FindAsync(produto.ProdutoId);
        Assert.True(deletedProduto.Deletado);
        _cacheMock.Verify(c => c.RemoveAsync("ProdutosCache", It.IsAny<CancellationToken>()), Times.Once);
    }
}