using System;
using System.Collections.Generic;

namespace Com.BudgetMetal.ViewModels.EzyTender
{
    public class VmRfq
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

        public uint Id { get; set; }
        public uint DocumentId { get; set; }
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

        public VmDocument Document { get; set; }
        public ICollection<VmInvitedSupplier> InvitedSupplier { get; set; }
        public ICollection<VmPenalty> Penalty { get; set; }
        public ICollection<VmQuotation> Quotation { get; set; }
        public ICollection<VmRequirement> Requirement { get; set; }
        public ICollection<VmRfqPriceSchedule> RfqPriceSchedule { get; set; }
        public ICollection<VmSla> Sla { get; set; }
    }
}
