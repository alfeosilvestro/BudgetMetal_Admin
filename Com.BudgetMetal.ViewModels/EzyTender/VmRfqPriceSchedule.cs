using System;
using System.Collections.Generic;

namespace Com.BudgetMetal.ViewModels.EzyTender
{
    public class VmRfqPriceSchedule
    {
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public string InternalRefrenceCode { get; set; }
        public string QuantityRequired { get; set; }


        public int Rfq_Id { get; set; }

       
        public virtual VmRfq Rfq { get; set; }
    }
}
