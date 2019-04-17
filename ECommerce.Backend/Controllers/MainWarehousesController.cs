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
    public class MainWarehousesController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: MainWarehouses
        public async Task<ActionResult> Index()
        {
            var mainWarehouses = db.MainWarehouses.Include(m => m.Company).Include(m => m.Department).Include(m => m.District);
            return View(await mainWarehouses.ToListAsync());
        }

        // GET: MainWarehouses/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MainWarehouse mainWarehouse = await db.MainWarehouses.FindAsync(id);
            if (mainWarehouse == null)
            {
                return HttpNotFound();
            }
            return View(mainWarehouse);
        }

        // GET: MainWarehouses/Create
        public ActionResult Create()
        {
            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "Name");
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name");
            ViewBag.DistrictId = new SelectList(db.Districts, "DistrictId", "Name");
            return View();
        }

        // POST: MainWarehouses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MainWarehouseId,CompanyId,WarehouseId,Name,Phone,Address,DepartmentId,DistrictId")] MainWarehouse mainWarehouse)
        {
            if (ModelState.IsValid)
            {
                db.MainWarehouses.Add(mainWarehouse);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "Name", mainWarehouse.CompanyId);
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", mainWarehouse.DepartmentId);
            ViewBag.DistrictId = new SelectList(db.Districts, "DistrictId", "Name", mainWarehouse.DistrictId);
            return View(mainWarehouse);
        }

        // GET: MainWarehouses/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MainWarehouse mainWarehouse = await db.MainWarehouses.FindAsync(id);
            if (mainWarehouse == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "Name", mainWarehouse.CompanyId);
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", mainWarehouse.DepartmentId);
            ViewBag.DistrictId = new SelectList(db.Districts, "DistrictId", "Name", mainWarehouse.DistrictId);
            return View(mainWarehouse);
        }

        // POST: MainWarehouses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MainWarehouseId,CompanyId,WarehouseId,Name,Phone,Address,DepartmentId,DistrictId")] MainWarehouse mainWarehouse)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mainWarehouse).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "Name", mainWarehouse.CompanyId);
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", mainWarehouse.DepartmentId);
            ViewBag.DistrictId = new SelectList(db.Districts, "DistrictId", "Name", mainWarehouse.DistrictId);
            return View(mainWarehouse);
        }

        // GET: MainWarehouses/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MainWarehouse mainWarehouse = await db.MainWarehouses.FindAsync(id);
            if (mainWarehouse == null)
            {
                return HttpNotFound();
            }
            return View(mainWarehouse);
        }

        // POST: MainWarehouses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            MainWarehouse mainWarehouse = await db.MainWarehouses.FindAsync(id);
            db.MainWarehouses.Remove(mainWarehouse);
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
