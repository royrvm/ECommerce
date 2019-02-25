namespace ECommerce.Backend.Models
{
    using Domain.Models;
    public class LocalDataContext:DataContext
    {
        public System.Data.Entity.DbSet<ECommerce.Common.Models.Department> Departments { get; set; }
    }
}