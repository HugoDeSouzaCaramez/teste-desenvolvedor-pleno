using System.ComponentModel.DataAnnotations;
using APICatalogo.Validations;

namespace APICatalogo.DTOs;

public class ProdutoDTO
{
    public int ProdutoId { get; set; }

    [Required(ErrorMessage= "O nome é obrigatório")]
    [StringLength(80, ErrorMessage = "O nome deve ter entre 3 e 80 caracteres", MinimumLength = 3)]
    public string? Nome { get; set; }

    [Required]
    [StringLength(300, ErrorMessage = "A descrição deve ter entre 5 e 300 caracteres", MinimumLength = 5)]
    public string? Descricao { get; set; }

    [Required]
    [Range(1,99999, ErrorMessage ="O preço deve estar entre {1} e {2}")]
    public decimal Preco { get; set; }

    [Required]
    [StringLength(300)]
    public string? ImagemUrl { get; set; }

    [Range(0, 99999, ErrorMessage = "O estoque deve estar entre {1} e {2}")]
    public int Estoque { get; set; }

    [Required(ErrorMessage = "A categoria é obrigatória")]
    [Range(1, int.MaxValue, ErrorMessage = "A categoria deve ser válida e maior que zero")]
    [ValidCategoriaId(ErrorMessage = "A categoria informada não é válida.")]
    public int? CategoriaId { get; set; }
    public string? CategoriaNome { get; set; }

    [Required(ErrorMessage = "O fornecedor é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "O fornecedor deve ser válido e maior que zero")]
    [ValidFornecedorId(ErrorMessage = "O fornecedor informado não é válido.")]
    public int? FornecedorId { get; set; }
    public string? FornecedorNome { get; set; }

    public bool Deletado { get; set; } = false;
}
