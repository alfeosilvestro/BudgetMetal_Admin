using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.BudgetMetal.DBEntities
{
    public class Quotation
    {
        //public Quotation()
        //{
        //    QuotationPriceSchedule = new HashSet<QuotationPriceSchedule>();
        //}

        public int Id { get; set; }
       
        public decimal? QuotedFigure { get; set; }
        public DateTime? ValidToDate { get; set; }
        public string Comments { get; set; }

        [ForeignKey("Document")]
        public int Document_Id { get; set; }
        public virtual Document Document { get; set; }

        [ForeignKey("Rfq")]
        public int Rfq_Id { get; set; }
        public virtual Rfq Rfq { get; set; }
       // public ICollection<QuotationPriceSchedule> QuotationPriceSchedule { get; set; }
    }
}
