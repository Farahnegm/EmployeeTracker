using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeZone.DAL.Entities.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CodeZone.DAL.Entities
{
    public class Attendance
    {
        public int AttendanceId { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public AttendanceStatus Status { get; set; }
        public virtual Employee Employee { get; set; }
    }

}
