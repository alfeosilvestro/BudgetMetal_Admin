using System;
using System.Collections.Generic;

namespace Com.BudgetMetal.ViewModels.EzyTender
{
    public class VmDocumentUser : ViewModelItemBase
    {
       
        public uint DocumentId { get; set; }
        public uint RoleId { get; set; }
        public uint UserId { get; set; }
       

        public VmDocument Document { get; set; }
        public VmRole Role { get; set; }
        public VmUser User { get; set; }
    }
}
