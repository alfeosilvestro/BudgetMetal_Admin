using Com.BudgetMetal.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.ViewModels.EmailLog
{
    public class VmEmailLogPage : ViewModelBase
    {
        public PageResult<VmEmailLogItem> Result { get; set; }
    }
}
