using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models
{
    [MetadataType(typeof(EmployeeEducationMetadata))]
    public partial class EmployeeEducation
    {
    }

    public class EmployeeEducationMetadata
    {
        public int EducationId { get; set; }
        public string UserId { get; set; }

        [DisplayName("Education Type")]
        public Nullable<int> EducationTypeId { get; set; }

        [DisplayName("College Name")]
        public string CollegeName { get; set; }

        [DisplayName("Percentage")]
        public Nullable<decimal> Percentage { get; set; }

        [DisplayName("Passout Year")]
        public Nullable<int> PassoutYear { get; set; }

        [DisplayName("Specialization")]
        public string Specialization { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }
        public virtual EmployeeEducationType EmployeeEducationType { get; set; }
    }
}