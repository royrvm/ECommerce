using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Common.Models
{
    public class CollectionTmp
    {
        [Key]
        public int CollectionId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        public int WarehouseId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        public int DisbursedLoanId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(256, ErrorMessage = "The filed {0} must be maximun {1} characters length")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        [Display(Name = "LoanState")]
        public int LoanStateId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CollectionDate { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Range(0, double.MaxValue, ErrorMessage = "You must enter values in {0} between {1} and {2}")]
        [Display(Name = "Payment")]
        public decimal Payment { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Range(0, double.MaxValue, ErrorMessage = "You must enter values in {0} between {1} and {2}")]
        [Display(Name = "Balance")]
        public decimal CurrentBalance { get; set; }

        public virtual Warehouse Warehouse { get; set; }

        public virtual LoanState LoanState { get; set; }

        public virtual Company Company { get; set; }

        public virtual DisbursedLoan DisbursedLoan { get; set; }

        public virtual Inventory Inventory { get; set; }
    }
}
