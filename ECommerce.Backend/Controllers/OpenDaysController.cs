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
    [Authorize(Roles = "User")]

    public class OpenDaysController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: OpenDays
        public async Task<ActionResult> Index()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var openDays = db.OpenDays.Where(oD=>oD.CompanyId==user.CompanyId).Include(o => o.Company).OrderByDescending(oDat=>oDat.OpenDate);
            return View(await openDays.ToListAsync());

            //var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            //if (user == null)
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            //var users = this.db.Users.Where(c => c.CompanyId == user.CompanyId).Include(u => u.Department).Include(u => u.District);
            //return View(await users.ToListAsync());
        }

        // GET: OpenDays/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OpenDay openDay = await db.OpenDays.FindAsync(id);
            if (openDay == null)
            {
                return HttpNotFound();
            }
            return View(openDay);
        }

        // GET: OpenDays/Create
        public ActionResult Create()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            ViewBag.CompanyId = new SelectList(CombosHelper.GetCompanies(), "CompanyId", "Name",user.CompanyId);

            var view = new OpenDay
            {
                CompanyId = user.CompanyId,
                OpenDate = DateTime.Today,
                UserName=user.UserName,
                OnOff=true,
            };

            return View(view);
        }

        // POST: OpenDays/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OpenDay view)
        {
            var response = MovementsHelper.NewCollectionTmp(view, User.Identity.Name);

            if (ModelState.IsValid)
            {
                //db.OpenDays.Add(openDay);
                //await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "Name", view.CompanyId);
            return View(view);
        }

        // GET: OpenDays/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OpenDay openDay = await db.OpenDays.FindAsync(id);
            if (openDay == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "Name", openDay.CompanyId);
            return View(openDay);
        }

        // POST: OpenDays/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "OpenDayId,OpenDate,CompanyId,UserName")] OpenDay openDay)
        {
            if (ModelState.IsValid)
            {
                db.Entry(openDay).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "Name", openDay.CompanyId);
            return View(openDay);
        }

        // GET: OpenDays/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OpenDay openDay = await db.OpenDays.FindAsync(id);
            if (openDay == null)
            {
                return HttpNotFound();
            }
            return View(openDay);
        }

        // POST: OpenDays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            OpenDay openDay = await db.OpenDays.FindAsync(id);
            db.OpenDays.Remove(openDay);
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
