using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeZone.BLL.DTOs
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public int EmployeeCode { get; set; }  
        public string FullName { get; set; }
        public string Email { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int Presents { get; set; }
        public int Absents { get; set; }
        public double AttendancePercentage { get; set; }
    }

}
