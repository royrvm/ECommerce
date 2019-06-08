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
    [Authorize(Roles = "User")]
    public class UsersController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: Users
        public async Task<ActionResult> Index()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if(user==null)
            {
                return RedirectToAction("Index", "Home");
            }
            var users = this.db.Users.Where(c=>c.CompanyId==user.CompanyId).Include(u => u.Department).Include(u => u.District);
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
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var userlog = new UserView { CompanyId = user.CompanyId };

            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name");
            ViewBag.DistrictId = new SelectList(CombosHelper.GetDistricts(), "DistrictId", "Name");
            ViewBag.MainWarehouseId = new SelectList(db.MainWarehouses, "MainWarehouseId", "Name");
            ViewBag.userRoles = SelectUserRolers();
            return View(userlog);
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserView view)
        {
            //Sirve para poder ver errores en el ingreso de datos
            //var errors = ModelState.Where(x => x.Value.Errors.Count > 0)
            // .Select(x => new { x.Key, x.Value.Errors })
            // .ToArray();

            if (ModelState.IsValid)
            {
                var pic = string.Empty;
                var folder = "~/Content/Users";

                if (view.PhotoFile != null)
                {
                    pic = FilesHelper.UploadPhoto(view.PhotoFile, folder);
                    pic = string.Format("{0}/{1}", folder, pic);
                }

                var user = this.ToUser(view, pic);

                this.db.Users.Add(user);
                await this.db.SaveChangesAsync();
                UsersHelper.CreateUserASP(user.UserName, user.AspRoles);
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name", view.DepartmentId);
            ViewBag.DistrictId = new SelectList(CombosHelper.GetDistricts(), "DistrictId", "Name", view.DistrictId);
            ViewBag.MainWarehouseId = new SelectList(db.MainWarehouses, "MainWarehouseId", "Name");
            ViewBag.userRoles = SelectUserRolers();
            return View(view);
        }

        private User ToUser(UserView view, string pic)
        {            
            return new User
            {
                UserName=view.UserName,
                DNI = view.DNI,
                FirstName = view.FirstName,
                LastName = view.LastName,
                Phone = view.Phone,
                Address = view.Address,
                Photo = pic,
                DepartmentId = view.DepartmentId,
                DistrictId = view.DistrictId,
                CompanyId = view.CompanyId,
                MainWarehouseId = view.MainWarehouseId,
                AspRoles=view.AspRoles,
            };
        }

        private User ToUserEdit(UserView view, string pic)
        {
            return new User
            {
                UserId=view.UserId,
                UserName = view.UserName,
                DNI = view.DNI,
                FirstName = view.FirstName,
                LastName = view.LastName,
                Phone = view.Phone,
                Address = view.Address,
                Photo = pic,
                DepartmentId = view.DepartmentId,
                DistrictId = view.DistrictId,
                CompanyId = view.CompanyId,
                MainWarehouseId = view.MainWarehouseId,
                AspRoles = view.AspRoles,
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
            
            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name", user.DepartmentId);
            ViewBag.DistrictId = new SelectList(CombosHelper.GetDistricts(), "DistrictId", "Name", user.DistrictId);
            ViewBag.MainWarehouseId = new SelectList(db.MainWarehouses, "MainWarehouseId", "Name", user.MainWarehouseId);
            ViewBag.userRoles = SelectUserRolers();

            var view = this.ToView(user);

            return View(view);
        }

        private UserView ToView(User user)
        {
            return new UserView
            {
                UserId=user.UserId,
                UserName = user.UserName,
                DNI=user.DNI,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                Address = user.Address,
                Photo = user.Photo,
                DepartmentId = user.DepartmentId,
                DistrictId = user.DistrictId,
                CompanyId = user.CompanyId,
                MainWarehouseId = user.MainWarehouseId,
                AspRoles = user.AspRoles,
            };
        }

        public List<SelectListItem> SelectUserRolers ()
        {
            return new List<SelectListItem>() {
                new SelectListItem()
                {
                    Text="Admin",
                    Value="User",
                },
                new SelectListItem()
                {
                    Text="Supervisor",
                    Value="Supervisor",
                },
                new SelectListItem()
                {
                    Text="Salesman",
                    Value="Salesman",
                },
            };            
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserView view)
        {
            var userLast = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            //var role= db.Users.Where(r=>)
            if (ModelState.IsValid)
            {
                var pic = view.Photo;
                var folder = "~/Content/Users";

                if (view.PhotoFile != null)
                {
                    pic = FilesHelper.UploadPhoto(view.PhotoFile, folder);
                    pic = string.Format("{0}/{1}", folder, pic);
                }

                var db2 = new LocalDataContext();
                var currentUser = db2.Users.Find(view.UserId);
                if(currentUser.UserName!=view.UserName)
                {
                    UsersHelper.UpdateUserName(currentUser.UserName, view.UserName,view.AspRoles);
                }
                db2.Dispose();

                var user = this.ToUserEdit(view, pic);

                UsersHelper.UpdateUserRole(currentUser.UserName, userLast.AspRoles, user.AspRoles);


                this.db.Entry(user).State = EntityState.Modified;
                await this.db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name", view.DepartmentId);
            ViewBag.DistrictId = new SelectList(CombosHelper.GetDistricts(), "DistrictId", "Name", view.DistrictId);
            ViewBag.MainWarehouseId = new SelectList(db.MainWarehouses, "MainWarehouseId", "Name", view.MainWarehouseId);
            ViewBag.userRoles = SelectUserRolers();
            return View(view);
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
            var user = await this.db.Users.FindAsync(id);
            this.db.Users.Remove(user);
            await this.db.SaveChangesAsync();
            UsersHelper.DeleteUser(user.UserName);
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
