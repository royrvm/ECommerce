namespace ECommerce.Backend.Models
{
    using ECommerce.Domain.Models;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;

    public class LocalDataContext : DataContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public System.Data.Entity.DbSet<ECommerce.Common.Models.Warehouse> Warehouses { get; set; }

        public System.Data.Entity.DbSet<ECommerce.Common.Models.Customer> Customers { get; set; }
    }
}