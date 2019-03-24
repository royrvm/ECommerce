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
using ECommerce.Backend.Helpers;
using ECommerce.Backend.Classes;

namespace ECommerce.Backend.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CompaniesController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: Companies
        public async Task<ActionResult> Index()
        {
            var companies = this.db.Companies.Include(c => c.Department).Include(c => c.District);
            return View(await companies.ToListAsync());
        }

        // GET: Companies/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = await this.db.Companies.FindAsync(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // GET: Companies/Create
        public ActionResult Create()
        {
            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name");
            ViewBag.DistrictId = new SelectList(CombosHelper.GetDistricts(), "DistrictId", "Name");
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CompanyView view)
        {
            if (ModelState.IsValid)
            {
                var pic = string.Empty;
                var folder = "~/Content/Companies";

                if (view.LogoFile != null)
                {
                    pic = FilesHelper.UploadPhoto(view.LogoFile, folder);
                    pic = string.Format("{0}/{1}", folder, pic);
                }

                var company = this.ToCompany(view,pic);

                this.db.Companies.Add(company);
                await this.db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name", view.DepartmentId);
            ViewBag.DistrictId = new SelectList(CombosHelper.GetDistricts(), "DistrictId", "Name", view.DistrictId);
            return View(view);
        }

        private Company ToCompany(CompanyView view, string pic)
        {
            return new Company
            {
                Name=view.Name,
                Phone = view.Phone,
                Address = view.Address,
                Logo = pic,
                DepartmentId = view.DepartmentId,
                DistrictId = view.DistrictId,
            };
        }

        // GET: Companies/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var company = await this.db.Companies.FindAsync(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name", company.DepartmentId);
            ViewBag.DistrictId = new SelectList(CombosHelper.GetDistricts(), "DistrictId", "Name", company.DistrictId);
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit( Company company)
        {
            if (ModelState.IsValid)
            {
                this.db.Entry(company).State = EntityState.Modified;
                await this.db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name", company.DepartmentId);
            ViewBag.DistrictId = new SelectList(CombosHelper.GetDistricts(), "DistrictId", "Name", company.DistrictId);
            return View(company);
        }

        // GET: Companies/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = await this.db.Companies.FindAsync(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Company company = await this.db.Companies.FindAsync(id);
            this.db.Companies.Remove(company);
            await this.db.SaveChangesAsync();
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
                this.db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
