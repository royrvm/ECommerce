namespace ECommerce.Common.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        public string Name { get; set; }
    }
}
