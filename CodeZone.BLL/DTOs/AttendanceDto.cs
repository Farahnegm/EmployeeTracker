using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeZone.DAL.Entities.Enum;

namespace CodeZone.BLL.DTOs
{
    public class AttendanceDto
    {
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public AttendanceStatus Status { get; set; }
    }

}
