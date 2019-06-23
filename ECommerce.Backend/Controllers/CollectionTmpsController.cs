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
    public class CollectionTmpsController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: CollectionTmps
        public async Task<ActionResult> Index()
        {
            var collectionTmps = db.CollectionTmps.Include(c => c.Company).Include(c => c.DisbursedLoan).Include(c => c.LoanState).Include(c => c.Warehouse).Include(c => c.DisbursedLoan.Customer);
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
            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "Name");
            ViewBag.DisbursedLoanId = new SelectList(db.DisbursedLoans, "DisbursedLoanId", "UserName");
            ViewBag.LoanStateId = new SelectList(db.LoanStates, "LoanStateId", "Description");
            ViewBag.WarehouseId = new SelectList(db.Warehouses, "WarehouseId", "Name");
            return View();
        }

        // POST: CollectionTmps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CollectionTmp collectionTmp)
        {
            if (ModelState.IsValid)
            {
                db.CollectionTmps.Add(collectionTmp);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "Name", collectionTmp.CompanyId);
            ViewBag.DisbursedLoanId = new SelectList(db.DisbursedLoans, "DisbursedLoanId", "UserName", collectionTmp.DisbursedLoanId);
            ViewBag.LoanStateId = new SelectList(db.LoanStates, "LoanStateId", "Description", collectionTmp.LoanStateId);
            ViewBag.WarehouseId = new SelectList(db.Warehouses, "WarehouseId", "Name", collectionTmp.WarehouseId);
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

        private CollectionTmp ToCollectionTmp(CollectionTmp view)
        {
            return new CollectionTmp
            {
                CollectionId= view.CollectionId,
                CompanyId=view.CompanyId,
                WarehouseId=view.WarehouseId,
                DisbursedLoanId=view.DisbursedLoanId,
                UserName=view.UserName,
                LoanStateId=view.LoanStateId,
                CollectionDate=view.CollectionDate,
                Payment=view.Payment,
                CurrentBalance= view.CurrentBalance  - view.Payment,
            };
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
                UserName = collectionTmp.UserName,
                LoanStateId = collectionTmp.LoanStateId,
                CollectionDate = collectionTmp.CollectionDate,
                Payment = collectionTmp.Payment,
                CurrentBalance = disbusedLoan.Balance - collectionTmp.Payment,
            };


            if (ModelState.IsValid)
            {
                //var collection = this.ToCollectionTmp(collectionTmp);

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
