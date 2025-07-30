
using CodeZone.DAL.Data;
using CodeZone.DAL.Interface;
using CodeZone.DAL.Repositories;
using CodeZone.DAL.Seeder;
using CodeZone_Task.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CodeZone.DAL.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDalServices(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("AttendanceTracker"));

            services.AddScoped<IDataSeeder, DataSeeder>();
            services.AddScoped<IDepartmentService, DepartmentRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IAttendanceRepository, AttendanceRepository>();


        }
    }
}
