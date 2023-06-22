using EmployeeManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeManagement.Controllers
{
    public class CompanyHRController : Controller
    {
        EmployeeManagementDBContext _db = new EmployeeManagementDBContext();

        // GET: CompanyHR
        public ActionResult Index()
        {
            string userId = GetUserId();
            int hrCompanyId = _db.Employees.FirstOrDefault(e => e.UserId == userId).CompanyId ?? 0;

            var employeesUnderCompany = _db.Employees
                .Where(e => e.CompanyId == hrCompanyId).ToList();

            var onlyEmployees = (from e in employeesUnderCompany
                                 join u in _db.AspNetUsers
                                 on e.UserId equals u.Id
                                 where u.AspNetRoles.All(r => r.Name == "Employee")
                                 select e).ToList();



            return View(onlyEmployees);
        }

        public ActionResult ResignedCandidates()
        {
            string userId = GetUserId();
            string hrCompanyName = _db.Employees.
                FirstOrDefault(e => e.UserId == userId).Company.CompanyName ?? "NA";

            var employeesUnderCompany = _db.EmployeeBgvViews
                .Where(e => e.Company_Name == hrCompanyName 
                && e.Last_Working_Day != null).ToList();

            return View(employeesUnderCompany);
        }

        private string GetUserId()
        {
            //string userEmail = Session["VHaaShUserEmail"]?.ToString();
            //return
            //    _db.AspNetUsers.FirstOrDefault
            //    (u => u.Email.Equals(userEmail))?.Id;
            return Request.Cookies["userId"]?.Value;
        }

        //[HttpGet]
        //public ActionResult Details(string Id)
        //{
        //    var employee = _db.Employees.Find(int.Parse(Id));

        //    return View(employee);
        //}

        [HttpGet]
        public ActionResult BgvDetails(string Id)
        {
            var employee = _db.EmployeeBgvViews.Find(Id);

            return View(employee);
        }

        [HttpGet]
        public ActionResult CandidateDetails(string userId)
        {
            var employee = _db.Employees.FirstOrDefault(e =>
            e.UserId == userId);

            return View(employee);
        }

        [HttpPost]
        public ActionResult CandidateDetails(Employee employee)
        {
            try
            {
                var empFromDB = _db.Employees.Find(employee.Id);
                empFromDB.OfficialEmployeeId = employee.OfficialEmployeeId;
                empFromDB.OfficialEmailId = employee.OfficialEmailId;
                empFromDB.OfficialEmailIdPassword = employee.OfficialEmailIdPassword;

                _db.Entry<Employee>(empFromDB).State = EntityState.Modified;
                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(employee);
            }
        }
    }
}