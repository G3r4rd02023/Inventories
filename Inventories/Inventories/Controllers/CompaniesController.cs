using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Inventories.Helpers;
using Inventories.Models;
using PagedList;

namespace Inventories.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CompaniesController : Controller
    {
        private InventoriesContext db = new InventoriesContext();

        // GET: Companies
        public ActionResult Index()
        {
            var companies = db.Companies.Include(c => c.City).Include(c => c.Department);
            return View(companies.ToList());
        }

        // GET: Companies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // GET: Companies/Create
        public ActionResult Create()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            ViewBag.CityID = new SelectList(CombosHelpers.GetCities(0), "CityID", "Name");
            ViewBag.DepartmentID = new SelectList(CombosHelpers.GetDepartments(), "DepartmentID", "Name");
            return View();
        }

        // POST: Companies/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Company company)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase FileBase = Request.Files[0];
                WebImage image = new WebImage(FileBase.InputStream);
                company.Logo = image.GetBytes();
                db.Companies.Add(company);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CityID = new SelectList(CombosHelpers.GetCities(company.DepartmentID), "CityID", "Name", company.CityID);
            ViewBag.DepartmentID = new SelectList(CombosHelpers.GetDepartments(), "DepartmentID", "Name", company.DepartmentID);
            return View(company);
        }

        // GET: Companies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityID = new SelectList(CombosHelpers.GetCities(company.DepartmentID), "CityID", "Name", company.CityID);
            ViewBag.DepartmentID = new SelectList(CombosHelpers.GetDepartments(), "DepartmentID", "Name", company.DepartmentID);
            return View(company);
        }

        // POST: Companies/Edit/5
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Company company)
        {
            if (ModelState.IsValid)
            {
                byte[] imagenActual = null;

                HttpPostedFileBase FileBase = Request.Files[0];
                if (FileBase == null)
                {
                    imagenActual = db.Companies.SingleOrDefault(t => t.CompanyID == company.CompanyID).Logo;
                }
                else
                {
                    WebImage image = new WebImage(FileBase.InputStream);
                    company.Logo = image.GetBytes();

                }
                db.Entry(company).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityID = new SelectList(CombosHelpers.GetCities(company.DepartmentID), "CityID", "Name", company.CityID);
            ViewBag.DepartmentID = new SelectList(CombosHelpers.GetDepartments(), "DepartmentID", "Name", company.DepartmentID);
            return View(company);
        }

        // GET: Companies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Company company = db.Companies.Find(id);
            db.Companies.Remove(company);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult GetImage(int id)
        {
            Company company = db.Companies.Find(id);
            byte[] byteImage = company.Logo;

            MemoryStream memoryStream = new MemoryStream(byteImage);
            Image image = Image.FromStream(memoryStream);

            memoryStream = new MemoryStream();
            image.Save(memoryStream, ImageFormat.Jpeg);
            memoryStream.Position = 0;

            return File(memoryStream, "image/jpg");
        }

        public JsonResult GetCities(int departmentId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var cities = db.Cities.Where(c => c.DepartmentID == departmentId);
            return Json(cities);
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
