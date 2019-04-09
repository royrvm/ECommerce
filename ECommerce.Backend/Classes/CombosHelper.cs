﻿using ECommerce.Backend.Models;
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

        public static List<User> GetUsers()
        {
            var user = db.Users.ToList();
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

        public void Dispose()
        {
            db.Dispose();
        }
    }
}