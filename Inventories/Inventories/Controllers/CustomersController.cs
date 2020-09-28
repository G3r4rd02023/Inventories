using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Inventories.Helpers;
using Inventories.Models;

namespace Inventories.Controllers
{
    [Authorize(Roles ="User")]
    public class CustomersController : Controller
    {
        private InventoriesContext db = new InventoriesContext();

        // GET: Customers
        public ActionResult Index()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var qry = (from cu in db.Customers
                       join cc in db.CompanyCustomers on cu.CustomerId equals cc.CustomerID
                       join co in db.Companies on cc.CompanyID equals co.CompanyID
                       where co.CompanyID == user.CompanyID
                       select new { cu }).ToList();

            var customers = new List<Customer>();
            foreach (var item in qry)
            {
                customers.Add(item.cu);
            }

            return View(customers);

        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            ViewBag.CityId = new SelectList(CombosHelpers.GetCities(0), "CityID", "Name");
            ViewBag.DepartmentId = new SelectList(CombosHelpers.GetDepartments(), "DepartmentID", "Name");

            return View();
        }

        // POST: Customers/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Customer customer)
        {
            if (ModelState.IsValid)
            {
               using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Customers.Add(customer);
                        var response = DBHelper.SaveChanges(db);
                        if (!response.Succeeded)
                        {
                            ModelState.AddModelError(string.Empty, response.Message);
                            transaction.Rollback();
                            ViewBag.CityId = new SelectList(CombosHelpers.GetCities(customer.DepartmentId), "CityID", "Name", customer.CityId);
                            ViewBag.DepartmentId = new SelectList(CombosHelpers.GetDepartments(), "DepartmentID", "Name", customer.DepartmentId);
                            return View(customer);
                        }

                        UsersHelper.CreateUserASP(customer.UserName, "Customer");
                        var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
                        var companyCustomer = new CompanyCustomers
                        {
                            CompanyID = user.CompanyID,
                            CustomerID = customer.CustomerId,
                        };

                        db.CompanyCustomers.Add(companyCustomer);
                        db.SaveChanges();

                        transaction.Commit();

                        return RedirectToAction("Index");
                    }

                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                }
            }

            
            ViewBag.CityId = new SelectList(CombosHelpers.GetCities(customer.DepartmentId), "CityID", "Name", customer.CityId);
            ViewBag.DepartmentId = new SelectList(CombosHelpers.GetDepartments(), "DepartmentId", "Name", customer.DepartmentId);
            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityId = new SelectList(CombosHelpers.GetCities(customer.DepartmentId), "CityID", "Name", customer.CityId);
            ViewBag.DepartmentId = new SelectList(CombosHelpers.GetDepartments(), "DepartmentID", "Name", customer.DepartmentId);
            return View(customer);
        }

        // POST: Customers/Edit/5     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                var response = DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, response.Message);

            }
            ViewBag.CityId = new SelectList(CombosHelpers.GetCities(customer.DepartmentId), "CityID", "Name", customer.CityId);
            ViewBag.DepartmentId = new SelectList(CombosHelpers.GetDepartments(), "DepartmentID", "Name", customer.DepartmentId);
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var companyCustomer = db.CompanyCustomers.Where(cc => cc.CompanyID == user.CompanyID && cc.CustomerID == customer.CustomerId).FirstOrDefault();
           
            using(var transaction = db.Database.BeginTransaction())
            {
                db.CompanyCustomers.Remove(companyCustomer);
                db.Customers.Remove(customer);
                var response = DBHelper.SaveChanges(db);

                if (response.Succeeded)
                {
                    transaction.Commit();
                    return RedirectToAction("Index");
                }

                transaction.Rollback();
                ModelState.AddModelError(string.Empty, response.Message);
                return View(customer);
            }
                   
                  
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
