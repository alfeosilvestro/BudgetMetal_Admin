using System;
using System.Collections.Generic;

namespace Com.BudgetMetal.ViewModels.EzyTender
{
    public class VmSla : ViewModelItemBase
    {
      
        public int Rfq_Id { get; set; }
        public string Requirement { get; set; }
        public string Description { get; set; }

        public VmRfq Rfq { get; set; }
    }
}
