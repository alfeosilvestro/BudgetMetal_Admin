using Com.BudgetMetal.ViewModels.Document;
using Com.BudgetMetal.ViewModels.DocumentActivity;
using Com.BudgetMetal.ViewModels.InvitedSupplier;
using Com.BudgetMetal.ViewModels.Quotation;
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

        [Required]
        public string InternalProjectName { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dddd dd MMMM yyyy}")]
        public DateTime? StartRfqdate { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dddd dd MMMM yyyy}")]
        public DateTime? ValidRfqdate { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dddd dd MMMM yyyy}")]
        public DateTime? EstimatedProjectStartDate { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dddd dd MMMM yyyy}")]
        public DateTime? EstimatedProjectEndDate { get; set; }

        public bool SupplierProvideMaterial { get; set; }
        public bool SupplierProvideTransport { get; set; }
        public string MessageToSupplier { get; set; }
        public string IndustryOfRfq { get; set; }
        public string SelectedTags { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dddd dd MMMM yyyy}")]
        public DateTime? QuotationOpeningDate { get; set; }
        public string ContractValue { get; set; }
        public bool IsPublic { get; set; }
        public bool SupplierProvideInstallationService { get; set; }

        public virtual VmDocumentItem Document { get; set; }
        public virtual List<VmInvitedSupplierItem> InvitedSupplier { get; set; }
        public virtual List<VmPenaltyItem> Penalty { get; set; }
        public virtual List<VmRequirementItem> Requirement { get; set; }
        public virtual List<VmRfqPriceScheduleItem> RfqPriceSchedule { get; set; }
        public virtual List<VmSlaItem> Sla { get; set; }
        
        public List<VmDocumentItem> DocumentList { get; set; }
        //public List<VmDocumentActivityItem> DocumentActivityList { get; set; }
        public virtual List<VmQuotationItem> Quotation { get; set; }

        public List<List<string>> RequirementComparison { get; set; }
        public List<List<string>> PriceComparison { get; set; }
        //public List<VmDocumentActivityItem> DocumentActivityList { get; set; }
    }
}
