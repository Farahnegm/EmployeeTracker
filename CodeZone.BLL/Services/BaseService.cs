using CodeZone.BLL.Interfaces;
using FluentValidation.Results;

namespace CodeZone.BLL.Services
{
    public abstract class BaseService : IBaseService
    {
        public string GetValidationErrorMessage(ValidationResult validation)
        {
            return string.Join(", ", validation.Errors.Select(e => e.ErrorMessage));
        }

        public string GetSuccessMessage(string operation, string? entity = null)
        {
            return entity != null 
                ? $"{entity} {operation} successfully!" 
                : $"{operation} successfully!";
        }
    }
} 