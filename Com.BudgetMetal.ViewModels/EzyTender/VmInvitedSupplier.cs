using System;
using System.Collections.Generic;

namespace Com.BudgetMetal.ViewModels.EzyTender
{
    public class VmInvitedSupplier : ViewModelItemBase
    {
        
        public int RfqId { get; set; }
        public int Company_Id { get; set; }
        

        public VmCompany Company { get; set; }
        public VmRfq Rfq { get; set; }
    }
}
