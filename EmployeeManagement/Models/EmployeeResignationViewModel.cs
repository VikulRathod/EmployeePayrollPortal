﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models
{
    [MetadataType(typeof(EmployeeResignationMetadata))]
    public partial class EmployeeResignation
    {
    }

    public class EmployeeResignationMetadata
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        [DisplayName("Company Offers")]
        public string CompanyOffers { get; set; }

        [DisplayName("Resignation Date")]
        public Nullable<System.DateTime> ResignationDate { get; set; }

        [DisplayName("Last Working Day")]
        public Nullable<System.DateTime> LastWorkingDay { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }
    }
}