
using CodeZone.DAL.Data;
using CodeZone.DAL.Entities;
using CodeZone.DAL.Entities.Enum;
using CodeZone.DAL.Seeder;

namespace CodeZone_Task.Models
{
    public class DataSeeder(AppDbContext context) : IDataSeeder
    {
        public async Task Seed()
        {
            if (!context.Departments.Any())
            {
                // Seed Departments
                var dept1 = new Department { Name = "HRR", Code = "HRMG", Location = "Cairo" };
                var dept2 = new Department { Name = "Tech", Code = "TECH", Location = "Alexandria" };
                var dept3 = new Department { Name = "Finance", Code = "FINC", Location = "Giza" };
                context.Departments.AddRange(dept1, dept2, dept3);
                await context.SaveChangesAsync();

                // Seed Employees with unique codes
                int employeeCodeCounter = 1000;
                var emp1 = new Employee
                {
                    EmployeeCode = employeeCodeCounter++,
                    FullName = "John Adam Smith Lee",
                    Email = "john.smith@example.com",
                    DepartmentId = dept1.DepartmentId
                };
                var emp2 = new Employee
                {
                    EmployeeCode = employeeCodeCounter++,
                    FullName = "Sara Jane Mark Doe",
                    Email = "sara.doe@example.com",
                    DepartmentId = dept2.DepartmentId
                };
                var emp3 = new Employee
                {
                    EmployeeCode = employeeCodeCounter++,
                    FullName = "Ali Omar Fady Noor",
                    Email = "ali.noor@example.com",
                    DepartmentId = dept3.DepartmentId
                };
                context.Employees.AddRange(emp1, emp2, emp3);
                await context.SaveChangesAsync();

                // Seed Attendance
                var today = DateTime.Today;
                context.Attendances.AddRange(
                    new Attendance { EmployeeId = emp1.Id, Date = today, Status = AttendanceStatus.Present },
                    new Attendance { EmployeeId = emp2.Id, Date = today, Status = AttendanceStatus.Absent },
                    new Attendance { EmployeeId = emp3.Id, Date = today, Status = AttendanceStatus.Present }
                );
                await context.SaveChangesAsync();
            }
        }

        private static int GenerateUniqueEmployeeCode(AppDbContext context)
        {
            // Get the maximum employee code from the database
            var maxCode = context.Employees.Any() 
                ? context.Employees.Max(e => e.EmployeeCode) 
                : 999; // Start from 1000 if no employees exist
            
            return maxCode + 1;
        }
    }
}
