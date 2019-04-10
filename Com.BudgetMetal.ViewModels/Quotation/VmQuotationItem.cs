using Com.BudgetMetal.ViewModels.Document;
using Com.BudgetMetal.ViewModels.DocumentActivity;
using Com.BudgetMetal.ViewModels.QuotationCommercial;
using Com.BudgetMetal.ViewModels.QuotationPriceSchedule;
using Com.BudgetMetal.ViewModels.QuotationRequirement;
using Com.BudgetMetal.ViewModels.QuotationSupport;
using Com.BudgetMetal.ViewModels.Rfq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.BudgetMetal.ViewModels.Quotation
{
    public class VmQuotationItem : ViewModelItemBase
    {
        public decimal? QuotedFigure { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MMMM.yyyy}")]
        public DateTime? ValidToDate { get; set; }

        public string Comments { get; set; }

        public int Document_Id { get; set; }
        public virtual VmDocumentItem Document { get; set; }

        public int Rfq_Id { get; set; }
        public virtual VmRfqItem Rfq { get; set; }

        

        public virtual List<VmQuotationPriceScheduleItem> QuotationPriceSchedule { get; set; }
        public virtual List<VmQuotationSupportItem> QuotationSupport { get; set; }
        public virtual List<VmQuotationCommercialItem> QuotationCommercial { get; set; }

        public virtual List<VmQuotationRequirementItem> QuotationRequirement { get; set; }
        //public List<VmDocumentActivityItem> DocumentActivityList { get; set; }
    }
}
