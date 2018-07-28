using System;
using System.Collections.Generic;

namespace Com.BudgetMetal.ViewModels.EzyTender
{
    public class VmSla : ViewModelItemBase
    {
      
        public uint RfqId { get; set; }
        public string Requirement { get; set; }
        public string Description { get; set; }

        public VmRfq Rfq { get; set; }
    }
}
