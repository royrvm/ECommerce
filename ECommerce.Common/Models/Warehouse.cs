using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Common.Models
{
    public class Warehouse
    {
        [Key]
        public int WarehouseId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        [Display(Name = "Company")]
        //[Index("Warehouse_CompanyId_Index"),1,IsUnique=true]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(50, ErrorMessage = "The field {0} must be maximun {1} characters lenght")]
        [Display(Name = "Warehouse")]
        //[Index("Warehouse_CompanyId_Index"),2,IsUnique=true]
        public string Name { get; set; }
                

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(20, ErrorMessage = "The field {0} must be maximun {1} characters lenght")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(100, ErrorMessage = "The field {0} must be maximun {1} characters lenght")]
        public string Address { get; set; }
        

        [Required(ErrorMessage = "The field {0} is required")]
        [Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        [Display(Name = "District")]
        public int DistrictId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        [Display(Name = "User")]
        public int UserId { get; set; }

        public virtual Department Department { get; set; }

        public virtual District District { get; set; }

        public virtual Company Company { get; set; }

        public virtual User User { get; set; }

    }
}
