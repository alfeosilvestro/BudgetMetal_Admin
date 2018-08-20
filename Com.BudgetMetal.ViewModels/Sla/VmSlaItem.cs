using System;
using System.Collections.Generic;
using Com.BudgetMetal.ViewModels.Rfq;

namespace Com.BudgetMetal.ViewModels.Sla
{
    public class VmSlaItem : ViewModelItemBase
    {
      
        public int Rfq_Id { get; set; }
        public string Requirement { get; set; }
        public string Description { get; set; }

        public virtual VmRfqItem Rfq { get; set; }
    }
}
