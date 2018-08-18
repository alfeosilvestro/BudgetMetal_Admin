using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.ViewModels.RfqPenalty
{
    public class VmPenaltyItem : ViewModelItemBase
    {
       

        public string BreachOfServiceDefinition { get; set; }
        public string PenaltyAmount { get; set; }
        public string Description { get; set; }


        public int Rfq_Id { get; set; }

        
    }
}
