using EmployeeManagement.Models;
using EmployeeManagement.Notifications;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeManagement.Controllers
{
    public class EmployeeV1Controller : Controller
    {
        private EmployeeManagementDBContext db = new EmployeeManagementDBContext();

        [HttpGet]
        public ActionResult RedirectOnLogin()
        {
            if (User.IsInRole("Super Admin"))
            {
                return RedirectToAction("Index", "UsersAdmin");
            }
            else if (User.IsInRole("HR"))
            {
                return RedirectToAction("Index", "CompanyHR");
            }
            else
            {
                return RedirectToAction("Home");
            }
        }

        [HttpGet]
        public ActionResult Home()
        {
            string userId = GetUserId();
            string name = db.Employees.FirstOrDefault(e => e.UserId == userId)?.FullName;
            ViewBag.Name = !string.IsNullOrEmpty(name)? name: "Not Exists";
            //string userId = GetUserId();

            //var employees = db.Employees.Where(e => e.UserId.Equals(userId))?.ToList();
            //var employeeEducation = db.EmployeeEducations.Where(e => e.UserId.Equals(userId))?.ToList();
            //var employeeBankAccount = db.EmployeeBankAccounts.Where(e => e.UserId.Equals(userId))?.ToList();
            //var employeeExperience = db.EmployeeExperieces.Where(e => e.UserId.Equals(userId))?.ToList();
            //var employeeITExperience = db.EmployeeITExperiences.Where(e => e.UserId.Equals(userId))?.ToList();
            //var employeeIncrements = db.EmployeeIncrements.Where(e => e.UserId.Equals(userId))?.ToList();
            //var employeeInterviews = db.EmployeeInterviews.Where(e => e.UserId.Equals(userId))?.ToList();
            //var employeeResignation = db.EmployeeResignations.Where(e => e.UserId.Equals(userId))?.ToList();

            return View();
        }

        private string GetUserId()
        {
            try
            {
                //string userEmail = Session["VHaaShUserEmail"].ToString();
                //return
                //    db.AspNetUsers.FirstOrDefault
                //    (u => u.Email.Equals(userEmail))?.Id;
                return Request.Cookies["userId"]?.Value;
            }
            catch (Exception ex)
            {
                string exceptionMessage = $"Error in method: *GetUserId()* Message: *{ex.Message}*";
                SendException(exceptionMessage);
                return null;
            }
        }

        [HttpGet]
        [ChildActionOnly]
        public PartialViewResult PersonalDetailsEdit()
        {
            string userId = GetUserId();
            var employee = db.Employees.FirstOrDefault(e => e.UserId.Equals(userId));

            ViewBag.BloodGroup = GetBloodGroups(employee?.BloodGroup);
            if (employee != null)
                employee.DateOfBirth = employee?.DateOfBirth;
            return PartialView("_PersonalDetailsEdit", employee);
        }

        //[HttpGet]
        //public ActionResult PersonalDetails()
        //{
        //    string userId = GetUserId();

        //    ViewBag.BloodGroup = GetBloodGroups(null);

        //    return View();
        //}

        [HttpPost]
        public ActionResult PersonalDetails
            ([Bind(Include = "Id,UserId,FullName,Photo, ImageFile, PermanentAddress,CurrentAddress,PAN,Adhaar,UAN,Mobile,AlternateMobile,EmailId,AlternateEmailId,BloodGroup,Gender,DateOfBirth")] Employee employee)
        {
            string userId = GetUserId();

            //if (ModelState.IsValid)
            //{
            try
            {
                if (employee.ImageFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(employee.ImageFile?.FileName);
                    string fileExtension = Path.GetExtension(employee.ImageFile?.FileName);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        employee.ImageFile?.InputStream.CopyTo(ms);
                        employee.Photo = ms.GetBuffer();
                    }
                }

                var employeeDb = db.Employees.Find(employee?.Id);

                if (employeeDb != null)
                {
                    //db.Employees.Attach(employee);
                    //db.Entry(employee).State = EntityState.Modified;
                    employee.CompanyId = employeeDb.CompanyId;
                    employee.OfficialEmployeeId = employeeDb.OfficialEmployeeId;
                    employee.OfficialEmailId = employeeDb.OfficialEmailId;
                    employee.OfficialEmailIdPassword = employeeDb.OfficialEmailIdPassword;
                    db.Entry(employeeDb).CurrentValues.SetValues(employee);
                }
                else
                {
                    employee.UserId = userId;
                    employee.AddedDate = DateTime.Now;
                    db.Employees.Add(employee);
                }
                db.SaveChanges();

                if (User.IsInRole("Super Admin,HR"))
                {
                    return RedirectToAction("Home");
                }

                return RedirectToAction("Home", "EmployeeV1");
                //}
            }
            catch (Exception ex)
            {
                ViewBag.BloodGroup = GetBloodGroups(employee.BloodGroup);
                // string userEmail = Session["VHaaShUserEmail"]?.ToString();
                string uId = GetUserId();
                string name = db.Employees.FirstOrDefault(e => e.UserId == uId)?.FullName;
                string exceptionMessage = $"Error occured for user *{name}*: method: *PersonalDetails()* Message: *{ex.Message}*";
                SendException(exceptionMessage);

                return RedirectToAction("Home", "EmployeeV1");
            }
        }

        List<SelectListItem> GetBloodGroups(string bloodGroup)
        {
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

            if (!string.IsNullOrEmpty(bloodGroup))
            {
                bloodGroups.ForEach(b =>
                {
                    if (b.Text.Equals(bloodGroup))
                        b.Selected = true;
                });
            }

            return bloodGroups;
        }

        public PartialViewResult EducationalNewForm()
        {
            ViewBag.EducationTypeIdList = new SelectList
                (db.EmployeeEducationTypes, "EducationTypeId", "EducationName");

            return PartialView("_newEducationalDetailsForm");
        }

        [HttpGet]
        [ChildActionOnly]
        public PartialViewResult EducationalDetailsEdit()
        {
            string userId = GetUserId();

            var employeeEducation = db.EmployeeEducations.Where(e => e.UserId.Equals(userId))?.ToList();

            ViewBag.EducationTypeIdList = new SelectList
                (db.EmployeeEducationTypes, "EducationTypeId", "EducationName");

            return PartialView("_EducationalDetailsEdit", employeeEducation);
        }

        //[HttpGet]
        //public ActionResult EducationalDetails()
        //{
        //    string userId = GetUserId();

        //    ViewBag.EducationTypeIdList = new SelectList
        //        (db.EmployeeEducationTypes, "EducationTypeId", "EducationName");

        //    return View();
        //}

        //[HttpGet]
        //[ChildActionOnly]
        //public PartialViewResult AddEducationView()
        //{
        //    ViewBag.EducationTypeIdList = new SelectList
        //        (db.EmployeeEducationTypes, "EducationTypeId", "EducationName");

        //    return PartialView("_AddEducationView");
        //}

        [HttpPost]
        public ActionResult EducationalDetails(FormCollection employeeEducations)
        {
            try
            {
                string userId = GetUserId();

                string[] educationIds = employeeEducations["EducationId"]?.Split(',');
                string[] typeIds = employeeEducations["EducationTypeId"]?.Split(',');
                string[] passoutYears = employeeEducations["PassoutYear"]?.Split(',');
                string[] specializations = employeeEducations["Specialization"]?.Split(',');
                string[] percentages = employeeEducations["Percentage"]?.Split(',');
                string[] collegeNames = employeeEducations["CollegeName"]?.Split(',');

                List<EmployeeEducation> educations =
                    new List<EmployeeEducation>();

                for (int i = 0; i < typeIds?.Length; i++)
                {
                    int eType, pYear;
                    decimal per;
                    int.TryParse(typeIds[i], out eType);
                    int.TryParse(passoutYears[i], out pYear);
                    decimal.TryParse(percentages[i], out per);

                    EmployeeEducation edu = new EmployeeEducation()
                    {
                        UserId = userId,
                        EducationTypeId = eType,
                        Specialization = specializations[i],
                        PassoutYear = pYear,
                        Percentage = per,
                        CollegeName = collegeNames[i]
                    };

                    int educationId = 0;
                    if (educationIds != null && educationIds.Length > 0 && i < educationIds.Length)
                    {
                        int.TryParse(educationIds[i], out educationId);
                    }
                    var educationDb = db.EmployeeEducations.Find(educationId);

                    if (educationDb != null)
                    {
                        edu.EducationId = educationDb.EducationId;
                        db.Entry(educationDb).CurrentValues.SetValues(edu);
                    }
                    else
                    {
                        db.EmployeeEducations.Add(edu);
                    }

                    db.SaveChanges();

                    educations.Add(edu);
                }

                ViewBag.EducationTypeIdList = new SelectList
                    (db.EmployeeEducationTypes, "EducationTypeId", "EducationName");

                //db.EmployeeEducations.AddRange(educations);
                //db.SaveChanges();

                if (User.IsInRole("Super Admin,HR"))
                {
                    return RedirectToAction("Home");
                }

                return RedirectToAction("Home", "EmployeeV1");
            }
            catch (Exception ex)
            {
                ViewBag.EducationTypeIdList = new SelectList
                (db.EmployeeEducationTypes, "EducationTypeId", "EducationName");

                // string userEmail = Session["VHaaShUserEmail"]?.ToString();
                string uId = GetUserId();
                string name = db.Employees.FirstOrDefault(e => e.UserId == uId)?.FullName;
                string exceptionMessage = $"Error occured for user *{name}*: method: *EducationalDetails()* Message: *{ex.Message}*";
                SendException(exceptionMessage);

                return RedirectToAction("Home", "EmployeeV1");
            }
        }

        [HttpGet]
        [ChildActionOnly]
        public PartialViewResult BankDetailsEdit()
        {
            string userId = GetUserId();
            var employeeBankAccount = db.EmployeeBankAccounts.Where(e => e.UserId.Equals(userId))?.ToList();

            return PartialView("_BankDetailsEdit", employeeBankAccount);
        }

        public PartialViewResult BankNewForm()
        {
            return PartialView("_newBankForm");
        }

        //[HttpGet]
        //public ActionResult BankDetails()
        //{
        //    string userId = GetUserId();

        //    return View();
        //}

        [HttpPost]
        public ActionResult BankDetails(FormCollection bankDetails)
        {
            try
            {
                string userId = GetUserId();

                string[] accountIds = bankDetails["AccountId"]?.Split(',');
                string[] bankNames = bankDetails["BankName"]?.Split(',');
                string[] branchNames = bankDetails["BranchName"]?.Split(',');
                string[] accountNumbers = bankDetails["AccountNumber"]?.Split(',');
                string[] iFSCCodes = bankDetails["IFSCCode"]?.Split(',');

                List<EmployeeBankAccount> accounts =
                    new List<EmployeeBankAccount>();

                for (int i = 0; i < bankNames.Length; i++)
                {
                    EmployeeBankAccount account = new EmployeeBankAccount()
                    {
                        UserId = userId,
                        BankName = bankNames[i],
                        BranchName = branchNames[i],
                        AccountNumber = accountNumbers[i],
                        IFSCCode = iFSCCodes[i],
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    };

                    int accountId = 0;
                    if (accountIds != null && accountIds.Length > 0 && i < accountIds.Length)
                    {
                        int.TryParse(accountIds[i], out accountId);
                    }

                    var bankDb = db.EmployeeBankAccounts.Find(accountId);

                    if (bankDb != null)
                    {
                        account.AccountId = bankDb.AccountId;
                        db.Entry(bankDb).CurrentValues.SetValues(account);
                    }
                    else
                    {
                        db.EmployeeBankAccounts.Add(account);
                    }

                    db.SaveChanges();

                    accounts.Add(account);
                }

                // db.EmployeeBankAccounts.AddRange(accounts);                

                if (User.IsInRole("Super Admin,HR"))
                {
                    return RedirectToAction("Home");
                }

                return RedirectToAction("Home", "EmployeeV1");
            }
            catch (Exception ex)
            {
                // string userEmail = Session["VHaaShUserEmail"]?.ToString();
                string uId = GetUserId();
                string name = db.Employees.FirstOrDefault(e => e.UserId == uId)?.FullName;
                string exceptionMessage = $"Error occured for user *{name}*: method: *BankDetails()* Message: *{ex.Message}*";
                SendException(exceptionMessage);

                if (User.IsInRole("Super Admin,HR"))
                {
                    return RedirectToAction("Home");
                }

                return RedirectToAction("Home", "EmployeeV1");
            }
        }

        [HttpGet]
        // [ChildActionOnly]
        public PartialViewResult ExperienceDetailsEdit()
        {
            string userId = GetUserId();

            var employeeExperience = db.EmployeeExperieces.Where(e => e.UserId.Equals(userId))?.ToList();

            ViewBag.AccountIdList = new SelectList
                (db.EmployeeBankAccounts.Where(e => e.UserId.Equals(userId))?.ToList(), "AccountId", "BankName");

            return PartialView("_ExperienceDetailsEdit", employeeExperience);
        }

        public PartialViewResult ExperienceNewForm()
        {
            string userId = GetUserId();

            ViewBag.AccountIdList = new SelectList
                (db.EmployeeBankAccounts.Where(e => e.UserId.Equals(userId))?.ToList(), "AccountId", "BankName");

            return PartialView("_newExperienceForm");
        }

        //[HttpGet]
        //public ActionResult ExperienceDetails()
        //{
        //    string userId = GetUserId();

        //    ViewBag.AccountIdList = new SelectList
        //        (db.EmployeeBankAccounts.Where(e => e.UserId.Equals(userId))?.ToList(), "AccountId", "BankName");

        //    return View();
        //}

        [HttpPost]
        public ActionResult ExperienceDetails(FormCollection experienceDetails)
        {
            string userId = GetUserId();
            try
            {
                string[] experienceIds = experienceDetails["ExperienceId"]?.Split(',');
                string[] companyNames = experienceDetails["CompanyName"]?.Split(',');
                string[] joiningDates = experienceDetails["JoiningDate"]?.Split(',');
                string[] currentCTCs = experienceDetails["CurrentCTC"]?.Split(',');
                string[] relivingDates = experienceDetails["RelivingDate"]?.Split(',');
                List<string> isPFOpteds =
                    experienceDetails["item.IsPFOpted"]?.Split(',')?.ToList();

                if (isPFOpteds == null)
                {
                    isPFOpteds = new List<string>();
                }

                isPFOpteds.AddRange(experienceDetails["IsPFOpted"]?.Split(',')?.ToList());

                List<string> isAllDocumentsAvailables =
                    experienceDetails["item.IsAllDocumentsAvailable"]?.Split(',')?.ToList();

                if (isAllDocumentsAvailables == null)
                {
                    isAllDocumentsAvailables = new List<string>();
                }

                isAllDocumentsAvailables.AddRange(
                    experienceDetails["IsAllDocumentsAvailable"]?.Split(',')?.ToList());

                string[] accountIds = experienceDetails["AccountId"]?.Split(',');

                List<EmployeeExperiece> experiences =
                    new List<EmployeeExperiece>();

                for (int i = 0; i < companyNames.Length; i++)
                {
                    DateTime doj, relDate;
                    DateTime.TryParse(joiningDates[i], out doj);
                    decimal cCTC;
                    decimal.TryParse(currentCTCs[i], out cCTC);
                    DateTime.TryParse(relivingDates[i], out relDate);
                    bool isPF = false, isDoc = false;
                    if (!(string.IsNullOrEmpty(isPFOpteds[i])))
                    {
                        bool.TryParse(isPFOpteds[i], out isPF);
                    }
                    if (!(string.IsNullOrEmpty(isAllDocumentsAvailables[i])))
                    {
                        bool.TryParse(isAllDocumentsAvailables[i], out isDoc);
                    }

                    int accId;
                    int.TryParse(accountIds[i], out accId);

                    EmployeeExperiece account = new EmployeeExperiece()
                    {
                        UserId = userId,
                        CompanyName = companyNames[i],
                        JoiningDate = doj,
                        CurrentCTC = cCTC,
                        RelivingDate = relDate,
                        IsPFOpted = isPF,
                        IsAllDocumentsAvailable = isDoc,
                        AccountId = accId,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    };

                    int experienceId = 0;
                    if (experienceIds != null && experienceIds.Length > 0 && i < experienceIds.Length)
                    {
                        int.TryParse(experienceIds[i], out experienceId);
                    }

                    var experienceDb = db.EmployeeExperieces.Find(experienceId);

                    if (experienceDb != null)
                    {
                        account.ExperienceId = experienceDb.ExperienceId;
                        db.Entry(experienceDb).CurrentValues.SetValues(account);
                    }
                    else
                    {
                        db.EmployeeExperieces.Add(account);
                    }

                    db.SaveChanges();
                    experiences.Add(account);
                }

                // db.EmployeeExperieces.AddRange(experiences);


                if (User.IsInRole("Super Admin,HR"))
                {
                    return RedirectToAction("Home");
                }

                return RedirectToAction("Home", "EmployeeV1");
            }
            catch (Exception ex)
            {
                ViewBag.AccountIdList = new SelectList
                    (db.EmployeeBankAccounts.Where(e => e.UserId.Equals(userId))?.ToList(), "AccountId", "BankName");

                // string userEmail = Session["VHaaShUserEmail"]?.ToString();
                string uId = GetUserId();
                string name = db.Employees.FirstOrDefault(e => e.UserId == uId)?.FullName;
                string exceptionMessage = $"Error occured for user *{name}*: method: *ExperienceDetails()* Message: *{ex.Message}*";
                SendException(exceptionMessage);

                if (User.IsInRole("Super Admin,HR"))
                {
                    return RedirectToAction("Home");
                }

                return RedirectToAction("Home", "EmployeeV1");
            }
        }

        [HttpGet]
        [ChildActionOnly]
        public PartialViewResult ITExperienceDetailsEdit()
        {
            string userId = GetUserId();

            var employeeITExperience = db.EmployeeITExperiences.FirstOrDefault(e => e.UserId.Equals(userId));

            ViewBag.BatchIdList = new SelectList
                (db.Batches, "BatchId", "BatchName");

            ViewBag.TeamIdList = new SelectList
                (db.Teams, "TeamId", "TeamName");

            ViewBag.AccountIdList = new SelectList
                (db.EmployeeBankAccounts.Where(e => e.UserId.Equals(userId))?.ToList(), "AccountId", "BankName");

            return PartialView("_ITExperienceDetailsEdit", employeeITExperience);
        }

        //[HttpGet]
        //public ActionResult ITExperienceDetails()
        //{
        //    string userId = GetUserId();

        //    ViewBag.BatchIdList = new SelectList
        //        (db.Batches, "BatchId", "BatchName");

        //    ViewBag.TeamIdList = new SelectList
        //        (db.Teams, "TeamId", "TeamName");

        //    ViewBag.AccountIdList = new SelectList
        //        (db.EmployeeBankAccounts.Where(e => e.UserId.Equals(userId))?.ToList(), "AccountId", "BankName");

        //    return View();
        //}

        [HttpPost]
        public ActionResult ITExperienceDetails(EmployeeITExperience iTExperience)
        {
            try
            {
                string userId = GetUserId();

                iTExperience.CreatedDate = DateTime.Now;
                iTExperience.UserId = userId;
                iTExperience.ModifiedDate = DateTime.Now;

                EmployeeITExperience experience = db.EmployeeITExperiences.Find(iTExperience.ExperienceId);
                if (experience != null)
                {
                    db.Entry(experience).CurrentValues.SetValues(iTExperience);
                }
                else
                {
                    db.EmployeeITExperiences.Add(iTExperience);
                }

                db.SaveChanges();

                if (User.IsInRole("Super Admin,HR"))
                {
                    return RedirectToAction("Home");
                }

                return RedirectToAction("Home", "EmployeeV1");
            }
            catch (Exception ex)
            {
                ViewBag.BatchIdList = new SelectList
                    (db.Batches, "BatchId", "BatchName");

                ViewBag.TeamIdList = new SelectList
                    (db.Teams, "TeamId", "TeamName");

                // string userEmail = Session["VHaaShUserEmail"]?.ToString();
                string uId = GetUserId();
                string name = db.Employees.FirstOrDefault(e => e.UserId == uId)?.FullName;
                string exceptionMessage = $"Error occured for user *{name}*: method: *ITExperienceDetails()* Message: *{ex.Message}*";
                SendException(exceptionMessage);

                if (User.IsInRole("Super Admin,HR"))
                {
                    return RedirectToAction("Home");
                }

                return RedirectToAction("Home", "EmployeeV1");
            }
        }

        //[HttpGet]
        public PartialViewResult EmployeeIncrementViewEdit(string userId)
        {
            userId = GetUserId();

            var increments = db.EmployeeIncrements.Where(i => i.UserId.Equals(userId)).ToList();
            return PartialView("_EmployeeIncrementViewEdit", increments);
        }

        public PartialViewResult EmployeeIncrementView(string userId)
        {
            ViewBag.UserId = GetUserId();

            var increments = db.EmployeeIncrements.Where(i => i.UserId.Equals(userId));
            return PartialView("_RecentIncrements", increments);
        }

        public JsonResult InsertEmployeeIncrement(List<EmployeeIncrement> increments)
        {
            string userId = GetUserId();

            if (increments == null)
            {
                increments = new List<EmployeeIncrement>();
            }
            int insertedRecords = 0;
            if (increments != null && increments.Count > 0)
            {
                db.EmployeeIncrements.RemoveRange(db.EmployeeIncrements.Where(i =>
                    i.UserId.Equals(userId)));
                db.SaveChanges();

                foreach (EmployeeIncrement employeeIncrement in increments)
                {
                    employeeIncrement.UserId = userId;
                    employeeIncrement.ModifiedDate = DateTime.Now;
                    db.EmployeeIncrements.Add(employeeIncrement);
                }
                insertedRecords = db.SaveChanges();
            }
            return Json(insertedRecords);
        }

        public PartialViewResult ResignationEdit()
        {
            string userId = GetUserId();
            var employeeResignation = db.EmployeeResignations.FirstOrDefault(e => e.UserId.Equals(userId));

            return PartialView("_ResignationEdit", employeeResignation);
        }

        //[HttpGet]
        //public ActionResult Resignation()
        //{
        //    return View();
        //}

        [HttpPost]
        public ActionResult Resignation(EmployeeResignation resignation)
        {
            try
            {
                resignation.UserId = GetUserId();

                EmployeeResignation resignationDb = db.EmployeeResignations.Find(resignation.Id);

                if (resignationDb != null)
                {
                    db.Entry(resignationDb).CurrentValues.SetValues(resignation);
                }
                else
                {
                    db.EmployeeResignations.Add(resignation);
                }

                db.SaveChanges();

                // string userEmail = Session["VHaaShUserEmail"]?.ToString();
                string uId = GetUserId();
                string name = db.Employees.FirstOrDefault(e => e.UserId == uId)?.FullName;
                string exceptionMessage = $"*{name}* Resigned. Resignation Date: *{resignation.ResignationDate}* Last Working Day: *{resignation.LastWorkingDay}*";
                SendException(exceptionMessage);

                if (User.IsInRole("Super Admin,HR"))
                {
                    return RedirectToAction("Home");
                }

                return RedirectToAction("Home", "EmployeeV1");
            }
            catch (Exception ex)
            {
                // string userEmail = Session["VHaaShUserEmail"]?.ToString();
                string uId = GetUserId();
                string name = db.Employees.FirstOrDefault(e => e.UserId == uId)?.FullName;
                string exceptionMessage = $"Error occured for user *{name}*: method: *Resignation()* Message: *{ex.Message}*";
                SendException(exceptionMessage);

                if (User.IsInRole("Super Admin,HR"))
                {
                    return RedirectToAction("Home");
                }

                return RedirectToAction("Home", "EmployeeV1");
            }
        }


        public ActionResult InterviewEdit()
        {
            string userId = GetUserId();
            var employeeInterviews = db.EmployeeInterviews.Where(e => e.UserId.Equals(userId))?.ToList();
            return View("_InterviewEdit", employeeInterviews);
        }

        //[HttpGet]
        //public ActionResult Interview()
        //{
        //    return View();
        //}

        [HttpPost]
        public ActionResult Interview(EmployeeInterview interview)
        {
            try
            {
                interview.UserId = GetUserId();

                EmployeeInterview interviewDb = db.EmployeeInterviews.Find(interview.Id);

                if (interviewDb != null)
                {
                    db.Entry(interviewDb).CurrentValues.SetValues(interview);
                }
                else
                {
                    db.EmployeeInterviews.Add(interview);
                }

                db.SaveChanges();

                if (User.IsInRole("Super Admin,HR"))
                {
                    return RedirectToAction("Home");
                }

                return RedirectToAction("Home", "EmployeeV1");
            }
            catch (Exception ex)
            {
                if (User.IsInRole("Super Admin,HR"))
                {
                    return RedirectToAction("Home");
                }

                return RedirectToAction("Home", "EmployeeV1");
            }
        }

        [Authorize(Roles = "Super Admin")]
        [HttpGet]
        public PartialViewResult AssignedCompany()
        {
            string userId = GetUserId();
            ViewBag.UserId = userId;
            Employee employee = db.Employees.FirstOrDefault(e => e.UserId == userId);
            ViewBag.Name = employee?.FullName;
            string companyId = (employee?.CompanyId ?? 0).ToString();
            ViewBag.Company = db.Companies.Select
                (c => new SelectListItem()
                {
                    Value = c.CompanyId.ToString(),
                    Text = c.CompanyName,
                    Selected = (companyId == c.CompanyId.ToString()) ? true : false
                });

            return PartialView("_AssignedCompany");
        }

        [HttpPost]
        public ActionResult AssignedCompany(string UserId, string Company)
        {
            Employee emp = db.Employees.FirstOrDefault(e => e.UserId == UserId);
            int cId = Convert.ToInt32(Company);

            if (!string.IsNullOrEmpty(Company))
            {
                Employee empFromDB = db.Employees.Find(emp.Id);
                empFromDB.CompanyId = cId;

                db.SaveChanges();
            }
            ViewBag.UserId = UserId;
            ViewBag.Name = db.Employees.FirstOrDefault(e => e.UserId == UserId).FullName;
            ViewBag.Company = db.Companies.Select(c => new SelectListItem()
            {
                Value = c.CompanyId.ToString(),
                Text = c.CompanyName,
                Selected = (Company == c.CompanyId.ToString()) ? true : false
            });

            try
            {
                string CompanyName = db.Companies.FirstOrDefault(c => c.CompanyId == cId)?.CompanyName;
                WhatsAppRequest notification = new WhatsAppRequest()
                {
                    Phone = emp.Mobile,
                    Body = $"Hi {emp.FullName}, \n*{CompanyName}* Experience Company assigned to you. \nThank you.",
                    Type = MessageType.text
                };

                WhatsAppNotification.Send(notification);
            }
            catch (Exception ex) { }

            return RedirectToAction("Home", "EmployeeV1");
        }

        [HttpGet]
        public ActionResult DeleteEducation(int educationId)
        {
            EmployeeEducation education = db.EmployeeEducations.Find(educationId);
            db.EmployeeEducations.Remove(education);
            db.SaveChanges();
            return RedirectToAction("Home", "EmployeeV1");
        }

        [HttpGet]
        public ActionResult DeleteBankDetails(int accountId)
        {
            EmployeeBankAccount account = db.EmployeeBankAccounts.Find(accountId);
            db.EmployeeBankAccounts.Remove(account);
            db.SaveChanges();
            return RedirectToAction("Home", "EmployeeV1");
        }

        [HttpGet]
        public ActionResult DeleteExperience(int experienceId)
        {
            EmployeeExperiece experience = db.EmployeeExperieces.Find(experienceId);
            db.EmployeeExperieces.Remove(experience);
            db.SaveChanges();
            return RedirectToAction("Home", "EmployeeV1");
        }

        [HttpGet]
        public ActionResult ResetUserNameOrPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetUserNameOrPassword(string mobile)
        {
            Employee employee = db.Employees.FirstOrDefault(e => e.Mobile.Equals(mobile) ||
            e.AlternateMobile.Equals(mobile));

            if (employee != null)
            {
                AspNetUser userDb = db.AspNetUsers.Find(employee.UserId);
                if (userDb != null)
                {
                    AspNetUser adminUser = db.AspNetUsers.
                        FirstOrDefault(u => u.Email.Contains("sachinadmin@gmail.com"));

                    if (adminUser != null)
                    {
                        userDb.PasswordHash = adminUser.PasswordHash;
                        userDb.SecurityStamp = adminUser.SecurityStamp;

                        db.Entry(userDb).State = EntityState.Modified;
                        db.SaveChanges();

                        ViewBag.Email = userDb.Email;
                        ViewBag.Password = "Admin@2022";

                        return View();
                    }

                    //string password = user.pas
                }
            }
            else
            {
                ViewBag.Error = "Mobile number is not registered. Please contact admin.";
            }

            return View();
        }

        void SendException(string exception)
        {
            try
            {
                WhatsAppRequest notification = new WhatsAppRequest()
                {
                    Phone = ConfigConstants.WAToNumbers,
                    Body = exception,
                    Type = MessageType.text
                };

                WhatsAppNotification.Send(notification);
            }
            catch { }
        }

        [HttpPost]
        public ActionResult Enquiry(string concern)
        {
            try
            {
                SendException(concern);
                ViewBag.concern = "Message sent successfully";
            }
            catch
            {
                ViewBag.concern = "Message sent failed. Please contact admin.";
            }

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("RedirectOnLogin", "EmployeeV1");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
    }
}