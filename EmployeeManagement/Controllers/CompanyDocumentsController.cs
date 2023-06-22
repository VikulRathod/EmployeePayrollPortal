using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using EmployeeManagement.Models;
using EmployeeManagement.Utility;
using Microsoft.AspNet.Identity;
using Microsoft.Office.Interop.Word;

namespace EmployeeManagement.Controllers
{
    public class CompanyDocumentsController : Controller
    {
        private EmployeeManagementDBContext db = new EmployeeManagementDBContext();

        // GET: CompanyDocuments
        public ActionResult Index()
        {
            var companyDocuments = db.CompanyDocuments.Include(c => c.AspNetUser)
                .Include(c1 => c1.Company).Include(c2 => c2.CompanyDocumentsType);

            return View(companyDocuments.ToList());
        }

        // GET: CompanyDocuments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyDocument companyDocument = db.CompanyDocuments.Find(id);
            if (companyDocument == null)
            {
                return HttpNotFound();
            }
            Session["document"] = companyDocument;

            return View(companyDocument);
        }

        // GET: CompanyDocuments/Create
        public ActionResult Create()
        {
            string loggedInUserId = User.Identity.GetUserId();

            ViewBag.CreatedBy = new SelectList
                (db.AspNetUsers.Where(u => u.Id.Equals(loggedInUserId)), "Id", "Email");

            ViewBag.DocumentType = db.CompanyDocumentsTypes.ToList().Select(cd =>
            new SelectListItem()
            {
                Value = cd.DocumentTypeId.ToString(),
                Text = cd.DocumentTypeName
            }).ToList();

            //ViewBag.DocumentType = new List<SelectListItem>()
            //{
            //    new SelectListItem(){ Text = "Offer Letter Format", Value = "OfferLetter" },
            //    new SelectListItem(){ Text = "Increment Letter Format", Value = "IncrementLetter" },
            //    new SelectListItem(){ Text = "Salary Slip Format", Value = "SalarySlip" },
            //    new SelectListItem(){ Text = "Reliving Letter Format", Value = "RelivingLetter" },
            //    new SelectListItem(){ Text = "Experience Letter Format", Value = "ExperienceLetter" }
            //};

            ViewBag.CompanyIdList = new SelectList
                (db.Companies, "CompanyId", "CompanyName");

            return View();
        }

