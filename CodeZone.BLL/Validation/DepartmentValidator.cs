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
                .NotEmpty().WithMessage("Department name is required.")
                .Length(3, 50).WithMessage("Department name must be between 3 and 50 characters.")
                .Must((department, name) => !db.Departments.Any(dep => dep.Name == name && dep.DepartmentId != department.DepartmentId))
                .WithMessage("This department name is already in use. Please choose a different name.");

            RuleFor(d => d.Code)
                .NotEmpty().WithMessage("Department code is required.")
                .Matches(@"^[A-Z]{4}$").WithMessage("Department code must be exactly 4 uppercase letters (A-Z).")
                .Must((department, code) => !db.Departments.Any(dep => dep.Code == code && dep.DepartmentId != department.DepartmentId))
                .WithMessage("This department code is already in use. Please choose a different code.");

            RuleFor(d => d.Location)
                .NotEmpty().WithMessage("Department location is required.")
                .MaximumLength(100).WithMessage("Department location must not exceed 100 characters.");
        }
    }
} 