﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Common.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(256, ErrorMessage = "The field {0} must be maximun {1} characters lenght")]
        [Display(Name = "E-Mail")]
        //[Index("User_UserName_Index"),IsUnique=true]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(8, ErrorMessage = "The field {0} must be maximun {1} characters lenght")]
        [Display(Name = "DNI")]
        public string DNI { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(50, ErrorMessage = "The field {0} must be maximun {1} characters lenght")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(50, ErrorMessage = "The field {0} must be maximun {1} characters lenght")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(20, ErrorMessage = "The field {0} must be maximun {1} characters lenght")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(100, ErrorMessage = "The field {0} must be maximun {1} characters lenght")]
        public string Address { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Photo { get; set; }

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
        [Display(Name = "Company")]
        public int CompanyId { get; set; }
                
        [Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        [Display(Name = "Company")]
        public int MainWarehouseId { get; set; }

        [Display(Name = "User")]
        public string FullName { get {return string.Format("{0}{1}",FirstName,LastName); } }

        [Display(Name = "UserRoles")]
        public string AspRoles { get; set; } 

        public virtual Department Department { get; set; }

        public virtual District District { get; set; }

        public virtual Company Company { get; set; }

        public virtual MainWarehouse MainWarehouse { get; set; }

        public virtual ICollection<Warehouse> Warehouses { get; set; }
    }
}
