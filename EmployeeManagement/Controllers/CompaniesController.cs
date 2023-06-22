using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EmployeeManagement.Models;
using Microsoft.AspNet.Identity;

namespace EmployeeManagement.Controllers
{
    public class CompaniesController : Controller
    {
        private EmployeeManagementDBContext db = new EmployeeManagementDBContext();

        // GET: Companies
        [Authorize(Roles = "Super Admin,HR")]
        public ActionResult Index()
        {
            string loggedInUserId = User.Identity.GetUserId();
            var companies = db.Companies.Include(c => c.AspNetUser);

            if (User.IsInRole("Super Admin"))
            {
                return View(companies.ToList());
            }

            return View(companies.ToList().Where(c => c.CreatedBy.Equals(loggedInUserId)));
        }

        // GET: Companies/Details/5
        [Authorize(Roles = "Super Admin,HR")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // GET: Companies/Create
        [Authorize(Roles = "Super Admin,HR")]
        public ActionResult Create()
        {
            string loggedInUserId = User.Identity.GetUserId();

            ViewBag.CreatedBy = new SelectList
                (db.AspNetUsers.Where(u => u.Id.Equals(loggedInUserId)), "Id", "Email");

            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Super Admin,HR")]
        public ActionResult Create([Bind(Include = "Id,CompanyName,CreatedBy,CreatedDate,ManagerId,ManagerName,ManagerMobile,ManagerOfficialEmail,HRId,HRName,HRMobile,HROfficialEmail")] Company company)
        {
            if (ModelState.IsValid)
            {
                company.CreatedDate = DateTime.Now;

                db.Companies.Add(company);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", company.CreatedBy);
            return View(company);
        }

        // GET: Companies/Edit/5
        [Authorize(Roles = "Super Admin,HR")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", company.CreatedBy);
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Super Admin,HR")]
        public ActionResult Edit([Bind(Include = "Id,CompanyName,CreatedBy,CreatedDate,ManagerId,ManagerName,ManagerMobile,ManagerOfficialEmail,HRId,HRName,HRMobile,HROfficialEmail")] Company company)
        {
            if (ModelState.IsValid)
            {
                db.Entry(company).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", company.CreatedBy);
            return View(company);
        }

        // GET: Companies/Delete/5
        [Authorize(Roles = "Super Admin,HR")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Super Admin,HR")]
        public ActionResult DeleteConfirmed(int id)
        {
            Company company = db.Companies.Find(id);
            db.Companies.Remove(company);
            db.SaveChanges();
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
