using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.BudgetMetal.DBEntities
{
    public class QuotationPriceSchedule : GenericEntity
    {
              
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public string InternalRefrenceCode { get; set; }
        public string QuantityRequired { get; set; }
        public decimal ItemAmount { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quotation_Id { get; set; }
        public int CategoryId { get; set; }

        [ForeignKey("Quotation_Id")]
        public virtual Quotation Quotation { get; set; }
    }
}
