using System;
using System.Collections.Generic;
using System.Text;
using Com.BudgetMetal.ViewModels.EzyTender;

namespace Com.BudgetMetal.Services.RFQ
{
    public interface IRFQService
    {
        int SaveRFQ(VmRfq rfq);
    }
}