        // POST: CompanyDocuments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DocumentName,CompanyId,DocumentPath,DocumentFile,CreatedBy,CreatedDate")] CompanyDocument companyDocument)
        {
            if (ModelState.IsValid)
            {

                string companyName = db.Companies.FirstOrDefault(c => c.CompanyId == companyDocument.CompanyId).
                    CompanyName;

                string fileName = Path.GetFileNameWithoutExtension(companyDocument.DocumentFile.FileName);
                string fileExtension = Path.GetExtension(companyDocument.DocumentFile.FileName);

                fileName = fileName + DateTime.Now.ToString("yymm") + fileExtension;

                System.IO.Directory.CreateDirectory(Server.MapPath("~/documents/"));
                System.IO.Directory.CreateDirectory(Server.MapPath("~/documents/" + companyName));

                //companyDocument.DocumentPath = "~/documents/" + companyName + "/" + fileName;

                fileName = Path.Combine(Server.MapPath("~/documents/" + companyName), fileName);
                companyDocument.DocumentFile.SaveAs(fileName);

                companyDocument.DocumentType = fileExtension.TrimStart(new char[] { '.' });

                companyDocument.DocumentPath = System.IO.File.ReadAllBytes(fileName);
                companyDocument.CompanyDocumentsTypeId = int.Parse(companyDocument.DocumentName);
                companyDocument.CreatedDate = DateTime.Now;

                db.CompanyDocuments.Add(companyDocument);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            string loggedInUserId = User.Identity.GetUserId();

            ViewBag.CreatedBy = new SelectList
                (db.AspNetUsers.Where(u => u.Id.Equals(loggedInUserId)), "Id", "Email");

            ViewBag.DocumentType = db.CompanyDocumentsTypes.ToList().Select(cd =>
            new SelectListItem()
            {
                Value = cd.DocumentTypeId.ToString(),
                Text = cd.DocumentTypeName
            }).ToList();

            //ViewBag.DocumentType = new List<SelectListItem>()
            //{
            //    new SelectListItem(){ Text = "Offer Letter Format", Value = "OfferLetter" },
            //    new SelectListItem(){ Text = "Increment Letter Format", Value = "IncrementLetter" },
            //    new SelectListItem(){ Text = "Salary Slip Format", Value = "SalarySlip" },
            //    new SelectListItem(){ Text = "Reliving Letter Format", Value = "RelivingLetter" },
            //    new SelectListItem(){ Text = "Experience Letter Format", Value = "ExperienceLetter" }
            //};

            ViewBag.CompanyIdList = new SelectList
                (db.Companies, "CompanyId", "CompanyName");

            return View(companyDocument);
        }

        // GET: CompanyDocuments/Edit/5
        public ActionResult Edit(int? id)
        {
            CompanyDocument companyDocument = db.CompanyDocuments.Find(id);
            if (companyDocument == null)
            {
                return HttpNotFound();
            }

            string loggedInUserId = User.Identity.GetUserId();

            var documentTypes = db.CompanyDocumentsTypes.ToList().Select(cd =>
            new SelectListItem()
            {
                Value = cd.DocumentTypeId.ToString(),
                Text = cd.DocumentTypeName
            }).ToList();

            //var documentTypes = new List<SelectListItem>()
            //{
            //    new SelectListItem(){ Text = "Offer Letter Format", Value = "OfferLetter" },
            //    new SelectListItem(){ Text = "Increment Letter Format", Value = "IncrementLetter" },
            //    new SelectListItem(){ Text = "Salary Slip Format", Value = "SalarySlip" },
            //    new SelectListItem(){ Text = "Reliving Letter Format", Value = "RelivingLetter" },
            //    new SelectListItem(){ Text = "Experience Letter Format", Value = "ExperienceLetter" }
            //};

            ViewBag.DocumentType = documentTypes.Select(bg =>
            new SelectListItem()
            { Text = bg.Text, Value = bg.Value, Selected = (int.Parse(bg.Value).Equals(companyDocument.CompanyDocumentsTypeId)) }
            ).ToList();

            ViewBag.CompanyIdList = new SelectList
                (db.Companies, "CompanyId", "CompanyName", companyDocument.CompanyId);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Session["document"] = companyDocument;

            ViewBag.CreatedBy = new SelectList(db.AspNetUsers.Where(u => u.Id.Equals(loggedInUserId)), "Id", "Email", companyDocument.CreatedBy);
            return View(companyDocument);
        }

        // POST: CompanyDocuments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DocumentName,CompanyId,DocumentPath,DocumentFile,CreatedBy,CreatedDate")] CompanyDocument companyDocument)
        {
            if (ModelState.IsValid)
            {
                string companyName = db.Companies.FirstOrDefault(c => c.CompanyId == companyDocument.CompanyId).
                    CompanyName;

                if (companyDocument.DocumentFile != null && !string.IsNullOrEmpty(companyDocument.DocumentFile.FileName))
                {
                    string fileName = Path.GetFileNameWithoutExtension(companyDocument.DocumentFile.FileName);
                    string fileExtension = Path.GetExtension(companyDocument.DocumentFile.FileName);

                    fileName = fileName + DateTime.Now.ToString("yymm") + fileExtension;
                    System.IO.Directory.CreateDirectory(Server.MapPath("~/documents/"));
                    System.IO.Directory.CreateDirectory(Server.MapPath("~/documents/" + companyName));

                    //companyDocument.DocumentPath = "~/documents/" + companyName + "/" + fileName;

                    fileName = Path.Combine(Server.MapPath("~/documents/" + companyName), fileName);
                    companyDocument.DocumentFile.SaveAs(fileName);

                    companyDocument.DocumentType = fileExtension.TrimStart(new char[] { '.' });
                }

                var existingCompanyDocument = db.CompanyDocuments.FirstOrDefault(cd => cd.Id == companyDocument.Id);

                existingCompanyDocument.CompanyId = companyDocument.CompanyId;
                existingCompanyDocument.DocumentName = companyDocument.DocumentName;
                existingCompanyDocument.CreatedBy = companyDocument.CreatedBy;
                existingCompanyDocument.CreatedDate = DateTime.Now;

                //if (!string.IsNullOrEmpty(companyDocument.DocumentPath))
                //{
                //    existingCompanyDocument.DocumentPath = companyDocument.DocumentPath;
                //    existingCompanyDocument.DocumentType = companyDocument.DocumentType;
                //}

                //db.Entry(companyDocument).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreatedBy = new SelectList(db.AspNetUsers, "Id", "Email", companyDocument.CreatedBy);
            return View(companyDocument);
        }

        // GET: CompanyDocuments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyDocument companyDocument = db.CompanyDocuments.Find(id);
            if (companyDocument == null)
            {
                return HttpNotFound();
            }
            return View(companyDocument);
        }

        // POST: CompanyDocuments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CompanyDocument companyDocument = db.CompanyDocuments.Find(id);
            db.CompanyDocuments.Remove(companyDocument);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //public void ShowDocument(CompanyDocument document)
        public void ShowDocument()
        {
            CompanyDocument document = (CompanyDocument)Session["document"];

            //Split the string by character . to get file extension type  
            // string[] stringParts = document.DocumentName.Split(new char[] { '.' });

            string documentName = db.CompanyDocumentsTypes.Where(cd =>
            cd.DocumentTypeId == document.CompanyDocumentsTypeId).FirstOrDefault()?.DocumentTypeName;

            string strType = document.DocumentType;
            string tempFilName = Server.MapPath("~/Temp Files/" + document.DocumentName + "." + document.DocumentType);

            System.IO.File.WriteAllBytes(tempFilName, document.DocumentPath);

            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            //Response.AddHeader("content-disposition", "attachment; filename=" + document.DocumentName + "." + strType);

            //Response.AddHeader("content-disposition", "attachment; filename=" + Server.MapPath("~/Temp Files/" + documentName + "." + strType));
            Response.AddHeader("content-disposition", "attachment; filename=" + documentName + "." + strType);

            //Set the content type as file extension type  
            Response.ContentType = strType;
            //Write the file content  

            this.Response.BinaryWrite(System.IO.File.ReadAllBytes(tempFilName));
            this.Response.End();

            System.IO.File.Delete(tempFilName);
        }

        public ActionResult DownloadDocument(int? id)
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
                loggedInUserId = employee.UserId;
            }

