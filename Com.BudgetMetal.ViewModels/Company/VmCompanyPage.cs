using Com.BudgetMetal.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.ViewModels.Company
{
    public class VmCompanyPage : ViewModelBase
    {
        public PageResult<VmCompanyItem> Result { get; set; }
    }
}
