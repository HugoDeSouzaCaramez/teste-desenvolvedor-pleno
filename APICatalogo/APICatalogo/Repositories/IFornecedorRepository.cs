using System.Collections.Generic;
using APICatalogo.Models;

namespace APICatalogo.Repositories
{
    public interface IFornecedorRepository
    {
        IEnumerable<Fornecedor> GetAllFornecedores();
        Fornecedor GetFornecedorById(int id);
    }
}