            Session["userId"] = loggedInUserId;

            //var documents = db.Employees.Where(e => e.UserId.Equals(loggedInUserId)).Join(db.CompanyDocuments,
            //     e => e.CompanyId,
            //     c => c.CompanyId,
            //     (e1, c1) => c1).Select(company => company.DocumentName);

            var documents = from e in db.Employees
                            where e.UserId.Equals(loggedInUserId)
                            join d in db.CompanyDocuments
                            on e.CompanyId equals d.CompanyId
                            join dt in db.CompanyDocumentsTypes
                            on d.CompanyDocumentsTypeId equals dt.DocumentTypeId
                            select dt.DocumentTypeName;

            ViewBag.DocumentType = documents.Select(d => new SelectListItem() { Value = d, Text = d });

            return View();
        }

        [HttpPost]
        public void DownloadDocumentEmployee(string DocumentName, string MonthYear)
        {
            string loggedInUserId = User.Identity.GetUserId();

            if (Session["userId"] != null)
            {
                loggedInUserId = Session["userId"].ToString();
            }

            var data = from e in db.Employees
                       where e.UserId.Equals(loggedInUserId)
                       join d in db.CompanyDocuments
                       on e.CompanyId equals d.CompanyId
                       join dt in db.CompanyDocumentsTypes
                       on d.CompanyDocumentsTypeId equals dt.DocumentTypeId
                       where dt.DocumentTypeName == DocumentName
                       select new
                       {
                           employee = e,
                           companyDocument = d,
                           companyDocumentType = dt
                       };

            //var data = db.Employees.Where(e => e.UserId.Equals(loggedInUserId)).Join(db.CompanyDocuments,
            //     e => e.CompanyId,
            //     c => c.CompanyId,
            //     (e1, c1) => new { employee = e1, companyDocument = c1 }).
            //     FirstOrDefault(cd => cd.companyDocument.DocumentName.Equals(DocumentName));

            var document = data.FirstOrDefault().companyDocument;
            document.DocumentName = document.CompanyDocumentsType.DocumentTypeName;

            string companyName = db.Companies.FirstOrDefault(c => c.CompanyId == document.CompanyId).CompanyName;

            // string tempFilName = Path.Combine(Server.MapPath("~/documents/" + companyName + "/"), document.DocumentName + "." + document.DocumentType);
            string tempFilName = Server.MapPath("~/Temp Files/" + document.DocumentName + " Copy." + document.DocumentType);
            System.IO.File.WriteAllBytes(tempFilName, document.DocumentPath);

            string tempNewFilName = Server.MapPath("~/Temp Files/" + document.DocumentName + "." + document.DocumentType);
            CreateWordDocument(tempFilName, tempNewFilName, data.FirstOrDefault().employee, data.FirstOrDefault().companyDocument.Id, MonthYear, document.DocumentName);

            byte[] fileContent = System.IO.File.ReadAllBytes(tempNewFilName);

            //Split the string by character . to get file extension type  
            // string[] stringParts = document.DocumentName.Split(new char[] { '.' });
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("content-disposition", "attachment; filename=" + document.DocumentName + "." + document.DocumentType);
            //Response.AddHeader("content-disposition", "attachment; filename=" + document.DocumentName + ".pdf");
            //Set the content type as file extension type  
            Response.ContentType = document.DocumentType;
            //Write the file content  
            this.Response.BinaryWrite(fileContent);
            this.Response.End();

            System.IO.File.Delete(tempFilName);
            System.IO.File.Delete(tempNewFilName);
        }

        private void FindAndReplace(Microsoft.Office.Interop.Word.Application wordApp, object toFindText, object replaceWithText)
        {
            object matchCase = true;

            object matchwholeWord = true;

            object matchwildCards = false;

            object matchSoundLike = false;

            object nmatchAllforms = false;

            object forward = true;

            object format = false;

            object matchKashida = false;

            object matchDiactitics = false;

            object matchAlefHamza = false;

            object matchControl = false;

            object read_only = true;

            object visible = true;

            object replace = 2;

            object wrap = 1;

            wordApp.Selection.Find.Execute(ref toFindText, ref matchCase,
                                            ref matchwholeWord, ref matchwildCards, ref matchSoundLike,

                                            ref nmatchAllforms, ref forward,

                                            ref wrap, ref format, ref replaceWithText,

                                                ref replace, ref matchKashida,

                                            ref matchDiactitics, ref matchAlefHamza,

                                             ref matchControl);
        }

        private void CreateWordDocument(object filename, object SaveAs, Employee employee, int documentId, string MonthYear, string DocumentName)
        {
            Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
            object missing = Missing.Value;

            Microsoft.Office.Interop.Word.Document myWordDoc = null;

            if (System.IO.File.Exists((string)filename))
            {
                object readOnly = true;

                object isvisible = false;

                wordApp.Visible = false;
                myWordDoc = wordApp.Documents.Open(ref filename, ref missing, ref readOnly,
                                                    ref missing, ref missing, ref missing,
                                                    ref missing, ref missing, ref missing,
                                                    ref missing, ref missing, ref missing,
                                                     ref missing, ref missing, ref missing, ref missing);
                myWordDoc.Activate();

                var documentValues = db.CompanyDocumentConfigurableValues.Where(d => d.CompanyId == employee.CompanyId
                && d.CompanyDocumentId == documentId);

                Dictionary<string, DocumentFormatValue> documentDirectValues = new Dictionary<string, DocumentFormatValue>();

                foreach (CompanyDocumentConfigurableValue item in documentValues)
                {
                    if (item.ValueType.Equals("Direct Value From DB"))
                    {
                        var value = string.Empty;

                        if (employee.GetType().GetProperty(item.FieldValue) != null)
                        {
                            value = employee.GetType().GetProperty(item.FieldValue).GetValue(employee, null).ToString();
                        }
                        else
                        {
                            if (DocumentName.ToUpper().Contains("INCREMENT") || DocumentName.ToUpper().Contains("HIKE")
                                || DocumentName.ToUpper().Contains("APPRAISAL"))
                            {
                                var employeeIncrement = db.EmployeeIncrements.Single(ei => ei.UserId ==
                                employee.UserId && ei.HikeMonthYear == MonthYear);

                                if (employeeIncrement.GetType().GetProperty(item.FieldValue) != null)
                                {
                                    value = employeeIncrement.GetType().GetProperty(item.FieldValue).GetValue(employeeIncrement, null).ToString();
                                }
                            }
                            else
                            {
                                DateTime monthYear = Convert.ToDateTime(MonthYear);

                                var employeeIncrement = db.EmployeeIncrements.Where(ei => ei.UserId ==
                                employee.UserId && monthYear >= ei.SalaryEffectiveFrom).
                                ToList().OrderByDescending(l => l.SalaryEffectiveFrom).Take(1).First();

                                if (employeeIncrement.GetType().GetProperty(item.FieldValue) != null)
                                {
                                    //value = employeeIncrement.GetType().GetProperty(item.FieldValue).GetValue(employeeIncrement, null).ToString();
                                    if (item.FieldValue == "HikeMonthYear")
                                    {
                                        value = MonthYear;
                                    }
                                    else
                                    {
                                        value = employeeIncrement.GetType().GetProperty(item.FieldValue).GetValue(employeeIncrement, null).ToString();
                                    }
                                }
                            }
                        }

                        if (!documentDirectValues.ContainsKey(item.FieldName))
                        {
                            documentDirectValues.Add(item.FieldName, new DocumentFormatValue()
                            {
                                FieldName = item.FieldName,
                                FieldValue = value,
                                FieldFormat = item.FieldFormat
                            });
                        }

                        //FindAndReplace(wordApp, item.FieldName, value);
                    }
                    else if (item.ValueType.Equals("Fixed Value"))
                    {
                        if (!documentDirectValues.ContainsKey(item.FieldName))
                        {
                            documentDirectValues.Add(item.FieldName, new DocumentFormatValue()
                            {
                                FieldName = item.FieldName,
                                FieldValue = item.FieldValue,
                                FieldFormat = item.FieldFormat
                            });
                        }
                    }
                }
                foreach (CompanyDocumentConfigurableValue item in documentValues)
                {
                    if (item.ValueType.Equals("Calculated value"))
                    {
                        if (!item.FieldValue.ToUpper().Contains("TOTALDAYSFROMMONTH"))
                        {
                            var words = string.Join("|", documentDirectValues.Keys);
                            var value = Regex.Replace(item.FieldValue, $@"\b({words})\b", delegate (Match m)
                            {
                                return documentDirectValues[m.Value].FieldValue;
                            });

                            MSScriptControl.ScriptControl sc = new MSScriptControl.ScriptControl();
                            sc.Language = "VBScript";
                            decimal result = (decimal)sc.Eval(value);

                            if (!documentDirectValues.ContainsKey(item.FieldName))
                            {
                                documentDirectValues.Add(item.FieldName,
                                    new DocumentFormatValue()
                                    {
                                        FieldName = item.FieldName,
                                        FieldValue = string.Format("{0:0.00}", result),
                                        FieldFormat = item.FieldFormat
                                    });
                            }
                        }
                        else
                        {
                            var paymentMonth = Convert.ToDateTime(MonthYear);
                            var numberOfDays = DateTime.DaysInMonth(paymentMonth.Year, paymentMonth.Month);
                            documentDirectValues.Add(item.FieldName,
                                new DocumentFormatValue()
                                {
                                    FieldName = item.FieldName,
                                    FieldValue = numberOfDays.ToString(),
                                    FieldFormat = item.FieldFormat
                                });
                        }
                    }
                }

                foreach (KeyValuePair<string, DocumentFormatValue> item in documentDirectValues)
                {
                    DateTime dDate;
                    double doubleValue;

                    if (DateTime.TryParse(item.Value.FieldValue, out dDate))
                    {
                        string value = item.Value.FieldValue;

                        if (!string.IsNullOrEmpty(item.Value.FieldFormat))
                        {
                            string[] fieldFormat = item.Value.FieldFormat.Split(new char[] { ',' });
                            foreach (string format in fieldFormat)
                            {
                                if (format.ToUpper() == "INCAPITAL")
                                {
                                    value = value.ToUpper();
                                }
                                else
                                {
                                    value = dDate.ToString(format);
                                }
                            }
                        }
                        else
                        {
                            value = String.Format("{0:dd MMMM yyyy}", dDate);
                        }

                        FindAndReplace(wordApp, item.Key, value);
                    }
                    else if (double.TryParse(item.Value.FieldValue, out doubleValue))
                    {
                        if (!string.IsNullOrEmpty(item.Value.FieldFormat))
                        {
                            string value = doubleValue.ToString();
                            string[] fieldFormat = item.Value.FieldFormat.Split(new char[] { ',' });
                            foreach (string format in fieldFormat)
                            {
                                if (format.ToUpper() == "INWORDS")
                                {
                                    value = NumberToWords.ConvertAmount(doubleValue);
                                }
                                if (format.ToUpper() == "AMOUNTWITHCOMMAS")
                                {
                                    value = String.Format(new CultureInfo("en-IN", false), "{0:n}", doubleValue);
                                }
                                if (format.ToUpper() == "NODIGITAFTERDECIMALPOINT")
                                {
                                    value = String.Format(new CultureInfo("en-IN", false), "{0:n0}", doubleValue);
                                }
                            }
                            FindAndReplace(wordApp, item.Key, value);
                        }
                        else
                        {
                            FindAndReplace(wordApp, item.Key, doubleValue);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(item.Value.FieldFormat))
                        {
                            string[] fieldFormat = item.Value.FieldFormat.Split(new char[] { ',' });
                            foreach (string format in fieldFormat)
                            {
                                if (format.ToUpper() == "INCAPITAL")
                                {
                                    item.Value.FieldValue = item.Value.FieldValue.ToUpper();
                                }
                            }
                        }

                        FindAndReplace(wordApp, item.Key, item.Value.FieldValue);
                    }
                }

                myWordDoc.SaveAs2(ref SaveAs, ref missing, ref missing, ref missing,
                                                        ref missing, ref missing, ref missing,
                                                        ref missing, ref missing, ref missing, ref missing, ref missing, ref missing,
                                                        ref missing, ref missing, ref missing);

                myWordDoc.Close();
                wordApp.Quit();

                // System.IO.File.Delete(SaveAs.ToString());
            }
        }

        public PartialViewResult ViewDocumentInBrowser(string DocumentName, string MonthYear)
        {
            string loggedInUserId = User.Identity.GetUserId();

            if (Session["userId"] != null)
            {
                loggedInUserId = Session["userId"].ToString();
            }

            var data = db.Employees.Where(e => e.UserId.Equals(loggedInUserId)).Join(db.CompanyDocuments,
                 e => e.CompanyId,
                 c => c.CompanyId,
                 (e1, c1) => new { employee = e1, companyDocument = c1 }).
                 FirstOrDefault(cd => cd.companyDocument.DocumentName.Equals(DocumentName)); ;

            var document = data.companyDocument;

            string companyName = db.Companies.FirstOrDefault(c => c.CompanyId == document.CompanyId).CompanyName;

            string documentFolderLocation = Server.MapPath("~/documents/" + companyName + "/");

            string tempFilName = Path.Combine(documentFolderLocation, document.DocumentName + "." + document.DocumentType);

            //CreateWordDocument(Server.MapPath(document.DocumentPath), tempFilName, data.employee, data.companyDocument.Id, MonthYear, DocumentName);

            string fileName = tempFilName;

            var wordApp = new Microsoft.Office.Interop.Word.Application();
            var wordDocument = wordApp.Documents.Open(fileName, ReadOnly: true);

            wordDocument.ExportAsFixedFormat(Path.Combine(documentFolderLocation, document.DocumentName + ".pdf"),
                Microsoft.Office.Interop.Word.WdExportFormat.wdExportFormatPDF);

            wordDocument.Close(Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges,
                               Microsoft.Office.Interop.Word.WdOriginalFormat.wdOriginalDocumentFormat,
                               false); //Close document

            wordApp.Quit();

            ViewBag.FilePath = "../../documents/" + companyName + "/" + document.DocumentName + ".pdf";

            return PartialView("_PdfViewer");

            //using (Viewer viewer = new Viewer(fileName))
            //{
            //    PdfViewOptions options = new PdfViewOptions(outputFilePath);
            //    viewer.View(options);
            //}

            //var fileStream = new FileStream(outputFilePath,
            //    FileMode.Open,
            //    FileAccess.Read);

            //var fsResult = new FileStreamResult(fileStream, "application/pdf");

            //return fsResult;

            //string FileName = "SampleOfferFormat2143.docx";
            //object documentFormat = 8;
            //string randomName = DateTime.Now.Ticks.ToString();
            //object htmlFilePath = Server.MapPath("~/documents/Square Vision Technologies/") + randomName + ".htm";
            //object fileSavePath = Server.MapPath("~/documents/Square Vision Technologies/") + Path.GetFileName(FileName);
            //_Application applicationclass = new Application();
            //applicationclass.Documents.Open(ref fileSavePath);
            //applicationclass.Visible = false;
            //Document document = applicationclass.ActiveDocument;
            //document.SaveAs(ref htmlFilePath, ref documentFormat);
            //document.Close();
            //string wordHTML = System.IO.File.ReadAllText(htmlFilePath.ToString());
            //return Content(wordHTML);
        }

        public ActionResult ConfigureDocumentValues(int? CompanyId, int? CompanyDocumentId)
        {
            ViewBag.CompanyIdList = new SelectList
                (db.Companies, "CompanyId", "CompanyName");

            ViewBag.Columns = new SqlDataHelper().GetEmployeeColumnNames().ToArray();
            //ViewBag.Columns = new SqlDataHelper().GetEmployeeColumnNames().Select(
            //    e => new SelectListItem() { Text = e, Value = e });

            //var query = from c in db.vw_SysColumns
            //            where c.TABLE_NAME == "Customers" && c.TABLE_SCHEMA == "dbo" && c.TABLE_CATALOG == "Northwind"
            //            select c;

            //foreach (var item in query)
            //{
            //    Response.Write(String.Format("ColumnName: {0}<br/>", item.COLUMN_NAME));
            //}

            var documentValues = db.CompanyDocumentConfigurableValues.Where(c => c.CompanyId == CompanyId &&
            c.CompanyDocumentId == CompanyDocumentId);

            ViewBag.CompanyId = CompanyId;
            ViewBag.CompanyDocumentId = CompanyDocumentId;

            return View(documentValues);
        }

        [HttpPost]
        public JsonResult ConfigureDocumentValues(List<CompanyDocumentConfigurableValue> configurableValues)
        {
            if (configurableValues == null)
            {
                configurableValues = new List<CompanyDocumentConfigurableValue>();
            }

            int counter = 0;

            foreach (CompanyDocumentConfigurableValue configurationValue in configurableValues)
            {
                counter++;

                configurationValue.AddedDate = DateTime.Now;

                var existingConfigurationValue = db.CompanyDocumentConfigurableValues.Find(configurationValue.Id);

                if (existingConfigurationValue != null && existingConfigurationValue.Id > 0)
                {
                    if (!configurationValue.FieldName.Equals("deleted"))
                    {
                        //db.Entry(configurationValue).State = EntityState.Modified;
                        existingConfigurationValue.CompanyDocumentId = configurationValue.CompanyDocumentId;
                        existingConfigurationValue.CompanyId = configurationValue.CompanyId;
                        existingConfigurationValue.FieldName = configurationValue.FieldName;
                        existingConfigurationValue.ValueType = configurationValue.ValueType;
                        existingConfigurationValue.FieldValue = configurationValue.FieldValue;
                        existingConfigurationValue.FieldFormat = configurationValue.FieldFormat;
                        existingConfigurationValue.AddedDate = DateTime.Now;
                    }
                    else
                    {
                        db.CompanyDocumentConfigurableValues.Remove(existingConfigurationValue);
                    }
                    db.SaveChanges();
                }
                else
                {
                    db.CompanyDocumentConfigurableValues.Add(configurationValue);
                    db.SaveChanges();
                }
            }

            return Json(counter);
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
