using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Models
{
    public class DataContext : DbContext
    {
        public DataContext() : base("DefaultConnection")
        {
            
        }

        public DbSet<Common.Models.Department> Departments { get; set; }

        public DbSet<Common.Models.District> Districts { get; set; }

        public DbSet<Common.Models.Company> Companies { get; set; }

        public DbSet<Common.Models.User> Users { get; set; }
    }
}
