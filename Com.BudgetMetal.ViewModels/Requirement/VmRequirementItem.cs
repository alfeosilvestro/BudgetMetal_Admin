using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Com.BudgetMetal.ViewModels.Rfq;

namespace Com.BudgetMetal.ViewModels.Requirement
{
    public class VmRequirementItem : ViewModelItemBase
    {       
        public int Rfq_Id { get; set; }

        
        public string ServiceName { get; set; }

        
        public string Description { get; set; }

        public virtual VmRfqItem Rfq { get; set; }
    }
}
