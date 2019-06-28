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
    [Authorize(Roles = "Admin")]
    public class TypeLoansController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: TypeLoans
        public async Task<ActionResult> Index()
        {
            return View(await db.TypeLoans.ToListAsync());
        }

        // GET: TypeLoans/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeLoan typeLoan = await db.TypeLoans.FindAsync(id);
            if (typeLoan == null)
            {
                return HttpNotFound();
            }
            return View(typeLoan);
        }

        // GET: TypeLoans/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TypeLoans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "TypeLoanId,Description")] TypeLoan typeLoan)
        {
            if (ModelState.IsValid)
            {
                db.TypeLoans.Add(typeLoan);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(typeLoan);
        }

        // GET: TypeLoans/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeLoan typeLoan = await db.TypeLoans.FindAsync(id);
            if (typeLoan == null)
            {
                return HttpNotFound();
            }
            return View(typeLoan);
        }

        // POST: TypeLoans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "TypeLoanId,Description")] TypeLoan typeLoan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(typeLoan).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(typeLoan);
        }

        // GET: TypeLoans/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeLoan typeLoan = await db.TypeLoans.FindAsync(id);
            if (typeLoan == null)
            {
                return HttpNotFound();
            }
            return View(typeLoan);
        }

        // POST: TypeLoans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TypeLoan typeLoan = await db.TypeLoans.FindAsync(id);
            db.TypeLoans.Remove(typeLoan);
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
