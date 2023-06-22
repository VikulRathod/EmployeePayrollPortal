using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EmployeeManagement.Models;
using Microsoft.AspNet.Identity;

namespace EmployeeManagement.Controllers
{
    public class EmployeesController : Controller
    {
        private EmployeeManagementDBContext db = new EmployeeManagementDBContext();

        // GET: Employees
        [Authorize(Roles = "Super Admin,HR")]
        public ActionResult Index()
        {
            var employees = db.Employees.Include(e => e.AspNetUser);
            return View(employees.ToList());
        }        

        // GET: Employees/Details/5
        [Authorize(Roles = "Super Admin,HR,Employee")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        [Authorize(Roles = "Super Admin,HR,Employee")]
        public ActionResult Create()
        {
            string loggedInUserId = User.Identity.GetUserId();

            ViewBag.UserId = new SelectList
                (db.AspNetUsers.Where(u => u.Id.Equals(loggedInUserId)), "Id", "Email");

            Employee employee = db.Employees.FirstOrDefault(e => e.UserId.Equals(loggedInUserId));

            ViewBag.BloodGroup = new List<SelectListItem>() {
                new SelectListItem(){ Text = "Not Applicable", Value="Not Applicable"},
                new SelectListItem(){ Text = "A+", Value="A+"},
                new SelectListItem(){ Text = "A-", Value="A-"},
                new SelectListItem(){ Text = "B+", Value="B+"},
                new SelectListItem(){ Text = "B-", Value="B-"},
                new SelectListItem(){ Text = "AB+", Value="AB+"},
                new SelectListItem(){ Text = "AB-", Value="AB-"},
                new SelectListItem(){ Text = "O+", Value="O+"},
                new SelectListItem(){ Text = "O-", Value="O-"}
            };

            ViewBag.CompanyIdList = new SelectList
                (db.Companies, "CompanyId", "CompanyName");

            if (employee != null)
            {
                return View(employee);
            }

            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Authorize(Roles = "Super Admin,HR,Employee")]
        public ActionResult Create([Bind(Include = "Id,UserId,FullName,Photo, ImageFile, PermanentAddress,CurrentAddress,PAN,Adhaar,UAN,Mobile,AlternateMobile,EmailId,AlternateEmailId,BloodGroup,HighestQualification,HighestQualificationPassoutMonthYear,Technology,OfferDate,JoiningDate,OfferDesignation,CurrentDesignation,OfferSalary,CurrentSalary,LastHikeMonthYear,ResignationDate,RelivingDate,CompanyId,Gender,DateOfBirth")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                string fileName = Path.GetFileNameWithoutExtension(employee.ImageFile.FileName);
                string fileExtension = Path.GetExtension(employee.ImageFile.FileName);

                //fileName = fileName + DateTime.Now.ToString("yymmssfff") + fileExtension;                
                //fileName = Path.Combine(Server.MapPath("~/images/"), fileName);
                //employee.ImageFile.SaveAs(fileName);

                using (MemoryStream ms = new MemoryStream())
                {
                    employee.ImageFile.InputStream.CopyTo(ms);
                    employee.Photo = ms.GetBuffer();
                }

                db.Employees.Add(employee);
                db.SaveChanges();

                if (User.IsInRole("Super Admin,HR"))
                {
                    return RedirectToAction("Index");
                }

                return RedirectToAction("Index", "Home");
            }

            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", employee.UserId);

            ViewBag.CompanyIdList = new SelectList
                (db.Companies, "CompanyId", "CompanyName");

            return View(employee);
        }

        // GET: Employees/Edit/5
        [Authorize(Roles = "Super Admin,HR,Employee")]
        public ActionResult Edit(int? id)
        {
            string loggedInUserId = User.Identity.GetUserId();

            Employee employee = null;
            if (id == null)
            {
                employee = db.Employees.FirstOrDefault(e => e.UserId.Equals(loggedInUserId));
            }
            else
            {
                employee = db.Employees.Find(id);
            }

            if (employee == null)
            {
                return HttpNotFound();
            }

            ViewBag.UserId = new SelectList
                (db.AspNetUsers.Where(u => u.Id.Equals(loggedInUserId)), "Id", "Email");

            var bloodGroups = new List<SelectListItem>() {
                new SelectListItem(){ Text = "Not Applicable", Value="Not Applicable"},
                new SelectListItem(){ Text = "A+", Value="A+"},
                new SelectListItem(){ Text = "A-", Value="A-"},
                new SelectListItem(){ Text = "B+", Value="B+"},
                new SelectListItem(){ Text = "B-", Value="B-"},
                new SelectListItem(){ Text = "AB+", Value="AB+"},
                new SelectListItem(){ Text = "AB-", Value="AB-"},
                new SelectListItem(){ Text = "O+", Value="O+"},
                new SelectListItem(){ Text = "O-", Value="O-"}
            };

            ViewBag.BloodGroup = bloodGroups.Select(bg =>
            new SelectListItem()
            { Text = bg.Text, Value = bg.Value, Selected = (bg.Value.Equals(employee.BloodGroup)) }
            ).ToList();

            ViewBag.CompanyIdList = new SelectList
                (db.Companies, "CompanyId", "CompanyName", employee.CompanyId);

            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Super Admin,HR,Employee")]
        public ActionResult Edit([Bind(Include = "Id,UserId,FullName,Photo, ImageFile, PermanentAddress,CurrentAddress,PAN,Adhaar,UAN,Mobile,AlternateMobile,EmailId,AlternateEmailId,BloodGroup,HighestQualification,HighestQualificationPassoutMonthYear,Technology,OfferDate,JoiningDate,OfferDesignation,CurrentDesignation,OfferSalary,CurrentSalary,LastHikeMonthYear,ResignationDate,RelivingDate,CompanyId,Gender,DateOfBirth")] Employee employee)
        {
            if (employee.ImageFile != null && !string.IsNullOrEmpty(employee.ImageFile.FileName))
            {
                string fileName = Path.GetFileNameWithoutExtension(employee.ImageFile.FileName);
                string fileExtension = Path.GetExtension(employee.ImageFile.FileName);

                using (MemoryStream ms = new MemoryStream())
                {
                    employee.ImageFile.InputStream.CopyTo(ms);
                    employee.Photo = ms.GetBuffer();
                }
            }

            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();

                if (User.IsInRole("Super Admin,HR"))
                {
                    return RedirectToAction("Index");
                }

                return RedirectToAction("Index", "Home");
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", employee.UserId);

            ViewBag.CompanyIdList = new SelectList
                (db.Companies, "CompanyId", "CompanyName");

            var bloodGroups = new List<SelectListItem>() {
                new SelectListItem(){ Text = "Not Applicable", Value="Not Applicable"},
                new SelectListItem(){ Text = "A+", Value="A+"},
                new SelectListItem(){ Text = "A-", Value="A-"},
                new SelectListItem(){ Text = "B+", Value="B+"},
                new SelectListItem(){ Text = "B-", Value="B-"},
                new SelectListItem(){ Text = "AB+", Value="AB+"},
                new SelectListItem(){ Text = "AB-", Value="AB-"},
                new SelectListItem(){ Text = "O+", Value="O+"},
                new SelectListItem(){ Text = "O-", Value="O-"}
            };

            ViewBag.BloodGroup = bloodGroups.Select(bg =>
            new SelectListItem()
            { Text = bg.Text, Value = bg.Value, Selected = (bg.Value.Equals(employee.BloodGroup)) }
            ).ToList();

            return View(employee);
        }

        // GET: Employees/Delete/5
        [Authorize(Roles = "Super Admin,HR")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public PartialViewResult EmployeeIncrementView(string userId)
        {
            ViewBag.UserId = userId;
            var increments = db.EmployeeIncrements.Where(i => i.UserId.Equals(userId));
            return PartialView("_EmployeeIncrementPartial", increments);
        }

        public JsonResult InsertEmployeeIncrement(List<EmployeeIncrement> increments)
        {
            if (increments == null)
            {
                increments = new List<EmployeeIncrement>();
            }

            foreach (EmployeeIncrement employeeIncrement in increments)
            {
                employeeIncrement.ModifiedDate = DateTime.Now;
                db.EmployeeIncrements.Add(employeeIncrement);
            }
            int insertedRecords = db.SaveChanges();
            return Json(insertedRecords);
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
