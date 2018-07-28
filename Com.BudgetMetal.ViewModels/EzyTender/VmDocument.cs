using System;
using System.Collections.Generic;

namespace Com.BudgetMetal.ViewModels.EzyTender
{
    public class VmDocument : ViewModelItemBase
    {
        //public Document()
        //{
        //    Attachment = new HashSet<Attachment>();
        //    Clarification = new HashSet<Clarification>();
        //    DocumentUser = new HashSet<DocumentUser>();
        //    Quotation = new HashSet<Quotation>();
        //    Rating = new HashSet<Rating>();
        //    Rfq = new HashSet<Rfq>();
        //}
        
        public string Title { get; set; }
        public uint DocumentTypeId { get; set; }
        public string ContactPersonName { get; set; }
        public string DocumentNo { get; set; }
        public uint CompanyId { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public uint DocumentStatusId { get; set; }

        public VmCompany Company { get; set; }
        public VmCodeTable DocumentStatus { get; set; }
        public VmCodeTable DocumentType { get; set; }
        public ICollection<VmAttachment> Attachment { get; set; }
        public ICollection<VmClarification> Clarification { get; set; }
        public ICollection<VmDocumentUser> DocumentUser { get; set; }
        public ICollection<VmQuotation> Quotation { get; set; }
        public ICollection<VmRating> Rating { get; set; }
        public ICollection<VmRfq> Rfq { get; set; }
    }
}
