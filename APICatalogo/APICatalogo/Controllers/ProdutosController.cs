using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : Controller
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> Get()
        {
            var produtos = await _context.Produtos
                .Where(p => !p.Deletado)
                .ToListAsync();

            if (produtos == null || !produtos.Any())
            {
                return NotFound("Produtos não encontrados...");
            }

            return produtos;
        }

        [HttpGet("{id:int}", Name="ObterProduto")]
        public async Task<ActionResult<Produto>> Get(int id)
        {
            var produto = await _context.Produtos
                .FirstOrDefaultAsync(p => p.ProdutoId == id && !p.Deletado);

            if (produto == null)
            {
                return NotFound("Produto não encontrado...");
            }

            return produto;
        }

        [HttpPost]
        public async Task<ActionResult> Post(Produto produto)
        {
            if (produto == null)
                return BadRequest();

            await _context.Produtos.AddAsync(produto);
            await _context.SaveChangesAsync();

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

            return Ok(produto);
        }
    }
}
