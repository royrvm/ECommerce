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
    [Authorize(Roles ="User, Admin")]
    public class WarehousesController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: Warehouses
        public async Task<ActionResult> Index()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var warehouses = db.Warehouses.Where(w => w.CompanyId == user.CompanyId).Include(w => w.Department).Include(w => w.District).Include(w => w.User);
            return View(await warehouses.ToListAsync());
        }

        // GET: Warehouses/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var warehouse = await db.Warehouses.FindAsync(id);
            if (warehouse == null)
            {
                return HttpNotFound();
            }
            return View(warehouse);
        }

        // GET: Warehouses/Create
        public ActionResult Create()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var warehouse = new Warehouse { CompanyId = user.CompanyId, };

            //TODO still i don´t created the getmainwarehouses
            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name");
            ViewBag.DistrictId = new SelectList(CombosHelper.GetDistricts(), "DistrictId", "Name");
            ViewBag.UserId = new SelectList(CombosHelper.GetUsers(user.CompanyId), "UserId", "UserName");
            ViewBag.MainWarehouseId = new SelectList(db.MainWarehouses, "MainWarehouseId", "Name");

            

            //var mainwarehouse = new MainWarehouse { MainWarehouseId = user.MainWarehouseId, };
            return View(warehouse);
        }

        // POST: Warehouses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Warehouse warehouse)
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            if (ModelState.IsValid)
            {
                db.Warehouses.Add(warehouse);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name", warehouse.DepartmentId);
            ViewBag.DistrictId = new SelectList(CombosHelper.GetDistricts(), "DistrictId", "Name", warehouse.DistrictId);
            ViewBag.UserId = new SelectList(CombosHelper.GetUsers(user.CompanyId), "UserId", "UserName", warehouse.UserId);
            ViewBag.MainWarehouseId = new SelectList(db.MainWarehouses, "MainWarehouseId", "Name", warehouse.MainWarehouseId);
            return View(warehouse);
        }

        // GET: Warehouses/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var warehouse = await db.Warehouses.FindAsync(id);
            if (warehouse == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name", warehouse.DepartmentId);
            ViewBag.DistrictId = new SelectList(CombosHelper.GetDistricts(), "DistrictId", "Name", warehouse.DistrictId);
            ViewBag.UserId = new SelectList(CombosHelper.GetUsers(user.CompanyId), "UserId", "UserName", warehouse.UserId);
            ViewBag.MainWarehouseId = new SelectList(db.MainWarehouses, "MainWarehouseId", "Name", warehouse.MainWarehouseId);
            return View(warehouse);
        }

        // POST: Warehouses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Warehouse warehouse)
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();


            if (ModelState.IsValid)
            {
                db.Entry(warehouse).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name", warehouse.DepartmentId);
            ViewBag.DistrictId = new SelectList(CombosHelper.GetDistricts(), "DistrictId", "Name", warehouse.DistrictId);
            ViewBag.UserId = new SelectList(CombosHelper.GetUsers(user.CompanyId), "UserId", "UserName", warehouse.UserId);
            ViewBag.MainWarehouseId = new SelectList(db.MainWarehouses, "MainWarehouseId", "Name",warehouse.MainWarehouseId);
            return View(warehouse);
        }

        // GET: Warehouses/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var warehouse = await db.Warehouses.FindAsync(id);
            if (warehouse == null)
            {
                return HttpNotFound();
            }
            return View(warehouse);
        }

        // POST: Warehouses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Warehouse warehouse = await db.Warehouses.FindAsync(id);
            db.Warehouses.Remove(warehouse);
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
