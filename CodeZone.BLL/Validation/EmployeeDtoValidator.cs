using FluentValidation;
using CodeZone.BLL.DTOs;
using System.Text.RegularExpressions;
using CodeZone.DAL.Data;
using System.Linq;

namespace CodeZone.BLL.Validation
{
    public class EmployeeDtoValidator : AbstractValidator<EmployeeDto>
    {
        public EmployeeDtoValidator(AppDbContext db)
        {
            RuleFor(e => e.FullName)
                .NotEmpty()
                .Must(BeFourValidNames).WithMessage("Full name must be 4 names, each at least 2 letters.");
            
            RuleFor(e => e.Email)
                .NotEmpty()
                .EmailAddress().WithMessage("Email is not valid.")
                .Must(email => 
                    email != null && 
                    System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$")
                ).WithMessage("Email must be a valid email address with a domain (e.g., user@example.com).")
                .Must((dto, email) =>
                    !db.Employees.Any(emp => emp.Email.ToLower().Trim() == email.ToLower().Trim() && emp.Id != dto.Id)
                )
                .WithMessage("Email must be unique.");

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
