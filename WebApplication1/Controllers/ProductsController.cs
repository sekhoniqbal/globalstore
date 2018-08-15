using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{   
    public class ProductsController : Controller
    {
        private globalSpaceEntities db = new globalSpaceEntities();
        

        // GET: Products
        [Authorize(Roles ="seller")]
        public ActionResult Index()

        {
            int userid = db.Users.Where(u => u.email == User.Identity.Name).First().id;
            return View(db.Products.Where(u=>u.sellerid==userid).ToList());
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
            int userid = db.Users.Where(u => u.email == User.Identity.Name).First().id;
            if (product.sellerid != userid)
            {
                TempData["class"] = "text-danger";
                TempData["msg"] = "Product you are trying to View does not belong to you. Please signin with correct username";
                return RedirectToAction("index", "home");
            }
            
            return View(product);
        }

        // GET: Products/Create
        [Authorize(Roles ="seller")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,description,price,currencyid,sellerid,dateAdded,categoryid")] Product product, HttpPostedFileBase img)
        {
            product.dateAdded = DateTime.Now;
            int userid = db.Users.Where(u => u.email == User.Identity.Name).First().id;
            product.sellerid = userid;
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                string imgurl = null;
                db.SaveChanges();


                if (img != null) { 
                string imgext = Path.GetExtension(img.FileName);
                    string filepath = Server.MapPath("~/Content/images/") + product.id.ToString() + imgext;
                img.SaveAs(filepath);
                  imgurl = "/content/images/" + product.id.ToString() + imgext;
                }
                product.imgLocation = imgurl;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
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
            int userid = db.Users.Where(u => u.email == User.Identity.Name).First().id;
            if (product.sellerid != userid)
            {
                TempData["class"] = "text-danger";
                TempData["msg"] = "Product you are trying to edit does not belong to you. Please signin with correct username";
                return RedirectToAction("index", "home");
            }
            
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,description,price,currencyid,sellerid,imgLocation, dateAdded,categoryId")] Product product, HttpPostedFileBase img)
        {
            //var pro = db.Products.Find(product.id);
            //if (pro.seller != User.Identity.Name)
            //{
            //    TempData["class"] = "text-danger";
            //    TempData["msg"] = "Product you are trying to edit does not belong to you. Please signin with correct username";
            //    //        return RedirectToAction("index", "home");
            //}
            if (ModelState.IsValid)
            {
                int userid = db.Users.Where(u => u.email == User.Identity.Name).First().id;
                product.sellerid = userid;
                product.dateAdded = DateTime.Now;
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                if (img != null)
                {
                    string imgurl = null;
                    string imgext = Path.GetExtension(img.FileName);
                    string filepath = Server.MapPath("~/Content/images/") + product.id.ToString() + imgext;
                    img.SaveAs(filepath);
                    imgurl = "/content/images/" + product.id.ToString() + imgext;
                    product.imgLocation = imgurl;
                    db.SaveChanges();
                }
                

                return RedirectToAction("Index");
            }
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
            if (product.sellerid == db.Users.Where(u => u.email == User.Identity.Name).First().id) { return View(product); }
            else {
                TempData["class"] = "text-danger";
                TempData["msg"] = "Product you are trying to delete does not belong to you. Please signin with correct username";
                return RedirectToAction("index", "home"); }
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            if (product.sellerid == db.Users.Where(u => u.email == User.Identity.Name).First().id)
            {
                db.Products.Remove(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                TempData["class"] = "text-danger";
                TempData["msg"] = "Product you are trying to delete does not belong to you. Please signin with correct username";
                return RedirectToAction("index", "home");
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
