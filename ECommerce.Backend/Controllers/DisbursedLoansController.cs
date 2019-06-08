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
    public class DisbursedLoansController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: DisbursedLoans
        public async Task<ActionResult> Index()
        {
            var disbursedLoans = db.DisbursedLoans.Where(state=>state.StateId==2).Include(d => d.Company).Include(d => d.Customer).Include(d => d.State).Include(d => d.Warehouse);
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
        public ActionResult Create(int? id)
        {

            //var details = db.Orders.Where(odt => odt.UserName == userName).ToList;
            Order orders = db.Orders.Find(id);

            DisbursedLoan views = new DisbursedLoan()
            {
                //UserName = User.Identity.Name,
                CompanyId = orders.CompanyId,
                CustomerId = orders.CustomerId,
                WarehouseId = orders.WarehouseId,
                OrderId = orders.OrderId,
                StateId = DBHelper.GetState("Disbursed", db),
                TypeLoanId = DBHelper.GetTypeLoan("Renewed", db),
                LoanStateId = DBHelper.GetLoanState("Common",db),
                StartDate = orders.StartDate,
                EndDate = orders.EndDate,
                Period = orders.Period,
                UserName = User.Identity.Name,
                Remarks = orders.Remarks,
                BorrowedCapital = orders.BorrowedCapital,
                Interest = orders.Interest,
                Total = orders.Total,
                Balance = orders.Balance,
                DailyPayment = orders.DailyPayment,
                OperatingExpenses = orders.OperatingExpenses,
            };
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();


            ViewBag.CompanyId = new SelectList(CombosHelper.GetCompanies(), "CompanyId", "Name", orders.CompanyId);
            ViewBag.CustomerId = new SelectList(CombosHelper.GetCustomers(user.CompanyId), "CustomerId", "FullName", orders.CustomerId);
            //ViewBag.LoanStateId = new SelectList(db.LoanStates, "LoanStateId", "Description");
            //ViewBag.TypeLoanId = new SelectList(db.TypeLoans, "TypeLoanId", "Description");
            //ViewBag.WarehouseId = new SelectList(db.Warehouses, "WarehouseId", "Name", orders.WarehouseId);

            return View(views);
        }

        // POST: DisbursedLoans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(DisbursedLoan view)
        {
            if (ModelState.IsValid)
            {
                var response = MovementsHelper.NewLoan(view, User.Identity.Name);
                if (response.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, response.Message);
            }

            ViewBag.CompanyId = new SelectList(CombosHelper.GetCompanies(), "CompanyId", "Name", view.CompanyId);
            ViewBag.CustomerId = new SelectList(CombosHelper.GetCustomers(view.CompanyId), "CustomerId", "FullName", view.CustomerId);
            return View(view);
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
        public async Task<ActionResult> Edit(DisbursedLoan disbursedLoan)
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
