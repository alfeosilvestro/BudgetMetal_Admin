using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.BudgetMetal.DBEntities
{
    public class Rfq
    {
        //public Rfq()
        //{
        //    InvitedSupplier = new HashSet<InvitedSupplier>();
        //    Penalty = new HashSet<Penalty>();
        //    Quotation = new HashSet<Quotation>();
        //    Requirement = new HashSet<Requirement>();
        //    RfqPriceSchedule = new HashSet<RfqPriceSchedule>();
        //    Sla = new HashSet<Sla>();
        //}

        public int Id { get; set; }
        
        public string InternalRefrenceNo { get; set; }
        public string InternalProjectName { get; set; }
        public DateTime? StartRfqdate { get; set; }
        public DateTime? ValidRfqdate { get; set; }
        public DateTime? EstimatedProjectStartDate { get; set; }
        public DateTime? EstimatedProjectEndDate { get; set; }
        public bool? SupplierProvideMaterial { get; set; }
        public bool? SupplierProvideTransport { get; set; }
        public string MessageToSupplier { get; set; }
        public string IndustryOfRfq { get; set; }
        public string SelectedTags { get; set; }

        [ForeignKey("Document")]
        public int Document_Id { get; set; }
        public virtual Document Document { get; set; }
        //public ICollection<InvitedSupplier> InvitedSupplier { get; set; }
        //public ICollection<Penalty> Penalty { get; set; }
        //public ICollection<Quotation> Quotation { get; set; }
        //public ICollection<Requirement> Requirement { get; set; }
        //public ICollection<RfqPriceSchedule> RfqPriceSchedule { get; set; }
        //public ICollection<Sla> Sla { get; set; }
    }
}
