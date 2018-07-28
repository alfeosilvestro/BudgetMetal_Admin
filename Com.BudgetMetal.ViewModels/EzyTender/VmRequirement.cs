using System;
using System.Collections.Generic;

namespace Com.BudgetMetal.ViewModels.EzyTender
{
    public class VmRequirement : ViewModelItemBase
    {
       
        public uint RfqId { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public int? Quantity { get; set; }

        public VmRfq Rfq { get; set; }
    }
}
