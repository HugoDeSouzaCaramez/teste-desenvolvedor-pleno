using APICatalogo.Models;
using System.Collections.Generic;

namespace APICatalogo.Repositories
{
    public interface IUsuarioRepository
    {
        IEnumerable<Usuario> GetAllUsuarios();
        Usuario GetUsuarioById(int id);
        Usuario GetUsuarioByNome(string nome);
        void AddUsuario(Usuario usuario);
        void UpdateUsuario(Usuario usuario);
        void DeleteUsuario(Usuario usuario);
    }
}