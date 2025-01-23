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
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _context.Produtos
                .Where(p => !p.Deletado)
                .ToList();

            if (produtos is null)
            {
                return NotFound("Produtos não encontrados...");
            }

            return produtos;
        }

        [HttpGet("{id:int}", Name="ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id && !p.Deletado);
            if (produto == null)
            {
                return NotFound("Produto não encontrado...");
            }

            return produto;
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if (produto == null)
                return BadRequest();

            _context.Produtos.Add(produto);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterProduto",
            new { id = produto.ProdutoId }, produto);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if (id != produto.ProdutoId)
            {
                return BadRequest();
            }

            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id && !p.Deletado);

            if (produto is null)
            {
                return NotFound("Produto não localizado ou já excluído...");
            }

            produto.Deletado = true;
            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(produto);
        }
    }
}