using CodeZone.BLL.DTOs;
using FluentValidation.Results;

namespace CodeZone.BLL.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();
        Task<EmployeeDto?> GetEmployeeByIdAsync(int id);
        Task<(EmployeeDto? employee, ValidationResult validation)> AddEmployeeAsync(EmployeeDto employeeDto);
        Task<(bool success, ValidationResult validation)> UpdateEmployeeAsync(int id, EmployeeDto dto);
        Task DeleteEmployeeAsync(int id);
        Task<(int present, int absent, double percentage)> GetEmployeeAttendanceStats(int id);
    }
}
