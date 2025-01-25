using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APICatalogo.Models
{
    public class Login
    {
        [Required(ErrorMessage= "O nome é obrigatório")]
        [StringLength(80, ErrorMessage = "O nome deve ter entre 3 e 80 caracteres", MinimumLength = 3)]
        public string Nome { get; set; } = string.Empty;
         
        [Required(ErrorMessage= "A senha é obrigatória")]
        [StringLength(80, ErrorMessage = "A senha deve ter entre 8 e 80 caracteres", MinimumLength = 8)]
        public string Senha { get; set; } = string.Empty;
    }
}