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
    public class LoanStatesController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: LoanStates
        public async Task<ActionResult> Index()
        {
            return View(await db.LoanStates.ToListAsync());
        }

        // GET: LoanStates/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoanState loanState = await db.LoanStates.FindAsync(id);
            if (loanState == null)
            {
                return HttpNotFound();
            }
            return View(loanState);
        }

        // GET: LoanStates/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LoanStates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "LoanStateId,Description")] LoanState loanState)
        {
            if (ModelState.IsValid)
            {
                db.LoanStates.Add(loanState);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(loanState);
        }

        // GET: LoanStates/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoanState loanState = await db.LoanStates.FindAsync(id);
            if (loanState == null)
            {
                return HttpNotFound();
            }
            return View(loanState);
        }

        // POST: LoanStates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "LoanStateId,Description")] LoanState loanState)
        {
            if (ModelState.IsValid)
            {
                db.Entry(loanState).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(loanState);
        }

        // GET: LoanStates/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoanState loanState = await db.LoanStates.FindAsync(id);
            if (loanState == null)
            {
                return HttpNotFound();
            }
            return View(loanState);
        }

        // POST: LoanStates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            LoanState loanState = await db.LoanStates.FindAsync(id);
            db.LoanStates.Remove(loanState);
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
