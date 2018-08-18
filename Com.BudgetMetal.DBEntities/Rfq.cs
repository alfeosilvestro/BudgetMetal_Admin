using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.BudgetMetal.DBEntities
{
    [Table("RFQ")]
    public class Rfq : GenericEntity
    {
              
        public string InternalRefrenceNo { get; set; }
        public string InternalProjectName { get; set; }
        public DateTime? StartRfqdate { get; set; }
        public DateTime? ValidRfqdate { get; set; }
        public DateTime? EstimatedProjectStartDate { get; set; }
        public DateTime? EstimatedProjectEndDate { get; set; }
        public bool SupplierProvideMaterial { get; set; }
        public bool SupplierProvideTransport { get; set; }
        public string MessageToSupplier { get; set; }
        public string IndustryOfRfq { get; set; }
        public string SelectedTags { get; set; }

        public int Document_Id { get; set; }

        [ForeignKey("Document_Id")]
        public virtual Document Document { get; set; }

        public virtual ICollection<InvitedSupplier> InvitedSupplier { get; set; }
        public virtual ICollection<Penalty> Penalty { get; set; }
        public virtual ICollection<Quotation> Quotation { get; set; }
        public virtual ICollection<Requirement> Requirement { get; set; }
        public virtual ICollection<RfqPriceSchedule> RfqPriceSchedule { get; set; }
        public virtual ICollection<Sla> Sla { get; set; }
    }
}
