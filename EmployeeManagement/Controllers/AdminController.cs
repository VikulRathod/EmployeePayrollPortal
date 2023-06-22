using EmployeeManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeManagement.Controllers
{
    [Authorize(Roles = "Super Admin")]
    public class AdminController : Controller
    {
        private EmployeeManagementDBContext db = new EmployeeManagementDBContext();

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchUser(string searchUser)
        {
            Session.Remove("VHaaShUserEmail");

            AspNetUser user = new AspNetUser();

            if (!string.IsNullOrEmpty(searchUser))
            {
                user = db.AspNetUsers.FirstOrDefault(
                 u =>
                 u.Email.Equals(searchUser, StringComparison.OrdinalIgnoreCase) ||
                 u.Email.ToLower().Contains(searchUser.ToLower())
                 );

                if (user != null && string.IsNullOrEmpty(user.Email))
                {
                    user.Email = (from u in db.AspNetUsers
                                  join e in db.Employees
                                  on u.Id equals e.UserId
                                  where e.Mobile.Equals(searchUser) || e.AlternateMobile.Equals(searchUser)
                                  select u.Email)?.SingleOrDefault();
                }
            }

            if (user != null && !string.IsNullOrEmpty(user.Email))
            {
                // Session["VHaaShUserEmail"] = user.Email;
                var userId = db.AspNetUsers.FirstOrDefault(u => u.Email.Equals(user.Email))?.Id;
                HttpCookie userInfo = new HttpCookie("userId", userId);
                Response.Cookies.Add(userInfo);
                return RedirectToAction("Home", "EmployeeV1");
            }

            return RedirectToAction("Index");
        }
    }
}