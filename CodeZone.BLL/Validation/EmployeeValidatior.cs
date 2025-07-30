
using System.Text.RegularExpressions;
using FluentValidation;
using CodeZone.DAL.Entities;
using CodeZone.DAL.Data;
using System.Linq;

namespace CodeZone.BLL.Validation
{
    public class EmployeeValidator : AbstractValidator<Employee>
    {
        public EmployeeValidator(AppDbContext db)
        {
            RuleFor(e => e.FullName)
                .NotEmpty()
                .Must(BeFourValidNames).WithMessage("Full name must be 4 names, each at least 2 letters.");

            RuleFor(e => e.Email)
                .NotEmpty()
                .EmailAddress().WithMessage("Email is not valid.")
                .Must((employee, email) => !db.Employees.Any(emp => emp.Email == email && emp.Id != employee.Id))
                .WithMessage("Email must be unique.");

            RuleFor(e => e.DepartmentId)
                .Must(departmentId => db.Departments.Any(d => d.DepartmentId == departmentId))
                .WithMessage("Department must exist.");
        }

        private bool BeFourValidNames(string fullName)
        {
            var names = fullName?.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (names?.Length != 4) return false;
            return names.All(name => name.Length >= 2 && Regex.IsMatch(name, @"^[A-Za-z]+$"));
        }
    }
}
