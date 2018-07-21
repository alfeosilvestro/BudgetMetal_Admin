using System;
using Com.BudgetMetal.Common;

namespace Com.BudgetMetal.ViewModels.User
{
    public class VmUserPage : ViewModelBase
    {
        public PageResult<VmUserItem> Result { get; set; }
    }
}
