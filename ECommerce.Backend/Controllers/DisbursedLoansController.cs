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
    public class DisbursedLoansController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: DisbursedLoans
        public async Task<ActionResult> Index()
        {
            var disbursedLoans = db.DisbursedLoans.Include(d => d.Company).Include(d => d.Customer).Include(d => d.LoanState).Include(d => d.State).Include(d => d.TypeLoan).Include(d => d.Warehouse);
            return View(await disbursedLoans.ToListAsync());
        }

        // GET: DisbursedLoans/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DisbursedLoan disbursedLoan = await db.DisbursedLoans.FindAsync(id);
            if (disbursedLoan == null)
            {
                return HttpNotFound();
            }
            return View(disbursedLoan);
        }

        // GET: DisbursedLoans/Create
        public ActionResult Create()
        {
            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "Name");
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "UserName");
            ViewBag.LoanStateId = new SelectList(db.LoanStates, "LoanStateId", "Description");
            ViewBag.StateId = new SelectList(db.States, "StateId", "Description");
            ViewBag.TypeLoanId = new SelectList(db.TypeLoans, "TypeLoanId", "Description");
            ViewBag.WarehouseId = new SelectList(db.Warehouses, "WarehouseId", "Name");
            return View();
        }

        // POST: DisbursedLoans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "DisbursedLoanId,CompanyId,CustomerId,WarehouseId,StateId,TypeLoanId,LoanStateId,OrderId,StartDate,EndDate,Period,UserName,Remarks,BorrowedCapital,Interest,Total,Balance,DailyPayment,OperatingExpenses")] DisbursedLoan disbursedLoan)
        {
            if (ModelState.IsValid)
            {
                db.DisbursedLoans.Add(disbursedLoan);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "Name", disbursedLoan.CompanyId);
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "UserName", disbursedLoan.CustomerId);
            ViewBag.LoanStateId = new SelectList(db.LoanStates, "LoanStateId", "Description", disbursedLoan.LoanStateId);
            ViewBag.StateId = new SelectList(db.States, "StateId", "Description", disbursedLoan.StateId);
            ViewBag.TypeLoanId = new SelectList(db.TypeLoans, "TypeLoanId", "Description", disbursedLoan.TypeLoanId);
            ViewBag.WarehouseId = new SelectList(db.Warehouses, "WarehouseId", "Name", disbursedLoan.WarehouseId);
            return View(disbursedLoan);
        }

        // GET: DisbursedLoans/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DisbursedLoan disbursedLoan = await db.DisbursedLoans.FindAsync(id);
            if (disbursedLoan == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "Name", disbursedLoan.CompanyId);
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "UserName", disbursedLoan.CustomerId);
            ViewBag.LoanStateId = new SelectList(db.LoanStates, "LoanStateId", "Description", disbursedLoan.LoanStateId);
            ViewBag.StateId = new SelectList(db.States, "StateId", "Description", disbursedLoan.StateId);
            ViewBag.TypeLoanId = new SelectList(db.TypeLoans, "TypeLoanId", "Description", disbursedLoan.TypeLoanId);
            ViewBag.WarehouseId = new SelectList(db.Warehouses, "WarehouseId", "Name", disbursedLoan.WarehouseId);
            return View(disbursedLoan);
        }

        // POST: DisbursedLoans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "DisbursedLoanId,CompanyId,CustomerId,WarehouseId,StateId,TypeLoanId,LoanStateId,OrderId,StartDate,EndDate,Period,UserName,Remarks,BorrowedCapital,Interest,Total,Balance,DailyPayment,OperatingExpenses")] DisbursedLoan disbursedLoan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(disbursedLoan).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "CompanyId", "Name", disbursedLoan.CompanyId);
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "UserName", disbursedLoan.CustomerId);
            ViewBag.LoanStateId = new SelectList(db.LoanStates, "LoanStateId", "Description", disbursedLoan.LoanStateId);
            ViewBag.StateId = new SelectList(db.States, "StateId", "Description", disbursedLoan.StateId);
            ViewBag.TypeLoanId = new SelectList(db.TypeLoans, "TypeLoanId", "Description", disbursedLoan.TypeLoanId);
            ViewBag.WarehouseId = new SelectList(db.Warehouses, "WarehouseId", "Name", disbursedLoan.WarehouseId);
            return View(disbursedLoan);
        }

        // GET: DisbursedLoans/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DisbursedLoan disbursedLoan = await db.DisbursedLoans.FindAsync(id);
            if (disbursedLoan == null)
            {
                return HttpNotFound();
            }
            return View(disbursedLoan);
        }

        // POST: DisbursedLoans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            DisbursedLoan disbursedLoan = await db.DisbursedLoans.FindAsync(id);
            db.DisbursedLoans.Remove(disbursedLoan);
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
