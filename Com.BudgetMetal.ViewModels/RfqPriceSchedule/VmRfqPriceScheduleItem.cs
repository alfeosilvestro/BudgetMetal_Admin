using Com.BudgetMetal.ViewModels.Rfq;
using System;
using System.Collections.Generic;

namespace Com.BudgetMetal.ViewModels.RfqPriceSchedule
{
    public class VmRfqPriceScheduleItem
    {
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public string InternalRefrenceCode { get; set; }
        public string QuantityRequired { get; set; }
        public int CategoryId { get; set; }

        public int Rfq_Id { get; set; }

       
        public virtual VmRfqItem Rfq { get; set; }
    }
}
