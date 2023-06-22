using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models
{
    [MetadataType(typeof(EmployeeInterviewMetadata))]
    public partial class EmployeeInterview
    {
    }

    public class EmployeeInterviewMetadata
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        [DisplayName("Company Name")]
        public string CompanyName { get; set; }

        [DisplayName("Interview Date")]
        public Nullable<System.DateTime> InterviewDate { get; set; }

        [DisplayName("Interviewer Name")]
        public string InterviewerName { get; set; }

        [DisplayName("Interview Round")]
        public string InterviewRound { get; set; }

        [DisplayName("Interview Questions")]
        public string InterviewQuestions { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }
    }
}