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

        public DbSet<Common.Models.Warehouse> Warehouses { get; set; }

        public DbSet<Common.Models.Customer> Customers { get; set; }

        public DbSet<Common.Models.Inventory> Inventories { get; set; }

        public DbSet<Common.Models.State> States { get; set; }

        public DbSet<Common.Models.Order> Orders { get; set; }

        public DbSet<Common.Models.MainWarehouse> MainWarehouses { get; set; }

        public DbSet<Common.Models.TypeLoan> TypeLoans { get; set; }

        public DbSet<Common.Models.LoanState> LoanStates { get; set; }

        public DbSet<Common.Models.DisbursedLoan> DisbursedLoans { get; set; }

        public DbSet<Common.Models.Collection> Collections { get; set; }

        public DbSet<Common.Models.CollectionTmp> CollectionTmps { get; set; }

        public DbSet<Common.Models.MainInventory> MainInventories { get; set; }
    }
}
