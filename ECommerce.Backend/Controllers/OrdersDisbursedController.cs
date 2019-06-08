using ECommerce.Backend.Classes;
using ECommerce.Backend.Models;
using ECommerce.Common.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ECommerce.Backend.Controllers
{
    public class OrdersDisbursedController : Controller
    {
        // GET: OrdersDisbursed
        private LocalDataContext db = new LocalDataContext();
        public async Task<ActionResult> Index()
        {
            var disbursedLoans = db.Orders.Where(state => state.StateId == 1).Include(d => d.Company).Include(d => d.Customer).Include(d => d.State).Include(d => d.Warehouse);
            return View(await disbursedLoans.ToListAsync());
        }

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
                LoanStateId = DBHelper.GetLoanState("Common", db),
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

            return View(views);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DisbursedLoan view)
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
    }
}