using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.BudgetMetal.DBEntities
{
    public class QuotationPriceSchedule
    {
        public int Id { get; set; }
        public int Document_Id { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public string InternalRefrenceCode { get; set; }
        public string QuantityRequired { get; set; }
        public decimal ItemAmount { get; set; }
        public string Version { get; set; }

        [ForeignKey("Quotation")]
        public int Quotation_Id { get; set; }
        public  virtual Quotation Quotation { get; set; }
    }
}
