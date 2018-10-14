using System;
using Com.BudgetMetal.Common;

namespace Com.BudgetMetal.ViewModels.UserRoles
{
    public class VmUserRolesPage : ViewModelBase
    {
        public PageResult<VmUserRolesItem> Result { get; set; }
    }
}
