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
using ECommerce.Backend.Classes;

namespace ECommerce.Backend.Controllers
{
    [Authorize(Roles = "Admin")]
    public class Users1Controller : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: Users1
        public async Task<ActionResult> Index()
        {
            var users = db.Users.Include(u => u.Company).Include(u => u.Department).Include(u => u.District).Include(u => u.MainWarehouse);
            return View(await users.ToListAsync());
        }

        // GET: Users1/Details/5
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

        // GET: Users1/Create
        public ActionResult Create()
        {
            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "Name");
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name");
            ViewBag.DistrictId = new SelectList(db.Districts, "DistrictId", "Name");
            ViewBag.MainWarehouseId = new SelectList(db.MainWarehouses, "MainWarehouseId", "Name");
            return View();
        }

        // POST: Users1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "UserId,UserName,FirstName,LastName,Phone,Address,Photo,DepartmentId,DistrictId,CompanyId,MainWarehouseId")] User user)
        {
            var errors = ModelState.Where(x => x.Value.Errors.Count > 0)
             .Select(x => new { x.Key, x.Value.Errors })
             .ToArray();

            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                await db.SaveChangesAsync();
                UsersHelper.CreateUserASP(user.UserName, "Admin");
                return RedirectToAction("Index");
            }

            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "Name", user.CompanyId);
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", user.DepartmentId);
            ViewBag.DistrictId = new SelectList(db.Districts, "DistrictId", "Name", user.DistrictId);
            ViewBag.MainWarehouseId = new SelectList(db.MainWarehouses, "MainWarehouseId", "Name", user.MainWarehouseId);
            return View(user);
        }

        // GET: Users1/Edit/5
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
            ViewBag.MainWarehouseId = new SelectList(db.MainWarehouses, "MainWarehouseId", "Name", user.MainWarehouseId);
            return View(user);
        }

        // POST: Users1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "UserId,UserName,FirstName,LastName,Phone,Address,Photo,DepartmentId,DistrictId,CompanyId,MainWarehouseId")] User user)
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
            ViewBag.MainWarehouseId = new SelectList(db.MainWarehouses, "MainWarehouseId", "Name", user.MainWarehouseId);
            return View(user);
        }

        // GET: Users1/Delete/5
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

        // POST: Users1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            User user = await db.Users.FindAsync(id);
            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public JsonResult GetDistricts(int departmentId)
        {
            this.db.Configuration.ProxyCreationEnabled = false;
            var districts = this.db.Districts.Where(d => d.DepartmentId == departmentId);
            return Json(districts);
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
