using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models
{
    [MetadataType(typeof(EmployeeIncrementMetadata))]
    public partial class EmployeeIncrement
    {
    }

    public class EmployeeIncrementMetadata
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        [DisplayName("Hike Date")]
        public string HikeMonthYear { get; set; }

        [DisplayName("Hike In Percent")]
        public Nullable<decimal> HikeInPercentage { get; set; }

        [DisplayName("Hike Amount")]
        public Nullable<decimal> HikeAmount { get; set; }

        [DisplayName("Salary After Hike")]
        public Nullable<decimal> SalaryAfterHike { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [DisplayName("Salary Effective From")]
        public Nullable<System.DateTime> SalaryEffectiveFrom { get; set; }

        public Nullable<System.DateTime> ModifiedDate { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }
    }
}