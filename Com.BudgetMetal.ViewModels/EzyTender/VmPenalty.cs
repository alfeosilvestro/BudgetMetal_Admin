using System;
using System.Collections.Generic;

namespace Com.BudgetMetal.ViewModels.EzyTender
{
    public class VmPenalty : ViewModelItemBase
    {
       
        public uint RfqId { get; set; }
        public string BreachOfServiceDefinition { get; set; }
        public string PenaltyAmount { get; set; }
        public string Description { get; set; }

        public VmRfq Rfq { get; set; }
    }
}
