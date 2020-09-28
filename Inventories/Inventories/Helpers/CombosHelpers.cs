using Inventories.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventories.Helpers
{
    public class CombosHelpers : IDisposable
    {

        public static InventoriesContext db = new InventoriesContext();

        public static List<Department>GetDepartments()
        {
            var departments = db.Departments.ToList();
            departments.Add(new Department
            {
                DepartmentID = 0,
                Name = "[Selecciona un Departamento...]",
            });

            return departments.OrderBy(d => d.Name).ToList();
        }

        public static List<Product> GetProducts(int companyID)
        {
            var product = db.Products.Where(p => p.CompanyID == companyID).ToList();
            product.Add(new Product
            {
                ProductID = 0,
                Description = "[Selecciona un Producto...]",
            });

            return product.OrderBy(p => p.Description).ToList();
        }

        public static List<Product> GetProducts(int companyId, bool sw)
        {
           
            var products = db.Products.Where(p => p.CompanyID == companyId).ToList();
            return products.OrderBy(p => p.Description).ToList();
        }


        public static List<City> GetCities(int departmentId)
        {
            var cities = db.Cities.Where(c => c.DepartmentID == departmentId).ToList();
            cities.Add(new City
            {
                CityID = 0,
                Name = "[Selecciona un Ciudad...]",
            });

            return cities.OrderBy(d => d.Name).ToList();
        }

        public static List<Company> GetCompanies()
        {
            var companies = db.Companies.ToList();
            companies.Add(new Company
            {
                CompanyID = 0,
                Name = "[Selecciona un Compañia...]",
            });

            return companies.OrderBy(d => d.Name).ToList();
        }

        public static List<Tax> GetTaxes(int companyID)
        {
            var taxes = db.Taxes.Where(c => c.CompanyID == companyID).ToList();
            taxes.Add(new Tax
            {
                TaxID = 0,
                Description = "[Selecciona un Impuesto...]",
            });

            return taxes.OrderBy(d => d.Description).ToList();
        }

        public static List<Customer> GetCustomers(int companyID)
        {

            var qry = (from cu in db.Customers
                       join cc in db.CompanyCustomers on cu.CustomerId equals cc.CustomerID
                       join co in db.Companies on cc.CompanyID equals co.CompanyID
                       where co.CompanyID == companyID
                       select new { cu }).ToList();

            var customer = new List<Customer>();
            foreach (var item in qry)
            {
                customer.Add(item.cu);
            }
            customer.Add(new Customer
            {
                CustomerId = 0,
                FirstName = "[Selecciona un Cliente...]",
            });

            return customer.OrderBy(c => c.FirstName).ThenBy(c => c.LastName).ToList();
        }

        public static List<Category> GetCategories(int companyID)
        {
            var categoria = db.Categories.Where(c=>c.CompanyID == companyID).ToList();
            categoria.Add(new Category
            {
                CategoryID = 0,
                Descripcion = "[Selecciona una Categoria...]",
            });

            return categoria.OrderBy(d => d.Descripcion).ToList();
        }
        public void Dispose()
        {
            db.Dispose();
        }
    }
}