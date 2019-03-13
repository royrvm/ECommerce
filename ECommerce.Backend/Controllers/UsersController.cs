using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ECommerce.Backend.Models;
using ECommerce.Common.Models;

namespace ECommerce.Backend.Controllers
{
    public class UsersController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: Users
        public async Task<ActionResult> Index()
        {
            var users = db.Users.Include(u => u.Company).Include(u => u.Department).Include(u => u.District);
            return View(await users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "Name");
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name");
            ViewBag.DistrictId = new SelectList(db.Districts, "DistrictId", "Name");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "UserId,UserName,FirstName,LastName,Phone,Address,Photo,DepartmentId,DistrictId,CompanyId")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "Name", user.CompanyId);
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", user.DepartmentId);
            ViewBag.DistrictId = new SelectList(db.Districts, "DistrictId", "Name", user.DistrictId);
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "Name", user.CompanyId);
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", user.DepartmentId);
            ViewBag.DistrictId = new SelectList(db.Districts, "DistrictId", "Name", user.DistrictId);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "UserId,UserName,FirstName,LastName,Phone,Address,Photo,DepartmentId,DistrictId,CompanyId")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "Name", user.CompanyId);
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", user.DepartmentId);
            ViewBag.DistrictId = new SelectList(db.Districts, "DistrictId", "Name", user.DistrictId);
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            User user = await db.Users.FindAsync(id);
            db.Users.Remove(user);
            await db.SaveChangesAsync();
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
    }
}
