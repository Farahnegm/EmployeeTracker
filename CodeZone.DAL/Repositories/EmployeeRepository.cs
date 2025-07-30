using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeZone.DAL.Data;
using CodeZone.DAL.Entities.Enum;
using CodeZone.DAL.Entities;
using CodeZone.DAL.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodeZone.DAL.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
            => await _context.Employees.Include(e => e.Department).ToListAsync();

        public async Task<Employee?> GetByIdAsync(int id)
            => await _context.Employees.FindAsync(id);

        public async Task AddAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var employee = await GetByIdAsync(id);
            if (employee is not null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
        }

        public bool EmailExists(string email)
        {
            return _context.Employees.Any(e => e.Email == email);
        }



        public async Task<(int present, int absent)> GetAttendanceSummary(int employeeId)
        {
            var currentMonth = DateTime.Today.Month;
            var records = await _context.Attendances
                .Where(a => a.EmployeeId == employeeId && a.Date.Month == currentMonth)
                .ToListAsync();

            int present = records.Count(a => a.Status == AttendanceStatus.Present);
            int absent = records.Count(a => a.Status == AttendanceStatus.Absent);
            return (present, absent);
        }
    }

}
