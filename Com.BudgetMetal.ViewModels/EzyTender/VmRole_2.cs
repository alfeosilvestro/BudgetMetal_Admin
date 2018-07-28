using System;
using System.Collections.Generic;

namespace Com.BudgetMetal.ViewModels.EzyTender
{
    public class VmRole_2 : ViewModelItemBase
    {
        //public Role_2()
        //{
        //    DocumentUser = new HashSet<DocumentUser>();
        //    UserRoles = new HashSet<UserRoles>();
        //}
        
        public string Code { get; set; }
        public string Name { get; set; }

        public ICollection<VmDocumentUser> DocumentUser { get; set; }
        public ICollection<VmUserRoles> UserRoles { get; set; }
    }
}
