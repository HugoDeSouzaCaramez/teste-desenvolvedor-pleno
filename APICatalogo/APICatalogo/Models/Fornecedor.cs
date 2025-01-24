using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalogo.Models;

[Table("Fornecedores")]
public class Fornecedor
{
    public Fornecedor()
    {
        Produtos = new Collection<Produto>();
    }

    [Key]
    public int FornecedorId { get; set; }

    [Required]
    [StringLength(100)]
    public string? Nome { get; set; }

    [Required]
    [StringLength(14)]
    public string? Cnpj { get; set; }

    [StringLength(15)]
    public string? Telefone { get; set; }

    [StringLength(300)]
    public string? Endereco { get; set; }

    public bool Deletado { get; set; } = false;

    [JsonIgnore]
    public ICollection<Produto>? Produtos { get; set; }
}
