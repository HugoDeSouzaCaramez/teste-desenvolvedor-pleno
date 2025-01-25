using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using APICatalogo.Models;
using APICatalogo.Repositories;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : Controller
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public CategoriasController(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        [HttpGet]
        public IActionResult GetAllCategorias()
        {
            var categorias = _categoriaRepository.GetAllCategorias();
            return Ok(categorias);
        }
        

        [HttpGet("{id}")]
        public IActionResult GetCategoriaById(int id)
        {
            var categoria = _categoriaRepository.GetCategoriaById(id);
            if (categoria == null)
            {
                return NotFound(new { Message = $"Categoria com ID {id} não encontrada..." });
            }
            return Ok(categoria);
        }
    }
}
