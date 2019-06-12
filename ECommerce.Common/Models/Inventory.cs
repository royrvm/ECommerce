using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Common.Models
{
    public class Inventory
    {
        [Key]
        public int InventoryId { get; set; }

        [Required]
        public int WarehouseId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage ="The field {0} is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}",ApplyFormatInEditMode =false)]
        public DateTime Date{ get; set; }

        [DisplayFormat(DataFormatString ="{0:C2}",ApplyFormatInEditMode =false)]
        public double Collection { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public double Pettycash{ get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public double Administrativeexpense{ get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public double Subtotal { get { return Collection + Pettycash+Administrativeexpense; } }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public double Renewedcredit { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public double Total { get{return Subtotal-Renewedcredit; } }

        public virtual Warehouse Warehouse { get; set; }

        public virtual Company Company { get; set; }

        public virtual ICollection<Collection> Collections { get; set; }

        public virtual ICollection<CollectionTmp> CollectionTmps { get; set; }
    }
}
