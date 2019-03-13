﻿namespace ECommerce.Common.Models
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
        //[Index("Department_Name_Index"),IsUnique=true]
        public string Name { get; set; }

        public virtual ICollection<District> Districts { get; set; }

        public virtual ICollection<Company> Companies { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
