using System;
using System.Collections.Generic;

namespace Com.BudgetMetal.DB.Entities
{
    public class Document : GenericEntity
    {
        public Document()
        {
            Attachment = new HashSet<Attachment>();
            Clarification = new HashSet<Clarification>();
            DocumentUser = new HashSet<DocumentUser>();
            Quotation = new HashSet<Quotation>();
            Rating = new HashSet<Rating>();
            Rfq = new HashSet<Rfq>();
        }
        
        public string Title { get; set; }
        public uint DocumentTypeId { get; set; }
        public string ContactPersonName { get; set; }
        public string DocumentNo { get; set; }
        public uint CompanyId { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public uint DocumentStatusId { get; set; }

        public Company Company { get; set; }
        public CodeTable DocumentStatus { get; set; }
        public CodeTable DocumentType { get; set; }
        public ICollection<Attachment> Attachment { get; set; }
        public ICollection<Clarification> Clarification { get; set; }
        public ICollection<DocumentUser> DocumentUser { get; set; }
        public ICollection<Quotation> Quotation { get; set; }
        public ICollection<Rating> Rating { get; set; }
        public ICollection<Rfq> Rfq { get; set; }
    }
}
