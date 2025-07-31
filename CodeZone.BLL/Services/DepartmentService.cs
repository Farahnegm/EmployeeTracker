
using CodeZone.BLL.Interfaces;
using CodeZone.DAL.Entities;
using CodeZone.DAL.Interface;
using FluentValidation;
using FluentValidation.Results;

namespace CodeZone.BLL.Services
{
    public class DepartmentService : BaseService, IDepartmentService
    {
        private readonly IDepartmentRepository _repository;
        private readonly IValidator<Department> _validator;

        public DepartmentService(IDepartmentRepository repository, IValidator<Department> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<List<Department>> GetDepartmentsAsync()
            => await _repository.GetAllWithEmployeeCountAsync();

        public async Task<Department?> GetDepartmentAsync(int id)
            => await _repository.GetByIdAsync(id);

        public async Task<(Department? department, ValidationResult validation)> CreateAsync(Department department)
        {
            var validationResult = await _validator.ValidateAsync(department);
            if (!validationResult.IsValid)
            {
                return (null, validationResult);
            }

            var exists = await _repository.GetByNameOrCodeAsync(department.Name, department.Code);
            if (exists != null)
            {
                validationResult.Errors.Add(new ValidationFailure("Name", "Department name or code already exists."));
                return (null, validationResult);
            }

            await _repository.AddAsync(department);
            return (department, validationResult);
        }

        public async Task<(bool success, ValidationResult validation)> UpdateAsync(Department department)
        {
            var validationResult = await _validator.ValidateAsync(department);
            if (!validationResult.IsValid)
            {
                return (false, validationResult);
            }

            var exists = await _repository.GetByNameOrCodeAsync(department.Name, department.Code);
            if (exists != null && exists.DepartmentId != department.DepartmentId)
            {
                validationResult.Errors.Add(new ValidationFailure("Name", "Department name or code already exists."));
                return (false, validationResult);
            }

            await _repository.UpdateAsync(department);
            return (true, validationResult);
        }

        public async Task<(bool success, ValidationResult validation)> DeleteAsync(int id)
        {
            var validationResult = new ValidationResult();

            var dept = await _repository.GetByIdAsync(id);
            if (dept == null)
            {
                validationResult.Errors.Add(new ValidationFailure("Id", "Department not found."));
                return (false, validationResult);
            }

            if (dept.Employees != null && dept.Employees.Any())
            {
                validationResult.Errors.Add(new ValidationFailure("Name", "Cannot delete department. It has associated employees."));
                return (false, validationResult);
            }

            await _repository.DeleteAsync(dept);
            return (true, validationResult);
        }
    }
}
