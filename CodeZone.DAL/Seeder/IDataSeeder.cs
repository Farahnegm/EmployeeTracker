using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeZone.DAL.Seeder
{
    public interface IDataSeeder
    {
        Task Seed();
    }
}
