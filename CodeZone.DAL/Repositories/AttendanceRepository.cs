using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeZone.DAL.Data;
using CodeZone.DAL.Entities;
using CodeZone.DAL.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodeZone.DAL.Repositories
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly AppDbContext _context;

        public AttendanceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Attendance>> GetAllAsync()
            => await _context.Attendances.Include(a => a.Employee).ThenInclude(e => e.Department).ToListAsync();

        public async Task<Attendance?> GetByIdAsync(int id)
            => await _context.Attendances.Include(a => a.Employee).ThenInclude(e => e.Department).FirstOrDefaultAsync(a => a.AttendanceId == id);

        public async Task AddAsync(Attendance attendance)
        {
            await _context.Attendances.AddAsync(attendance);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Attendance attendance)
        {
            _context.Attendances.Update(attendance);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity is not null)
            {
                _context.Attendances.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int employeeId, DateTime date)
        {
            var dateOnly = date.Date;

            return await _context.Attendances
                .AnyAsync(a => a.EmployeeId == employeeId && a.Date.Date == dateOnly);
        }

        public async Task<bool> ExistsAsync(int employeeId, DateTime date, int excludeId)
        {
            var dateOnly = date.Date;

            return await _context.Attendances
                .AnyAsync(a => a.EmployeeId == employeeId && a.Date.Date == dateOnly && a.AttendanceId != excludeId);
        }

        public async Task<IEnumerable<Attendance>> FilterAsync(int? departmentId, int? employeeId, DateTime? from, DateTime? to)
        {
            var query = _context.Attendances.Include(a => a.Employee).ThenInclude(e => e.Department).AsQueryable();

            if (departmentId.HasValue)
                query = query.Where(a => a.Employee.DepartmentId == departmentId.Value);

            if (employeeId.HasValue)
                query = query.Where(a => a.EmployeeId == employeeId.Value);

            if (from.HasValue)
                query = query.Where(a => a.Date >= from.Value);

            if (to.HasValue)
                query = query.Where(a => a.Date <= to.Value);

            return await query.ToListAsync();
        }
    }
}
