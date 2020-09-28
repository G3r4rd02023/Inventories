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
    [Authorize(Roles = "User")]
    public class ProductsController : Controller
    {
        private InventoriesContext db = new InventoriesContext();

        // GET: Products
        public ActionResult Index()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var products = db.Products
                .Include(p => p.Category)              
                .Include(p => p.Tax)
                .Where(p => p.CompanyID == user.CompanyID);

            return View(products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.CategoryID = new SelectList(CombosHelpers.GetCategories(user.CompanyID), "CategoryID", "Descripcion");          
            ViewBag.TaxID = new SelectList(CombosHelpers.GetTaxes(user.CompanyID), "TaxID", "Description");
            var product = new Product { CompanyID = user.CompanyID, };
            return View(product);
        }

        // POST: Products/Create
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Product product)
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            if (ModelState.IsValid)
            {
                HttpPostedFileBase FileBase = Request.Files[0];
                WebImage image = new WebImage(FileBase.InputStream);
                product.Image = image.GetBytes();
                db.Products.Add(product);
                var response = DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, response.Message);
            }
            ViewBag.CategoryID = new SelectList(CombosHelpers.GetCategories(user.CompanyID), "CategoryID", "Descripcion", product.CategoryID);           
            ViewBag.TaxID = new SelectList(CombosHelpers.GetTaxes(user.CompanyID), "TaxID", "Description", product.TaxID);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product product = db.Products.Find(id);
            
            if (product == null)
            {
                return HttpNotFound();
            }

            ViewBag.CategoryID = new SelectList(CombosHelpers.GetCategories(product.CompanyID), "CategoryID", "Descripcion", product.CategoryID);
            ViewBag.TaxID = new SelectList(CombosHelpers.GetTaxes(product.CompanyID), "TaxID", "Description", product.TaxID);
            return View(product);
        }

        // POST: Products/Edit/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Product product)
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            byte[] imagenActual = null;

            HttpPostedFileBase FileBase = Request.Files[0];
            if (FileBase == null)
            {
                imagenActual = db.Products.SingleOrDefault(t => t.ProductID == product.ProductID).Image;
            }
            else
            {
                WebImage image = new WebImage(FileBase.InputStream);
                product.Image = image.GetBytes();

            }
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                var response = DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, response.Message);

            }
            ViewBag.CategoryID = new SelectList(CombosHelpers.GetCategories(product.CompanyID), "CategoryID", "Descripcion", product.CategoryID);
            ViewBag.TaxID = new SelectList(CombosHelpers.GetTaxes(product.CompanyID), "TaxID", "Description", product.TaxID);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            var response = DBHelper.SaveChanges(db);
            if (response.Succeeded)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, response.Message);
            return View(product);
        }

        public ActionResult GetImage(int id)
        {
            Product product = db.Products.Find(id);
            byte[] byteImage = product.Image;

            MemoryStream memoryStream = new MemoryStream(byteImage);
            Image image = Image.FromStream(memoryStream);

            memoryStream = new MemoryStream();
            image.Save(memoryStream, ImageFormat.Jpeg);
            memoryStream.Position = 0;

            return File(memoryStream, "image/jpg");
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
