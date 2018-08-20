using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Com.BudgetMetal.ViewModels.Rfq;

namespace Com.BudgetMetal.ViewModels.Requirement
{
    public class VmRequirementItem : ViewModelItemBase
    {
       
        public int Rfq_Id { get; set; }

        [Required]
        public string ServiceName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int? Quantity { get; set; }

        public virtual VmRfqItem Rfq { get; set; }
    }
}
