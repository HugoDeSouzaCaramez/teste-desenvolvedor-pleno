using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IDistributedCache _cache;

        public ProdutosController(AppDbContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> Get()
        {
            const string cacheKey = "ProdutosCache";
            var produtosCache = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(produtosCache))
            {
                var produtosDoCache = JsonSerializer.Deserialize<IEnumerable<Produto>>(produtosCache);
                return Ok(produtosDoCache);
            }

            var produtos = await _context.Produtos
                .Where(p => !p.Deletado)
                .ToListAsync();

            if (produtos == null || !produtos.Any())
            {
                return NotFound("Produtos não encontrados...");
            }

            var serializedProdutos = JsonSerializer.Serialize(produtos);
            await _cache.SetStringAsync(cacheKey, serializedProdutos, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });

            return Ok(produtos);
        }

        [HttpGet("{id:int}", Name = "ObterProduto")]
        public async Task<ActionResult<Produto>> Get(int id)
        {
            string cacheKey = $"Produto_{id}";
            var produtoCache = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(produtoCache))
            {
                var produtoDoCache = JsonSerializer.Deserialize<Produto>(produtoCache);
                return Ok(produtoDoCache);
            }

            var produto = await _context.Produtos
                .FirstOrDefaultAsync(p => p.ProdutoId == id && !p.Deletado);

            if (produto == null)
            {
                return NotFound("Produto não encontrado...");
            }

            var serializedProduto = JsonSerializer.Serialize(produto);
            await _cache.SetStringAsync(cacheKey, serializedProduto, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });

            return Ok(produto);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Produto produto)
        {
            if (produto == null)
                return BadRequest();

            await _context.Produtos.AddAsync(produto);
            await _context.SaveChangesAsync();
            await _cache.RemoveAsync("ProdutosCache");

            return new CreatedAtRouteResult("ObterProduto",
            new { id = produto.ProdutoId }, produto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, Produto produto)
        {
            if (id != produto.ProdutoId)
            {
                return BadRequest();
            }

            _context.Entry(produto).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            await _cache.RemoveAsync("ProdutosCache");

            var cacheKey = $"Produto_{id}";
            var serializedProduto = JsonSerializer.Serialize(produto);
            await _cache.SetStringAsync(cacheKey, serializedProduto, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });

            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var produto = await _context.Produtos
                .FirstOrDefaultAsync(p => p.ProdutoId == id && !p.Deletado);

            if (produto == null)
            {
                return NotFound("Produto não localizado ou já excluído...");
            }

            produto.Deletado = true;
            _context.Entry(produto).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            await _cache.RemoveAsync("ProdutosCache");

            var cacheKey = $"Produto_{id}";
            await _cache.RemoveAsync(cacheKey);

            return Ok(produto);
        }
    }
}
