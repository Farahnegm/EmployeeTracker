using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeZone.BLL.DTOs;
using CodeZone.DAL.Entities;
using FluentValidation.Results;

namespace CodeZone.BLL.Interfaces
{
    public interface IAttendanceService : IBaseService
    {
        Task<IEnumerable<Attendance>> GetAllAsync();
        Task<Attendance?> GetByIdAsync(int id);
        Task<AttendanceDto?> GetDtoByIdAsync(int id);
        Task<Attendance?> GetByEmployeeAndDateAsync(int employeeId, DateTime date);
        Task<(Attendance? attendance, ValidationResult validation)> AddAsync(AttendanceDto dto);
        Task<(bool success, ValidationResult validation)> UpdateAsync(int id, AttendanceDto dto);
        Task<(bool success, ValidationResult validation)> UpdateStatusAsync(int id, string status);
        Task<(bool success, ValidationResult validation)> QuickUpdateAsync(int employeeId, DateTime date, string status);
        Task DeleteAsync(int id);
        Task<IEnumerable<Attendance>> FilterAsync(int? deptId, int? empId, DateTime? from, DateTime? to);
        Task<PaginationDto<Attendance>> GetAttendancesPaginatedAsync(int page = 1, int pageSize = 4);
        Task<PaginationDto<Attendance>> GetFilteredAttendancesPaginatedAsync(int? deptId, int? empId, DateTime? from, DateTime? to, int page = 1, int pageSize = 4);
    }
}
