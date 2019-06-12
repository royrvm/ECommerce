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
    public class OpenDayController : Controller
    {
        private LocalDataContext db = new LocalDataContext();
        // GET: OpenDay
        public async Task<ActionResult> Index()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var users = this.db.Users.Where(c => c.CompanyId == user.CompanyId).Include(u => u.Department).Include(u => u.District);
            return View(await users.ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DisbursedLoan view)
        {
            if (ModelState.IsValid)
            {
                var response = MovementsHelper.NewCollection(view, User.Identity.Name);
                if (response.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, response.Message);
            }

            return View(view);
        }
    }
}