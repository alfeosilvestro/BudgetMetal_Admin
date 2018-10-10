using Com.BudgetMetal.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.ViewModels.TimeLine
{
    public class VmTimeLinePage : ViewModelBase
    {
        public PageResult<VmTimeLineItem> Result { get; set; }
    }
}
