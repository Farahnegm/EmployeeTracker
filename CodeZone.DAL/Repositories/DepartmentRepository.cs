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
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly AppDbContext _context;

        public DepartmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Department>> GetAllWithEmployeeCountAsync()
        {
            return await _context.Departments
                .Include(d => d.Employees)
                .ToListAsync();
        }

        public async Task<Department?> GetByIdAsync(int id)
            => await _context.Departments.FindAsync(id);

        public async Task<Department?> GetByNameOrCodeAsync(string name, string code)
            => await _context.Departments
                .FirstOrDefaultAsync(d => d.Name == name || d.Code == code);

        public async Task AddAsync(Department department)
        {
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Department department)
        {
            var existingDepartment = await _context.Departments.FindAsync(department.DepartmentId);
            if (existingDepartment != null)
            {
                _context.Entry(existingDepartment).CurrentValues.SetValues(department);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Department department)
        {
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
            => await _context.Departments.AnyAsync(d => d.DepartmentId == id);
    }
}
