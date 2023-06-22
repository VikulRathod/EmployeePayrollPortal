using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmployeeManagement.Models
{
    [MetadataType(typeof(CompanyDocumentConfigurableValueMetadata))]
    public partial class CompanyDocumentConfigurableValue
    {

    }

    public class CompanyDocumentConfigurableValueMetadata
    {
        public int Id { get; set; }

        [DisplayName("Company Name")]
        public Nullable<int> CompanyId { get; set; }

        [DisplayName("Document Field Name")]
        public string FieldName { get; set; }

        [DisplayName("Document Field Type")]
        public string ValueType { get; set; }

        [DisplayName("Document Field Value")]
        public string FieldValue { get; set; }

        public Nullable<System.DateTime> AddedDate { get; set; }

        public Nullable<int> CompanyDocumentId { get; set; }

        public string FieldFormat { get; set; }
    }
}