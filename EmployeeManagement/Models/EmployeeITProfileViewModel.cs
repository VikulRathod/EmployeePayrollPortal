using System;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models
{
    [MetadataType(typeof(EmployeeITProfileMetadata))]
    public partial class EmployeeITProfile
    {

    }

    public class EmployeeITProfileMetadata
    {
        [Key]
        public string Id { get; set; }

        [Display(Name = "Full Name")]
        public string Full_Name { get; set; }

        [Display(Name = "Company Id")]
        public int CompanyId { get; set; }

        [Display(Name = "Company Logo")]
        public byte[] CompanyLogo { get; set; }

        [Display(Name = "Company Name")]
        public string Company_Name { get; set; }

        [Display(Name = "Employee Id")]
        public string Employee_Id { get; set; }

        [Display(Name = "Official Email")]
        public string Official_Email { get; set; }

        [Display(Name = "Email Password")]
        public string Official_Email_Password { get; set; }

        [Display(Name = "Company Address")]
        public string CompanyAddress { get; set; }

        [Display(Name = "Company Employee Strength")]
        public string CompanyEmployeeStrength { get; set; }

        [Display(Name = "Company Founded On")]
        public string CompanyFoundedMonthYear { get; set; }

        [Display(Name = "Company Founded By")]
        public string CompanyFounder { get; set; }

        [Display(Name = "Company Official Contact")]
        public string CompanyOfficialContact { get; set; }

        [Display(Name = "Company Official Email")]
        public string CompanyOfficialEmail { get; set; }

        [Display(Name = "Company Website")]
        public string CompanyWebsite { get; set; }

        [Display(Name = "HR Employee Id")]
        public string HRId { get; set; }

        [Display(Name = "HR Name")]
        public string HRName { get; set; }

        [Display(Name = "HR Mobile")]
        public string HRMobile { get; set; }

        [Display(Name = "HR Official Email")]
        public string HROfficialEmail { get; set; }

        [Display(Name = "Lead/ Supervisor Employee Id")]
        public string LeadId { get; set; }

        [Display(Name = "Lead/ Supervisor Name")]
        public string LeadName { get; set; }

        [Display(Name = "Lead/ Supervisor Mobile")]
        public string LeadMobile { get; set; }

        [Display(Name = "Lead/ Supervisor Email")]
        public string LeadOfficialEmail { get; set; }

        [Display(Name = "Manager Employee Id")]
        public string ManagerId { get; set; }

        [Display(Name = "Manager Name")]
        public string ManagerName { get; set; }

        [Display(Name = "Manager Mobile")]
        public string ManagerMobile { get; set; }

        [Display(Name = "Manager Email")]
        public string ManagerOfficialEmail { get; set; }

        [Display(Name = "Mobile")]
        public string Mobile { get; set; }

        [Display(Name = "Email Id")]
        public string Email_Id { get; set; }

        [Display(Name = "Batch Number")]
        public string Batch { get; set; }

        [Display(Name = "Team Number")]
        public string Team { get; set; }

        [Display(Name = "Permanent Address")]
        public string Permanent_Address { get; set; }

        [Display(Name = "Current Address")]
        public string Current_Address { get; set; }

        [Display(Name = "PAN")]
        public string PAN { get; set; }

        [Display(Name = "Adhaar")]
        public string Adhaar { get; set; }

        [Display(Name = "UAN")]
        public string UAN { get; set; }

        [Display(Name = "Joining Date")]
        public Nullable<System.DateTime> Joining_Date { get; set; }

        [Display(Name = "Joining Salary")]
        public Nullable<decimal> Joining_Salary { get; set; }

        [Display(Name = "Joining Designation")]
        public string Joining_Designation { get; set; }

        [Display(Name = "Current Salary")]
        public Nullable<decimal> Current_Salary { get; set; }

        [Display(Name = "Current Designation")]
        public string Current_Designation { get; set; }
        
        [Display(Name = "Resignation Date")]
        public Nullable<System.DateTime> Resignation_Date { get; set; }

        [Display(Name = "Last Working Day")]
        public Nullable<System.DateTime> Last_Working_Day { get; set; }

        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        [Display(Name = "Project Technologies")]
        public string ProjectTechnologies { get; set; }

        [Display(Name = "Project Description")]
        public string ProjectDescription { get; set; }
    }
}