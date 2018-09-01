using Com.BudgetMetal.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.ViewModels.Quotation
{
    public class VmQuotationPage : ViewModelBase
    {
        public PageResult<VmQuotationItem> Result { get; set; }
    }
}
