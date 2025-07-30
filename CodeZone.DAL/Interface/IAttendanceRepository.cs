using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeZone.DAL.Entities;

namespace CodeZone.DAL.Interface
{
    public interface IAttendanceRepository
    {
        Task<IEnumerable<Attendance>> GetAllAsync();
        Task<Attendance?> GetByIdAsync(int id);
        Task<Attendance?> GetByEmployeeAndDateAsync(int employeeId, DateTime date);
        Task AddAsync(Attendance attendance);
        Task UpdateAsync(Attendance attendance);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int employeeId, DateTime date);
        Task<bool> ExistsAsync(int employeeId, DateTime date, int excludeId);
        Task<IEnumerable<Attendance>> FilterAsync(int? departmentId, int? employeeId, DateTime? from, DateTime? to);
    }
}
