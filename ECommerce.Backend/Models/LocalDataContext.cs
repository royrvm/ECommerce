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

        public System.Data.Entity.DbSet<ECommerce.Common.Models.MainWarehouse> MainWarehouses { get; set; }

        public System.Data.Entity.DbSet<ECommerce.Common.Models.TypeLoan> TypeLoans { get; set; }

        public System.Data.Entity.DbSet<ECommerce.Common.Models.LoanState> LoanStates { get; set; }

        public System.Data.Entity.DbSet<ECommerce.Common.Models.DisbursedLoan> DisbursedLoans { get; set; }
    }
}