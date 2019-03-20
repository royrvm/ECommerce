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
using ECommerce.Backend.Helpers;

namespace ECommerce.Backend.Controllers
{
    public class UsersController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: Users
        public async Task<ActionResult> Index()
        {
            var users = this.db.Users.Include(u => u.Company).Include(u => u.Department).Include(u => u.District);
            return View(await users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await this.db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.CompanyId = new SelectList(CombosHelper.GetCompanies(), "CompanyId", "Name");
            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name");
            ViewBag.DistrictId = new SelectList(CombosHelper.GetDistricts(), "DistrictId", "Name");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserView view)
        {
            if (ModelState.IsValid)
            {
                var pic = string.Empty;
                var folder = "~/Content/Companies";

                if (view.PhotoFile != null)
                {
                    pic = FilesHelper.UploadPhoto(view.PhotoFile, folder);
                    pic = string.Format("{0}/{1}", folder, pic);
                }

                var user = this.ToUser(view, pic);

                this.db.Users.Add(user);
                await this.db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CompanyId = new SelectList(CombosHelper.GetCompanies(), "CompanyId", "Name", view.CompanyId);
            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name", view.DepartmentId);
            ViewBag.DistrictId = new SelectList(CombosHelper.GetDistricts(), "DistrictId", "Name", view.DistrictId);
            return View(view);
        }

        private User ToUser(UserView view, string pic)
        {
            return new User
            {
                UserName=view.UserName,
                FirstName = view.FirstName,
                LastName = view.LastName,
                Phone = view.Phone,
                Address = view.Address,
                Photo = pic,
                DepartmentId = view.DepartmentId,
                DistrictId = view.DistrictId,
                CompanyId = view.CompanyId,
            };
        }

        // GET: Users/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await this.db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyId = new SelectList(CombosHelper.GetCompanies(), "CompanyId", "Name", user.CompanyId);
            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name", user.DepartmentId);
            ViewBag.DistrictId = new SelectList(CombosHelper.GetDistricts(), "DistrictId", "Name", user.DistrictId);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit( User user)
        {
            if (ModelState.IsValid)
            {
                this.db.Entry(user).State = EntityState.Modified;
                await this.db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CompanyId = new SelectList(CombosHelper.GetCompanies(), "CompanyId", "Name", user.CompanyId);
            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name", user.DepartmentId);
            ViewBag.DistrictId = new SelectList(CombosHelper.GetDistricts(), "DistrictId", "Name", user.DistrictId);
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await this.db.Users.FindAsync(id);
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
            User user = await this.db.Users.FindAsync(id);
            this.db.Users.Remove(user);
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
