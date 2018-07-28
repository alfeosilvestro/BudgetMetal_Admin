using System;
using System.Collections.Generic;

namespace Com.BudgetMetal.ViewModels.EzyTender
{
    public class VmQuotation
    {
        //public Quotation()
        //{
        //    QuotationPriceSchedule = new HashSet<QuotationPriceSchedule>();
        //}

        public uint Id { get; set; }
        public uint DocumentId { get; set; }
        public uint RfqId { get; set; }
        public decimal? QuotedFigure { get; set; }
        public DateTime? ValidToDate { get; set; }
        public string Comments { get; set; }

        public VmDocument Document { get; set; }
        public VmRfq Rfq { get; set; }
        public ICollection<VmQuotationPriceSchedule> QuotationPriceSchedule { get; set; }
    }
}
