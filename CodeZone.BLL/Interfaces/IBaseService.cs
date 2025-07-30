using FluentValidation.Results;

namespace CodeZone.BLL.Interfaces
{
    public interface IBaseService
    {
        string GetValidationErrorMessage(ValidationResult validation);
        string GetSuccessMessage(string operation, string? entity = null);
    }
} 