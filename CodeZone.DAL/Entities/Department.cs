using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeZone.DAL.Entities
{
    public class Department
    {
        public int DepartmentId { get; set; }

        
        public string Name { get; set; }


        public string Code { get; set; }

        
        public string Location { get; set; }

      
        public virtual ICollection<Employee> Employees { get; set; }
    }

}
