using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeZone.DAL.Entities
{
    public class Employee
    {
            public int Id { get; set; } 
            public int EmployeeCode { get; set; } 
            public string FullName { get; set; }
            public string Email { get; set; }
            public int DepartmentId { get; set; }

            public Department Department { get; set; }
        

    }

}
