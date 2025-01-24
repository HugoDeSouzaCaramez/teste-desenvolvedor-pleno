using APICatalogo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APICatalogo.Repositories
{
    public interface IProdutoRepository
    {
        Task<IEnumerable<Produto>> GetProdutosAsync();
        Task<Produto?> GetProdutoByIdAsync(int id);
        Task<Produto> AddProdutoAsync(Produto produto);
        Task<bool> UpdateProdutoAsync(Produto produto);
        Task<bool> DeleteProdutoAsync(int id);
    }
}
