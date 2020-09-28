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

namespace Inventories.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private InventoriesContext db = new InventoriesContext();

        // GET: Users
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.City).Include(u => u.Company).Include(u => u.Department);
            return View(users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.CityID = new SelectList(CombosHelpers.GetCities(0), "CityID", "Name");
            ViewBag.CompanyID = new SelectList(CombosHelpers.GetCompanies(), "CompanyID", "Name");
            ViewBag.DepartmentID = new SelectList(CombosHelpers.GetDepartments(), "DepartmentID", "Name");
            return View();
        }

        // POST: Users/Create
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( User user)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase FileBase = Request.Files[0];
                WebImage image = new WebImage(FileBase.InputStream);
                user.Foto = image.GetBytes();
                db.Users.Add(user);
                db.SaveChanges();
                UsersHelper.CreateUserASP(user.UserName, "User");
                return RedirectToAction("Index");
            }

            ViewBag.CityID = new SelectList(CombosHelpers.GetCities(user.DepartmentID), "CityID", "Name", user.CityID);
            ViewBag.CompanyID = new SelectList(CombosHelpers.GetCompanies(), "CompanyID", "Name", user.CompanyID);
            ViewBag.DepartmentID = new SelectList(CombosHelpers.GetDepartments(), "DepartmentID", "Name", user.DepartmentID);
            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityID = new SelectList(CombosHelpers.GetCities(user.DepartmentID), "CityID", "Name", user.CityID);
            ViewBag.CompanyID = new SelectList(CombosHelpers.GetCompanies(), "CompanyID", "Name", user.CompanyID);
            ViewBag.DepartmentID = new SelectList(CombosHelpers.GetDepartments(), "DepartmentID", "Name", user.DepartmentID);
            return View(user);
        }

        // POST: Users/Edit/5
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                byte[] imagenActual = null;

                HttpPostedFileBase FileBase = Request.Files[0];
                if (FileBase == null)
                {
                    imagenActual = db.Users.SingleOrDefault(t => t.UserID == user.UserID).Foto;
                }
                else
                {
                    WebImage image = new WebImage(FileBase.InputStream);
                    user.Foto = image.GetBytes();

                }
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityID = new SelectList(CombosHelpers.GetCities(user.DepartmentID), "CityID", "Name", user.CityID);
            ViewBag.CompanyID = new SelectList(CombosHelpers.GetCompanies(), "CompanyID", "Name", user.CompanyID);
            ViewBag.DepartmentID = new SelectList(CombosHelpers.GetDepartments(), "DepartmentID", "Name", user.DepartmentID);
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult GetImage(int id)
        {
            User user = db.Users.Find(id);
            byte[] byteImage = user.Foto;

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
