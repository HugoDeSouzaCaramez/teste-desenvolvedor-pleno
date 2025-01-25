using System.ComponentModel.DataAnnotations;
using APICatalogo.Context;

namespace APICatalogo.Validations
{
    public class ValidCategoriaIdAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("A categoria é obrigatória.");
            }

            var categoriaId = (int)value;

            var dbContext = (AppDbContext)validationContext.GetService(typeof(AppDbContext))!;
            
            var categoriaExists = dbContext.Categorias.Any(c => c.CategoriaId == categoriaId);

            if (!categoriaExists)
            {
                return new ValidationResult($"O CategoriaId {categoriaId} não é válido.");
            }

            return ValidationResult.Success;
        }
    }
}
