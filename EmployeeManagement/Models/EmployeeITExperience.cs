//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EmployeeManagement.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class EmployeeITExperience
    {
        public int ExperienceId { get; set; }
        public string UserId { get; set; }
        public Nullable<int> BatchId { get; set; }
        public Nullable<int> TeamId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectTechnologies { get; set; }
        public string ProjectDescription { get; set; }
        public Nullable<System.DateTime> JoiningDate { get; set; }
        public Nullable<decimal> JoiningSalary { get; set; }
        public string JoiningDesignation { get; set; }
        public Nullable<decimal> CurrentSalary { get; set; }
        public string CurrentDesignation { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public Nullable<int> BankAccountId { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual Batch Batch { get; set; }
        public virtual Team Team { get; set; }
        public virtual EmployeeBankAccount EmployeeBankAccount { get; set; }
    }
}