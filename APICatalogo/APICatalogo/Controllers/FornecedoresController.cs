using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using APICatalogo.Models;
using APICatalogo.Repositories;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FornecedoresController : Controller
    {
        private readonly IFornecedorRepository _fornecedorRepository;

        public FornecedoresController(IFornecedorRepository fornecedorRepository)
        {
            _fornecedorRepository = fornecedorRepository;
        }

        [HttpGet]
        public IActionResult GetAllFornecedores()
        {
            var fornecedores = _fornecedorRepository.GetAllFornecedores();
            return Ok(fornecedores);
        }
    }
}
