using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : Controller
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutosController(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> Get()
        {
            var produtos = await _produtoRepository.GetProdutosAsync();
            if (!produtos.Any())
            {
                return NotFound("Produtos não encontrados...");
            }
            return Ok(produtos);
        }

        [HttpGet("{id:int}", Name = "ObterProduto")]
        public async Task<ActionResult<Produto>> Get(int id)
        {
            var produto = await _produtoRepository.GetProdutoByIdAsync(id);
            if (produto == null)
            {
                return NotFound("Produto não encontrado...");
            }
            return Ok(produto);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Produto produto)
        {
            if (produto == null)
            {
                return BadRequest();
            }

            var novoProduto = await _produtoRepository.AddProdutoAsync(produto);
            return new CreatedAtRouteResult("ObterProduto", new { id = novoProduto.ProdutoId }, novoProduto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, Produto produto)
        {
            if (id != produto.ProdutoId)
            {
                return BadRequest();
            }

            var updated = await _produtoRepository.UpdateProdutoAsync(produto);
            if (!updated)
            {
                return NotFound("Produto não encontrado...");
            }

            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _produtoRepository.DeleteProdutoAsync(id);
            if (!deleted)
            {
                return NotFound("Produto não localizado ou já excluído...");
            }

            return Ok($"Produto com ID {id} excluído com sucesso.");
        }
    }
}
