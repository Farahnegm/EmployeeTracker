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
    public interface IAttendanceService
    {
        Task<IEnumerable<Attendance>> GetAllAsync();
        Task<Attendance?> GetByIdAsync(int id);
        Task<AttendanceDto?> GetDtoByIdAsync(int id);
        Task<(Attendance? attendance, ValidationResult validation)> AddAsync(AttendanceDto dto);
        Task<(bool success, ValidationResult validation)> UpdateAsync(int id, AttendanceDto dto);
        Task<(bool success, ValidationResult validation)> UpdateStatusAsync(int id, string status);
        Task DeleteAsync(int id);
        Task<IEnumerable<Attendance>> FilterAsync(int? deptId, int? empId, DateTime? from, DateTime? to);
    }
}
