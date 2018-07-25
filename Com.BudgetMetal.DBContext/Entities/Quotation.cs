using System;
using System.Collections.Generic;

namespace Com.BudgetMetal.DB.Entities
{
    public class Quotation
    {
        public Quotation()
        {
            QuotationPriceSchedule = new HashSet<QuotationPriceSchedule>();
        }

        public uint Id { get; set; }
        public uint DocumentId { get; set; }
        public uint RfqId { get; set; }
        public decimal? QuotedFigure { get; set; }
        public DateTime? ValidToDate { get; set; }
        public string Comments { get; set; }

        public Document Document { get; set; }
        public Rfq Rfq { get; set; }
        public ICollection<QuotationPriceSchedule> QuotationPriceSchedule { get; set; }
    }
}
