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

        public string ContactPersonName { get; set; }
        public string DocumentNo { get; set; }
        public DateTime? SubmissionDate { get; set; }


        
        public int Company_Id { get; set; }
        public virtual VmCompany Company { get; set; }

       
        public int DocumentStatus_Id { get; set; }
        public virtual VmCodeTable DocumentStatus { get; set; }


        public int DocumentType_Id { get; set; }
        public virtual VmCodeTable DocumentType { get; set; }

        public ICollection<VmAttachment> Attachment { get; set; }
        //public ICollection<Clarification> Clarification { get; set; }
        //public ICollection<DocumentUser> DocumentUser { get; set; }
        //public ICollection<Quotation> Quotation { get; set; }
        //public ICollection<Rating> Rating { get; set; }
        //public ICollection<Rfq> Rfq { get; set; }
    }
}
