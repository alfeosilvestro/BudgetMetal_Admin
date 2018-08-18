using Com.BudgetMetal.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.ViewModels.Rfq
{
    public class VmRfqPage : ViewModelBase
    {
        public PageResult<VmRfqItem> Result { get; set; }
    }
}
