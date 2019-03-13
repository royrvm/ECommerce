using ECommerce.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECommerce.Backend.Models
{
    public class UserView : User
    {
        public HttpPostedFileBase PhotoFile { get; set; }
    }
}