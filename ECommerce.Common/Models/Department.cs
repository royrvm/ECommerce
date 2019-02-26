namespace ECommerce.Common.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage ="The field {0} is required")]
        [MaxLength(50,ErrorMessage ="The field {0} must be maximun {1} characters lenght")]
        [Display(Name ="Department")]
        public string Name { get; set; }

        public virtual ICollection<District> Districts { get; set; }
    }
}
