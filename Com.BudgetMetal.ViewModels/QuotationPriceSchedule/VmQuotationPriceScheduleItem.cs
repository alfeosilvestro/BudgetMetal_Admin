using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.BudgetMetal.ViewModels.QuotationPriceSchedule
{
    public class VmQuotationPriceScheduleItem : ViewModelItemBase
    {
              
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public string InternalRefrenceCode { get; set; }
        public string QuantityRequired { get; set; }
        public decimal ItemAmount { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quotation_Id { get; set; }
       
    }
}
