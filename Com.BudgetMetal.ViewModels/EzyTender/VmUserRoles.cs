using System;
using System.Collections.Generic;

namespace Com.BudgetMetal.ViewModels.EzyTender
{
    public class VmUserRoles : ViewModelItemBase
    {
        
        public uint RoleId { get; set; }
        public uint UserId { get; set; }

        public VmRole Role { get; set; }
        public VmUser User { get; set; }
    }
}
