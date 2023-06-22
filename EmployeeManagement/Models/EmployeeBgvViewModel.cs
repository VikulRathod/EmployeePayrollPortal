using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace EmployeeManagement.Models
{
    [MetadataType(typeof(EmployeeBgvViewMetaData))]
    public partial class EmployeeBgvView { }

    public class EmployeeBgvViewMetaData
    {
        [Key]
        public string Id { get; set; }

        [DisplayName("Full Name")]
        public string Full_Name { get; set; }

        [DisplayName("Company Name")]
        public string Company_Name { get; set; }

        [DisplayName("Employee Code")]
        public string Employee_Id { get; set; }

        public string Mobile { get; set; }

        [DisplayName("Email Address")]
        public string Email_Id { get; set; }
        public string Batch { get; set; }
        public string Team { get; set; }

        [DisplayName("Permanent Address")]
        public string Permanent_Address { get; set; }

        [DisplayName("Current Address")]
        public string Current_Address { get; set; }
        public string PAN { get; set; }
        public string Adhaar { get; set; }
        public string UAN { get; set; }

        [DisplayName("Joining Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> Joining_Date { get; set; }

        [DisplayName("Joining Salary")]
        public Nullable<decimal> Joining_Salary { get; set; }

        [DisplayName("Joining Designation")]
        public string Joining_Designation { get; set; }

        [DisplayName("Current Salary")]
        public Nullable<decimal> Current_Salary { get; set; }

        [DisplayName("Current Designation")]
        public string Current_Designation { get; set; }

        [DisplayName("Resignation Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> Resignation_Date { get; set; }

        [DisplayName("Last Working Day")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> Last_Working_Day { get; set; }
    }
}