using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using APICatalogo.Context;

namespace APICatalogo.Validations;

public class UniqueNameAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("O nome é obrigatório.");
        }

        var dbContext = (AppDbContext)validationContext.GetService(typeof(AppDbContext))!;
        var existingUser = dbContext.Usuarios!.AsNoTracking().FirstOrDefault(u => u.Nome == value.ToString());

        if (existingUser != null)
        {
            return new ValidationResult("O nome já está em uso.");
        }

        return ValidationResult.Success;
    }
}
