using ECommerce.Backend.Models;
using ECommerce.Common.Models;
using System;
using System.Collections;
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

        public static List<District> GetDistricts()
        {
            var districts = db.Districts.ToList();
            districts.Add(new District
            {
                DistrictId = 0,
                Name = "[Select a district...]"
            });
            return districts.OrderBy(d => d.Name).ToList();
        }
        
        public static List<Company> GetCompanies()
        {
            var companies = db.Companies.ToList();
            companies.Add(new Company
            {
                CompanyId = 0,
                Name = "[Select a company...]"
            });
            return companies.OrderBy(d => d.Name).ToList();
        }

        public static List<User> GetUsers(int companyId)
        {
            var user = db.Users.Where(u=>u.CompanyId==companyId).ToList();
            user.Add(new User
            {
                UserId = 0,
                UserName = "[Select a User...]"
            });
            return user.OrderBy(d => d.FirstName).ToList();
        }

        public static List<Customer> GetCustomers(int companyId)
        {
            var customers = db.Customers.Where(c => c.CompanyId == companyId).ToList();
            customers.Add(new Customer
            {
                CustomerId=0,
                FirstName="[Select a customer...]",
            });
            return customers.OrderBy(c => c.FirstName).ThenBy(c=>c.LastName).ToList();
        }

        public static List<MainWarehouse> GetMainWarehouses(int companyId)
        {
            var mainWarehouses = db.MainWarehouses.Where(c => c.CompanyId == companyId).ToList();

            var mainwarehouses = db.MainWarehouses.ToList();
            mainwarehouses.Add(new MainWarehouse
            {
                DepartmentId = 0,
                Name = "[Select a mainwarehouse...]"
            });
            return mainwarehouses.OrderBy(d => d.Name).ToList();
        }

        public static List<Warehouse> GetWarehouses(int companyId)
        {
            var wareHouses = db.Warehouses.Where(c => c.CompanyId == companyId).ToList();

            var warehouses = db.Warehouses.ToList();
            warehouses.Add(new Warehouse
            {
                WarehouseId = 0,
                Name = "[Select a warehouse...]"
            });
            return warehouses.OrderBy(d => d.Name).ToList();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}