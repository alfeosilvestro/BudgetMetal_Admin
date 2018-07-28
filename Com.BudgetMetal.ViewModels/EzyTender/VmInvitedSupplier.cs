using System;
using System.Collections.Generic;

namespace Com.BudgetMetal.ViewModels.EzyTender
{
    public class VmInvitedSupplier : ViewModelItemBase
    {
        
        public uint RfqId { get; set; }
        public uint CompanyId { get; set; }
        

        public VmCompany Company { get; set; }
        public VmRfq Rfq { get; set; }
    }
}
