using System.Collections.Generic;
using APICatalogo.Models;

namespace APICatalogo.Repositories
{
    public interface ICategoriaRepository
    {
        IEnumerable<Categoria> GetAllCategorias();
        Categoria GetCategoriaById(int id);
    }
}
