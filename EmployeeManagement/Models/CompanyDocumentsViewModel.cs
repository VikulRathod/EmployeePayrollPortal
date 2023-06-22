using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmployeeManagement.Models
{
    [MetadataType(typeof(CompanyDocumentMetadata))]
    public partial class CompanyDocument
    {
        [DisplayName("Upload File")]
        public HttpPostedFileBase DocumentFile { get; set; }
    }

    public class CompanyDocumentMetadata
    {
        public int Id { get; set; }

        [DisplayName("Document Type")]
        public string DocumentName { get; set; }
        public byte[] DocumentPath { get; set; }

        [DisplayName("Created By")]
        public string CreatedBy { get; set; }

        [DisplayName("Created Date")]
        public Nullable<System.DateTime> CreatedDate { get; set; }

        public Nullable<int> CompanyId { get; set; }

        public string DocumentType { get; set; }
    }
}