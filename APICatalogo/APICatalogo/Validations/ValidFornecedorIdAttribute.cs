using System.ComponentModel.DataAnnotations;
using APICatalogo.Context;

namespace APICatalogo.Validations
{
    public class ValidFornecedorIdAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("O fornecedor é obrigatório.");
            }

            var fornecedorId = (int)value;

            var dbContext = (AppDbContext)validationContext.GetService(typeof(AppDbContext))!;
            
            var fornecedorExists = dbContext.Fornecedores.Any(f => f.FornecedorId == fornecedorId);

            if (!fornecedorExists)
            {
                return new ValidationResult($"O FornecedorId {fornecedorId} não é válido.");
            }

            return ValidationResult.Success;
        }
    }
}
