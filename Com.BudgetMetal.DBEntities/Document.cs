using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.BudgetMetal.DBEntities
{
    public class Document : GenericEntity
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

        [ForeignKey("Company_Id")]
        public virtual Company Company { get; set; }

        
        public int DocumentStatus_Id { get; set; }

        [ForeignKey("DocumentStatus_Id")]
        public virtual CodeTable DocumentStatus { get; set; }

       
        public int DocumentType_Id { get; set; }

        [ForeignKey("DocumentType_Id")]
        public virtual CodeTable DocumentType { get; set; }

        //public ICollection<Attachment> Attachment { get; set; }
        //public ICollection<Clarification> Clarification { get; set; }
        //public ICollection<DocumentUser> DocumentUser { get; set; }
        //public ICollection<Quotation> Quotation { get; set; }
        //public ICollection<Rating> Rating { get; set; }
        //public ICollection<Rfq> Rfq { get; set; }
    }
}
