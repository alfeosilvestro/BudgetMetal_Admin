using Com.BudgetMetal.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.ViewModels.ServiceTags
{
    public class VmServiceTagsPage : ViewModelBase
    {
        public PageResult<VmServiceTagsItem> Result { get; set; }
    }
}
