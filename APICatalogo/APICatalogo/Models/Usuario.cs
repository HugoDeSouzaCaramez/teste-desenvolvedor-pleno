using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using APICatalogo.Validations;

namespace APICatalogo.Models;

[Table("Usuarios")]
public class Usuario
{
    [Key]
    public int UsuarioId { get; set; }

    [Required(ErrorMessage= "O nome é obrigatório")]
    [StringLength(80, ErrorMessage = "O nome deve ter entre 3 e 80 caracteres", MinimumLength = 3)]
    [UniqueName(ErrorMessage = "O nome já está em uso.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage= "A senha é obrigatória")]
    [StringLength(80, ErrorMessage = "A senha deve ter entre 8 e 80 caracteres", MinimumLength = 8)]
    public string Senha { get; set; } = string.Empty;
}
