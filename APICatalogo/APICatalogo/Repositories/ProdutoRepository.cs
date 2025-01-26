using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APICatalogo.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly AppDbContext _context;
        private readonly IDistributedCache _cache;

        public ProdutoRepository(AppDbContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<IEnumerable<Produto>> GetProdutosAsync()
        {
            const string cacheKey = "ProdutosCache";
            var produtosCache = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(produtosCache))
            {
                return JsonSerializer.Deserialize<IEnumerable<Produto>>(produtosCache) ?? new List<Produto>();
            }

            var produtos = await _context.Produtos
                .Where(p => !p.Deletado)
                .ToListAsync();

            if (produtos.Any())
            {
                var serializedProdutos = JsonSerializer.Serialize(produtos);
                await _cache.SetStringAsync(cacheKey, serializedProdutos, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                });
            }

            return produtos;
        }


        public async Task<(IEnumerable<Produto>, int)> GetProdutosPaginadosAsync(int pageNumber, int pageSize)
        {
            string cacheKey = $"ProdutosCache_Page{pageNumber}_Size{pageSize}";
            var produtosCache = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(produtosCache))
            {
                var produtosPaginados = JsonSerializer.Deserialize<IEnumerable<Produto>>(produtosCache);
                var totalProdutos = await _context.Produtos.CountAsync(p => !p.Deletado);
                return (produtosPaginados ?? new List<Produto>(), totalProdutos);
            }

            var produtos = await _context.Produtos
                .Where(p => !p.Deletado)
                .OrderBy(p => p.ProdutoId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            if (produtos.Any())
            {
                var serializedProdutos = JsonSerializer.Serialize(produtos);
                await _cache.SetStringAsync(cacheKey, serializedProdutos, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                });
            }

            var totalCount = await _context.Produtos.CountAsync(p => !p.Deletado);

            return (produtos, totalCount);
        }


        public async Task<IEnumerable<Produto>> BuscarPorNomeAsync(string nome)
        {
            string cacheKey = $"ProdutosPorNome_{nome.ToLower()}";
            var produtosCache = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(produtosCache))
            {
                return JsonSerializer.Deserialize<IEnumerable<Produto>>(produtosCache) ?? new List<Produto>();
            }

            var produtos = await _context.Produtos
                .Where(p => EF.Functions.Like(p.Nome!, $"%{nome}%") && !p.Deletado)
                .ToListAsync();

            if (produtos.Any())
            {
                var serializedProdutos = JsonSerializer.Serialize(produtos);
                await _cache.SetStringAsync(cacheKey, serializedProdutos, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                });
            }

            return produtos;
        }

        public async Task<Produto?> GetProdutoByIdAsync(int id)
        {
            var cacheKey = $"Produto_{id}";
            var produtoCache = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(produtoCache))
            {
                return JsonSerializer.Deserialize<Produto>(produtoCache);
            }

            var produto = await _context.Produtos
                .FirstOrDefaultAsync(p => p.ProdutoId == id && !p.Deletado);

            if (produto != null)
            {
                var serializedProduto = JsonSerializer.Serialize(produto);
                await _cache.SetStringAsync(cacheKey, serializedProduto, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                });
            }

            return produto;
        }

        public async Task<Produto> AddProdutoAsync(Produto produto)
        {
            await _context.Produtos.AddAsync(produto);
            await _context.SaveChangesAsync();
            await _cache.RemoveAsync("ProdutosCache");

            return produto;
        }

        public async Task<Produto> UpdateProdutoAsync(Produto produto)
        {
            var produtoExistente = await _context.Produtos
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProdutoId == produto.ProdutoId && !p.Deletado);

            if (produtoExistente == null)
            {
                throw new KeyNotFoundException($"Produto com ID {produto.ProdutoId} não encontrado ou excluído...");
            }

            _context.Entry(produto).State = EntityState.Modified;

            var updated = await _context.SaveChangesAsync() > 0;

            if (updated)
            {
                await _cache.RemoveAsync("ProdutosCache");
                var cacheKey = $"Produto_{produto.ProdutoId}";
                var serializedProduto = JsonSerializer.Serialize(produto);
                await _cache.SetStringAsync(cacheKey, serializedProduto, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                });
            }

            return produto;
        }


        public async Task<bool> DeleteProdutoAsync(int id)
        {
            var produto = await _context.Produtos
                .FirstOrDefaultAsync(p => p.ProdutoId == id && !p.Deletado);

            if (produto == null) return false;

            produto.Deletado = true;
            _context.Entry(produto).State = EntityState.Modified;
            var deleted = await _context.SaveChangesAsync() > 0;

            if (deleted)
            {
                await _cache.RemoveAsync("ProdutosCache");
                var cacheKey = $"Produto_{id}";
                await _cache.RemoveAsync(cacheKey);
            }

            return deleted;
        }
    }
}
