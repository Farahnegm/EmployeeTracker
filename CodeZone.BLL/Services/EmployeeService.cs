
using CodeZone.BLL.DTOs;
using CodeZone.BLL.Interfaces;
using CodeZone.DAL.Entities;
using CodeZone.DAL.Interface;
using FluentValidation;
using FluentValidation.Results;

namespace CodeZone.BLL.Services
{
    public class EmployeeService : BaseService, IEmployeeService
    {
        private readonly IEmployeeRepository _repo;
        private readonly IValidator<EmployeeDto> _validator;

        public EmployeeService(IEmployeeRepository repo, IValidator<EmployeeDto> validator)
        {
            _repo = repo;
            _validator = validator;
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
        {
            var employees = await _repo.GetAllAsync();
            var employeeDtos = new List<EmployeeDto>();
            
            foreach (var e in employees)
            {
                var (present, absent, percentage) = await GetEmployeeAttendanceStats(e.Id);
                employeeDtos.Add(new EmployeeDto
                {
                    Id = e.Id,
                    EmployeeCode = e.EmployeeCode,
                    FullName = e.FullName,
                    Email = e.Email,
                    DepartmentId = e.DepartmentId,
                    DepartmentName = e.Department?.Name ?? string.Empty,
                    Presents = present,
                    Absents = absent,
                    AttendancePercentage = percentage
                });
            }
            
            return employeeDtos;
        }

        public async Task<PaginationDto<EmployeeDto>> GetEmployeesPaginatedAsync(int page = 1, int pageSize = 10)
        {
            var allEmployees = await _repo.GetAllAsync();
            var totalItems = allEmployees.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            
            // Ensure page is within valid range
            page = Math.Max(1, Math.Min(page, totalPages > 0 ? totalPages : 1));
            
            var pagedEmployees = allEmployees
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
            
            var employeeDtos = new List<EmployeeDto>();
            
            foreach (var e in pagedEmployees)
            {
                var (present, absent, percentage) = await GetEmployeeAttendanceStats(e.Id);
                employeeDtos.Add(new EmployeeDto
                {
                    Id = e.Id,
                    EmployeeCode = e.EmployeeCode,
                    FullName = e.FullName,
                    Email = e.Email,
                    DepartmentId = e.DepartmentId,
                    DepartmentName = e.Department?.Name ?? string.Empty,
                    Presents = present,
                    Absents = absent,
                    AttendancePercentage = percentage
                });
            }
            
            return new PaginationDto<EmployeeDto>
            {
                Items = employeeDtos,
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            };
        }

        public async Task<EmployeeDto?> GetEmployeeByIdAsync(int id)
        {
            var e = await _repo.GetByIdAsync(id);
            if (e == null) return null;

            var (present, absent, percentage) = await GetEmployeeAttendanceStats(e.Id);
            return new EmployeeDto
            {
                Id = e.Id,
                EmployeeCode = e.EmployeeCode,
                FullName = e.FullName,
                Email = e.Email,
                DepartmentId = e.DepartmentId,
                DepartmentName = e.Department?.Name ?? string.Empty,
                Presents = present,
                Absents = absent,
                AttendancePercentage = percentage
            };
        }

        public async Task<(bool success, ValidationResult validation)> UpdateEmployeeAsync(int id, EmployeeDto dto)
        {
            // Validate the DTO
            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return (false, validationResult);
            }

            var employee = await _repo.GetByIdAsync(id);
            if (employee is null)
            {
                validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("Id", "Employee not found."));
                return (false, validationResult);
            }

            // Check email uniqueness (excluding current employee)
            if (_repo.EmailExists(dto.Email) && employee.Email != dto.Email)
            {
                validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("Email", "Email must be unique."));
                return (false, validationResult);
            }

            employee.FullName = dto.FullName;
            employee.Email = dto.Email;
            employee.DepartmentId = dto.DepartmentId;

            await _repo.UpdateAsync(employee);
            return (true, validationResult);
        }

        public async Task DeleteEmployeeAsync(int id)
            => await _repo.DeleteAsync(id);

        public async Task<(int present, int absent, double percentage)> GetEmployeeAttendanceStats(int id)
        {
            var (present, absent) = await _repo.GetAttendanceSummary(id);
            var total = present + absent;
            double percentage = total == 0 ? 0 : (present * 100.0 / total);
            return (present, absent, percentage);
        }

        private async Task<int> GenerateUniqueEmployeeCode()
        {
            var employees = await _repo.GetAllAsync();
            return employees.Any() ? employees.Max(e => e.EmployeeCode) + 1 : 1000;
        }

        public async Task<(EmployeeDto? employee, ValidationResult validation)> AddEmployeeAsync(EmployeeDto dto)
        {
            // Validate the DTO
            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return (null, validationResult);
            }

            // Check email uniqueness
            if (_repo.EmailExists(dto.Email))
            {
                validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("Email", "Email must be unique."));
                return (null, validationResult);
            }

            var employee = new Employee
            {
                FullName = dto.FullName,
                Email = dto.Email,
                DepartmentId = dto.DepartmentId,
                EmployeeCode = await GenerateUniqueEmployeeCode()
            };
            
            await _repo.AddAsync(employee);
            
            // Return the created employee DTO with the generated EmployeeCode
            return (new EmployeeDto
            {
                Id = employee.Id,
                EmployeeCode = employee.EmployeeCode,
                FullName = employee.FullName,
                Email = employee.Email,
                DepartmentId = employee.DepartmentId,
                DepartmentName = dto.DepartmentName
            }, validationResult);
        }
    }
}
