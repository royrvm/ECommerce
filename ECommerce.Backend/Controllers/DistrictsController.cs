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
    [Authorize(Roles ="Admin")]
    public class DistrictsController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: Districts
        public async Task<ActionResult> Index()
        {
            var districts = this.db.Districts.Include(d => d.Department);
            return View(await districts.ToListAsync());
        }

        // GET: Districts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            District district = await this.db.Districts.FindAsync(id);
            if (district == null)
            {
                return HttpNotFound();
            }
            return View(district);
        }

        // GET: Districts/Create
        public ActionResult Create()
        {
            ViewBag.DepartmentId = new SelectList(
                CombosHelper.GetDepartments(),
                "DepartmentId", 
                "Name");
            return View();
        }

        // POST: Districts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "DistrictId,Name,DepartmentId")] District district)
        {
            if (ModelState.IsValid)
            {
                this.db.Districts.Add(district);
                await this.db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentId = new SelectList(
                CombosHelper.GetDepartments(),
                "DepartmentId",
                "Name",
                district.DepartmentId);
            return View(district);
        }

        // GET: Districts/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            District district = await this.db.Districts.FindAsync(id);
            if (district == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentId = new SelectList(
                CombosHelper.GetDepartments(),
                "DepartmentId", 
                "Name",
                district.DepartmentId);
            return View(district);
        }

        // POST: Districts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "DistrictId,Name,DepartmentId")] District district)
        {
            if (ModelState.IsValid)
            {
                this.db.Entry(district).State = EntityState.Modified;
                await this.db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentId = new SelectList(
                CombosHelper.GetDepartments(),
                "DepartmentId",
                "Name", 
                district.DepartmentId);
            return View(district);
        }

        // GET: Districts/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            District district = await this.db.Districts.FindAsync(id);
            if (district == null)
            {
                return HttpNotFound();
            }
            return View(district);
        }

        // POST: Districts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            District district = await this.db.Districts.FindAsync(id);
            this.db.Districts.Remove(district);
            await this.db.SaveChangesAsync();
            return RedirectToAction("Index");

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
