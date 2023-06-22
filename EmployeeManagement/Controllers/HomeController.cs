using EmployeeManagement.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        private EmployeeManagementDBContext db = new EmployeeManagementDBContext();

        public ActionResult Index()
        {
            string loggedInUserId = User.Identity.GetUserId();
            ViewBag.IsRegisteredEmployee = 
                db.Employees.FirstOrDefault(e => e.UserId.Equals(loggedInUserId));

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}