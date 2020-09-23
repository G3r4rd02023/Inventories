using Inventories.Models;
using System;
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

        public static List<City> GetCities()
        {
            var cities = db.Cities.ToList();
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

        public void Dispose()
        {
            db.Dispose();
        }
    }
}