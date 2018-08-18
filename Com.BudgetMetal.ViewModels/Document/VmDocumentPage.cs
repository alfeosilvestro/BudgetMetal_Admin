using Com.BudgetMetal.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.ViewModels.Document
{
    public class VmDocumentPage : ViewModelBase
    {
        public PageResult<VmDocumentItem> Result { get; set; }
    }
}
