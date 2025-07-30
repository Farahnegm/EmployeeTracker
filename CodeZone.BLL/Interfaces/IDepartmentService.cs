using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeZone.DAL.Entities;
using FluentValidation.Results;

namespace CodeZone.BLL.Interfaces
{
    public interface IDepartmentService : IBaseService
    {
        Task<List<Department>> GetDepartmentsAsync();
        Task<Department?> GetDepartmentAsync(int id);
        Task<(Department? department, ValidationResult validation)> CreateAsync(Department department);
        Task<(bool success, ValidationResult validation)> UpdateAsync(Department department);
        Task<(bool success, ValidationResult validation)> DeleteAsync(int id);
    }
}
