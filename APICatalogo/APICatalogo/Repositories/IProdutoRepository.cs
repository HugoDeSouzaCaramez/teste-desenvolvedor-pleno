using APICatalogo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using APICatalogo.DTOs;

namespace APICatalogo.Repositories
{
    public interface IProdutoRepository
    {
        Task<IEnumerable<Produto>> GetProdutosAsync();
        Task<IEnumerable<ProdutoDTO>> GetProdutosComRelacionamentosAsync();
        Task<(IEnumerable<Produto>, int)> GetProdutosPaginadosAsync(int pageNumber, int pageSize);
        Task<Produto?> GetProdutoByIdAsync(int id);
        Task<Produto> AddProdutoAsync(Produto produto);
        Task<Produto> UpdateProdutoAsync(Produto produto);
        Task<bool> DeleteProdutoAsync(int id);
    }
}
