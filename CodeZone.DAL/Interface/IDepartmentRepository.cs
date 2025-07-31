using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeZone.DAL.Entities;

namespace CodeZone.DAL.Interface
{
    public interface IDepartmentRepository
    {
        Task<List<Department>> GetAllWithEmployeeCountAsync();
        Task<Department?> GetByIdAsync(int id);
        Task<Department?> GetByNameOrCodeAsync(string name, string code);
        Task AddAsync(Department department);
        Task UpdateAsync(Department department);
        Task DeleteAsync(Department department);
        Task<bool> ExistsAsync(int id);
    }

}
