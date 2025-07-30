using FluentValidation;
using CodeZone.DAL.Entities;
using CodeZone.DAL.Data;
using System.Linq;

namespace CodeZone.BLL.Validation
{
    public class DepartmentValidator : AbstractValidator<Department>
    {
        public DepartmentValidator(AppDbContext db)
        {
            RuleFor(d => d.Name)
                .NotEmpty()
                .Length(3, 50)
                .Must((department, name) => !db.Departments.Any(dep => dep.Name == name && dep.DepartmentId != department.DepartmentId))
                .WithMessage("Department name must be unique.");

            RuleFor(d => d.Code)
                .NotEmpty()
                .Matches(@"^[A-Z]{4}$")
                .Must((department, code) => !db.Departments.Any(dep => dep.Code == code && dep.DepartmentId != department.DepartmentId))
                .WithMessage("Department code must be unique and exactly 4 uppercase letters.");

            RuleFor(d => d.Location)
                .NotEmpty()
                .MaximumLength(100);
        }
    }
} 