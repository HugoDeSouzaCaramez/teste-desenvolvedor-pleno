using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : Controller
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasComProdutos()
        {
            var categorias = _context.Categorias
                .Where(c => !c.Deletado)
                .Include(c => c.Produtos.Where(p => !p.Deletado))
                .ToList();

            if (categorias == null || !categorias.Any())
            {
                return NotFound("Categorias não encontradas...");
            }

            return Ok(categorias);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            var categorias = _context.Categorias
                .Where(c => !c.Deletado)
                .ToList();

            if (categorias is null)
            {
                return NotFound("Categorias não encontradas...");
            }

            return categorias;
        }

        [HttpGet("{id:int}", Name="ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            var categoria = _context.Categorias.FirstOrDefault(c => c.CategoriaId == id && !c.Deletado);
            
            if (categoria == null)
            {
                return NotFound("Categoria não encontrada...");
            }

            return Ok(categoria);
        }

        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            if (categoria == null)
                return BadRequest();

            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterCategoria",
            new { id = categoria.CategoriaId }, categoria);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Categoria categoria)
        {
            if (id != categoria.CategoriaId)
            {
                return BadRequest();
            }

            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var categoria = _context.Categorias
                                    .FirstOrDefault(c => c.CategoriaId == id && !c.Deletado);

            if (categoria is null)
            {
                return NotFound("Categoria não localizada ou já excluída...");
            }

            categoria.Deletado = true;
            _context.Entry(categoria).State = EntityState.Modified;

            _context.Produtos
                    .Where(p => p.CategoriaId == id && !p.Deletado)
                    .ExecuteUpdate(p => p.SetProperty(prod => prod.Deletado, true)); 

            _context.SaveChanges();

            return Ok("Categoria e produtos relacionados foram marcados como excluídos.");
        }
    }
}