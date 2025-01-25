using System.Collections.Generic;
using System.Linq;
using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly AppDbContext _context;

        public CategoriaRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Categoria> GetAllCategorias() => _context.Categorias.Include(c => c.Produtos).ToList();

        public Categoria GetCategoriaById(int id) =>
            _context.Categorias.Include(c => c.Produtos).FirstOrDefault(c => c.CategoriaId == id);
    }
}
