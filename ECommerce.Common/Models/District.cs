using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Common.Models
{
    public class District
    {
        [Key]
        public int DistrictId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(50, ErrorMessage = "The field {0} must be maximun {1} characters lenght")]
        [Display(Name = "District")]
        //[Index("District_Name_Index"),IsUnique=true]
        public string Name { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Range(1,double.MaxValue,ErrorMessage ="You must select a {0}")]
        public int DepartmentId { get; set; }

        public virtual Department Department { get; set; }

        public virtual ICollection<Company> Companies { get; set; }
    }
}
