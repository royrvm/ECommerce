using ECommerce.Backend.Models;
using ECommerce.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECommerce.Backend.Classes
{
    public class CombosHelper : IDisposable
    {
        private static LocalDataContext db = new LocalDataContext();
        public static List<Department> GetDepartments()
        {
            var departments = db.Departments.ToList();
            departments.Add(new Department
            {
                DepartmentId = 0,
                Name = "[Select a department...]"
            });
            return departments.OrderBy(d => d.Name).ToList();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}