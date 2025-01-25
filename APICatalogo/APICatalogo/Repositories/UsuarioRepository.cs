using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace APICatalogo.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Usuario> GetAllUsuarios() => _context.Usuarios.ToList();

        public Usuario GetUsuarioById(int id) =>
            _context.Usuarios.FirstOrDefault(u => u.UsuarioId == id);

        public Usuario GetUsuarioByNome(string nome) => _context.Usuarios.FirstOrDefault(u => u.Nome == nome);


        public void AddUsuario(Usuario usuario)
        {
            usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
        }

        public void UpdateUsuario(Usuario usuario)
        {
            var trackedEntity = _context.Usuarios.Local.FirstOrDefault(u => u.UsuarioId == usuario.UsuarioId);

            if (trackedEntity != null)
            {
                _context.Entry(trackedEntity).State = EntityState.Detached;
            }

            if (!string.IsNullOrEmpty(usuario.Senha))
            {
                usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);
            }

            _context.Entry(usuario).State = EntityState.Modified;
            _context.SaveChanges();
        }


        public void DeleteUsuario(Usuario usuario)
        {
            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();
        }
    }
}
