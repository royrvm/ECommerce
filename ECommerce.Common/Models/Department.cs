using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Common.Models
{
    public class Department
    {
        [Key]
        public int DepartamentId { get; set; }

        public string Name { get; set; }
    }
}
