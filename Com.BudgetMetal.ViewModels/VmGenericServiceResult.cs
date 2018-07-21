using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.ViewModels
{
    public class VmGenericServiceResult : ViewModelBase
    {
        public bool IsSuccess { get; set; }
        public string MessageToUser { get; set; }
        public Exception Error { get; set; }
    }
}
