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
    [Authorize(Roles = "Salesman")]

    public class CollectionTmpsController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: CollectionTmps
        public async Task<ActionResult> Index()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var wHouse = db.Warehouses.Where(wh => wh.UserId == user.UserId).FirstOrDefault();
            var collectionTmps = db.CollectionTmps.Where(c=>c.WarehouseId==wHouse.WarehouseId)
                .OrderBy(oName => oName.DisbursedLoan.Customer.FirstName).Include(c => c.Company)
                .Include(c => c.DisbursedLoan).Include(c => c.LoanState).Include(c => c.Warehouse).Include(c => c.DisbursedLoan.Customer);
            return View(await collectionTmps.ToListAsync());
        }

        // GET: CollectionTmps/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CollectionTmp collectionTmp = await db.CollectionTmps.FindAsync(id);
            if (collectionTmp == null)
            {
                return HttpNotFound();
            }
            return View(collectionTmp);
        }

        // GET: CollectionTmps/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CollectionTmps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CollectionTmp collectionTmp)
        {
            var response = MovementsHelper.NewCollection(collectionTmp, User.Identity.Name);
            var responseUpdate = MovementsHelper.UpdateInventories(User.Identity.Name);

            if (ModelState.IsValid)
            {
                //db.CollectionTmps.Add(collectionTmp);
                //await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(collectionTmp);
        }

        // GET: CollectionTmps/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CollectionTmp collectionTmp = await db.CollectionTmps.FindAsync(id);
            if (collectionTmp == null)
            {
                return HttpNotFound();
            }

            return PartialView(collectionTmp);
        }

        // POST: CollectionTmps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CollectionTmp collectionTmp)
        {

            var disbusedLoan = db.DisbursedLoans.Where(d => d.DisbursedLoanId == collectionTmp.DisbursedLoanId).FirstOrDefault();

            var view= new CollectionTmp
            {
                CollectionId = collectionTmp.CollectionId,
                CompanyId = collectionTmp.CompanyId,
                WarehouseId = collectionTmp.WarehouseId,
                DisbursedLoanId = collectionTmp.DisbursedLoanId,
                UserName = User.Identity.Name,
                LoanStateId = collectionTmp.LoanStateId,
                CollectionDate = collectionTmp.CollectionDate,
                Payment = collectionTmp.Payment,
                CurrentBalance = disbusedLoan.Balance - collectionTmp.Payment,
            };


            if (ModelState.IsValid)
            {

                db.Entry(view).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }


            return View(collectionTmp);
        }

        // GET: CollectionTmps/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CollectionTmp collectionTmp = await db.CollectionTmps.FindAsync(id);
            if (collectionTmp == null)
            {
                return HttpNotFound();
            }
            return View(collectionTmp);
        }

        // POST: CollectionTmps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CollectionTmp collectionTmp = await db.CollectionTmps.FindAsync(id);
            db.CollectionTmps.Remove(collectionTmp);
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
