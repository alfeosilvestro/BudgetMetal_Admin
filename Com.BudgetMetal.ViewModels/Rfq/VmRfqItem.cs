using Com.BudgetMetal.ViewModels.Document;
using Com.BudgetMetal.ViewModels.InvitedSupplier;
using Com.BudgetMetal.ViewModels.Requirement;
using Com.BudgetMetal.ViewModels.RfqPenalty;
using Com.BudgetMetal.ViewModels.RfqPriceSchedule;
using Com.BudgetMetal.ViewModels.Sla;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.BudgetMetal.ViewModels.Rfq
{
    public class VmRfqItem : ViewModelItemBase
    {
       public int Document_Id { get; set; }

        [Required]
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

        public virtual VmDocumentItem Document { get; set; }
        public virtual List<VmInvitedSupplierItem> InvitedSupplier { get; set; }
        public virtual List<VmPenaltyItem> Penalty { get; set; }
        //public virtual ICollection<VmQuotationItem> Quotation { get; set; }
        public virtual List<VmRequirementItem> Requirement { get; set; }
        public virtual List<VmRfqPriceScheduleItem> RfqPriceSchedule { get; set; }
        public virtual List<VmSlaItem> Sla { get; set; }

        public List<VmDocumentItem> DocumentList { get; set; }
    }
}
