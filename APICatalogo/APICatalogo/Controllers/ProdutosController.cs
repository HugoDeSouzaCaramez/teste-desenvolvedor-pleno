using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;
using APICatalogo.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Cors;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [EnableCors("ReactPolicy")]
    public class ProdutosController : Controller
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMapper _mapper;

        public ProdutosController(IProdutoRepository produtoRepository, IMapper mapper)
        {
            _produtoRepository = produtoRepository;
            _mapper = mapper;
        }


        [HttpGet("todos")] 
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get()
        {
            var produtos = await _produtoRepository.GetProdutosAsync();
            if (!produtos.Any())
            {
                return NotFound("Produtos não encontrados...");
            }

            var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

            return Ok(produtosDto);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                return BadRequest("Os parâmetros pageNumber e pageSize devem ser maiores que 0.");
            }

            var (produtos, totalCount) = await _produtoRepository.GetProdutosPaginadosAsync(pageNumber, pageSize);

            if (!produtos.Any())
            {
                return NotFound("Nenhum produto encontrado para os parâmetros especificados.");
            }

            var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

            var response = new
            {
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                Data = produtosDto
            };

            return Ok(response);
        }


        [HttpGet("{id:int}", Name = "ObterProduto")]
        public async Task<ActionResult<ProdutoDTO>> Get(int id)
        {
            var produto = await _produtoRepository.GetProdutoByIdAsync(id);
            if (produto == null)
            {
                return NotFound("Produto não encontrado ou excluído...");
            }

            var produtoDto = _mapper.Map<ProdutoDTO>(produto);

            return Ok(produtoDto);
        }


        [HttpPost]
        public async Task<ActionResult<ProdutoDTO>> Post(ProdutoDTO produtoDto)
        {
            if (produtoDto == null)
            {
                return BadRequest();
            }

            var produto = _mapper.Map<Produto>(produtoDto);

            var novoProduto = await _produtoRepository.AddProdutoAsync(produto);

            var novoProdutoDto = _mapper.Map<ProdutoDTO>(novoProduto);

            return new CreatedAtRouteResult("ObterProduto", new { id = novoProdutoDto.ProdutoId }, novoProdutoDto);
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProdutoDTO>> Put(int id, ProdutoDTO produtoDto)
        {
            if (id != produtoDto.ProdutoId)
            {
                return BadRequest("IDs não correspondem.");
            }

            var produto = _mapper.Map<Produto>(produtoDto);

            try
            {
                var produtoAtualizado = await _produtoRepository.UpdateProdutoAsync(produto);
                var produtoAtualizadoDto = _mapper.Map<ProdutoDTO>(produtoAtualizado);

                return Ok(produtoAtualizadoDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
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
