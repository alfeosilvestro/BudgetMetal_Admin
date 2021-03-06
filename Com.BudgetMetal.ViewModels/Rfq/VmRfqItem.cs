﻿using Com.BudgetMetal.ViewModels.Document;
using Com.BudgetMetal.ViewModels.DocumentActivity;
using Com.BudgetMetal.ViewModels.InvitedSupplier;
using Com.BudgetMetal.ViewModels.Quotation;
using Com.BudgetMetal.ViewModels.Requirement;
using Com.BudgetMetal.ViewModels.RfqInvites;
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

       
        public string InternalRefrenceNo { get; set; }

        public string InternalProjectName { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MMM.yyyy}")]
        public DateTime? StartRfqdate { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MMM.yyyy}")]
        public DateTime? ValidRfqdate { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MMMM.yyyy}")]
        public DateTime? EstimatedProjectStartDate { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MMM.yyyy}")]
        public DateTime? EstimatedProjectEndDate { get; set; }

        public bool SupplierProvideMaterial { get; set; }
        public bool SupplierProvideTransport { get; set; }
        public string MessageToSupplier { get; set; }
        public string IndustryOfRfq { get; set; }
        public string SelectedTags { get; set; }

        
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MMM.yyyy}")]
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
        public virtual List<VmRfqInvitesItem> RfqEmailInvites { get; set; }

        public List<List<string>> RequirementComparison { get; set; }
        public List<List<string>> SupportComparison { get; set; }
        public List<List<string>> CommercialComparison { get; set; }
        public List<List<string>> ProductPriceComparison { get; set; }
        public List<List<string>> ServicePriceComparison { get; set; }
        public List<List<string>> WarrantyPriceComparison { get; set; }
        public List<List<string>> TotalPriceComparison { get; set; }
        public List<List<string>> SummaryComparison { get; set; }
        //public List<VmDocumentActivityItem> DocumentActivityList { get; set; }
    }
}
