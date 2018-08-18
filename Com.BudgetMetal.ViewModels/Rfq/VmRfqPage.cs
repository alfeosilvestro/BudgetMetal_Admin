using Com.BudgetMetal.Common;
using Com.BudgetMetal.ViewModels.EzyTender;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.ViewModels.Rfq
{
    public class VmRfqPage : ViewModelBase
    {
        public PageResult<VmRfq> Result { get; set; }
    }
}
