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


        public System.Data.Entity.DbSet<Common.Models.Department> Departments { get; set; }

        public System.Data.Entity.DbSet<ECommerce.Common.Models.District> Districts { get; set; }
    }
}