using System;
using System.Collections.Generic;
using Com.BudgetMetal.ViewModels.Company;
using Com.BudgetMetal.ViewModels.Rfq;

namespace Com.BudgetMetal.ViewModels.InvitedSupplier
{
    public class VmInvitedSupplierItem : ViewModelItemBase
    {
        
        public int RfqId { get; set; }
        public int Company_Id { get; set; }
        

        public virtual VmCompanyItem Company { get; set; }
        public virtual VmRfqItem Rfq { get; set; }
    }
}
