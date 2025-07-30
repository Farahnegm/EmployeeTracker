using FluentValidation;
using CodeZone.BLL.DTOs;
using System.Text.RegularExpressions;

namespace CodeZone.BLL.Validation
{
    public class EmployeeDtoValidator : AbstractValidator<EmployeeDto>
    {
        public EmployeeDtoValidator()
        {
            RuleFor(e => e.FullName)
                .NotEmpty()
                .Must(BeFourValidNames).WithMessage("Full name must be 4 names, each at least 2 letters.");
            
            RuleFor(e => e.Email)
                .NotEmpty()
                .EmailAddress().WithMessage("Email is not valid.");

            RuleFor(e => e.DepartmentId)
                .GreaterThan(0).WithMessage("Department must be selected.");
        }

        private bool BeFourValidNames(string fullName)
        {
            var names = fullName?.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (names?.Length != 4) return false;
            return names.All(name => name.Length >= 2 && Regex.IsMatch(name, @"^[A-Za-z]+$"));
        }
    }
}
