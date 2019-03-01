using ECommerce.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECommerce.Backend.Models
{
    public class CompanyView : Company
    {
        public HttpPostedFileBase LogoFile { get; set; }
    }
}