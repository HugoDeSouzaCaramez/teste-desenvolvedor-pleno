using System.Collections.Generic;
using System.Linq;
using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories
{
    public class FornecedorRepository : IFornecedorRepository
    {
        private readonly AppDbContext _context;

        public FornecedorRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Fornecedor> GetAllFornecedores() => _context.Fornecedores.Include(f => f.Produtos).ToList();

        public Fornecedor GetFornecedorById(int id) =>
            _context.Fornecedores.Include(f => f.Produtos).FirstOrDefault(f => f.FornecedorId == id);
    }
}
