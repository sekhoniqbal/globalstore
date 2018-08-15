using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "admin")]
    public class UsersController : Controller
    {
        private globalSpaceEntities db = new globalSpaceEntities();

        // GET: Users
        
        public ActionResult Index()
        {
            return View(db.Users.ToList());
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
        [AllowAnonymous]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost][ValidateAntiForgeryToken][AllowAnonymous]
        public ActionResult Create([Bind(Include = "id,firstName,lastName,email,password,seller,buyer,phone")] User user)
        {
            var count = db.Users.Where(u => u.email == user.email).Count();
            if (count>0) { TempData["msg"] = "your account already exists, please login below";
                TempData["class"] = "text-success";
                return RedirectToAction("login"); }
            else if (ModelState.IsValid)
            {
                db.Users.Add(user);
                try { db.SaveChanges(); }
                catch { return View(user); }
                
                return RedirectToAction("Index");
            }

            return View("create");
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
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,firstName,lastName,email,password,seller,buyer,phone")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [AllowAnonymous]
        public ActionResult login() {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult login(string email, string password)
        {
            var count = db.Users.Where(u => u.email == email && u.password == password).Count();
            if (count == 1)
            {
                TempData["msg"] = "login successfull";
                FormsAuthentication.SetAuthCookie(email, false);
                return RedirectToAction("index", "home");
            }
            else
            {
                TempData["msg"] = "email and password combination did not match, please try again";
                return View();
            }
        }
        [AllowAnonymous]
        public ActionResult logout() {
            FormsAuthentication.SignOut();
            return RedirectToAction("index", "home");

        }

    }
}
