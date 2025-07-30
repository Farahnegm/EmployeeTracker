
using CodeZone.BLL.Interfaces;
using CodeZone.BLL.Services;
using CodeZone.BLL.Validation;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CodeZone.BLL.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddBllServices(this IServiceCollection services)
        {
           
            services.AddScoped<IDepartmentService,DepartmentService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IAttendanceService, AttendanceService>();

            // Register all your validators from the BLL layer
           services.AddValidatorsFromAssemblyContaining<EmployeeDtoValidator>();
           services.AddValidatorsFromAssemblyContaining<EmployeeValidator>();
            services.AddValidatorsFromAssemblyContaining<DepartmentValidator>();
           services.AddValidatorsFromAssemblyContaining<AttendanceValidator>();
           services.AddValidatorsFromAssemblyContaining<AttendanceDtoValidator>();



        }
    }
}
