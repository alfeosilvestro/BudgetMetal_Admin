using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.BudgetMetal.ViewModels.EzyTender
{
    public class VmRole : ViewModelItemBase
    {       
        //public string RoleCode { get; set; }

        //public string Role{ get; set; }

        //public Role()
        //{
        //    DocumentUser = new HashSet<DocumentUser>();
        //    UserRoles = new HashSet<UserRoles>();
        //}

        public string VmCode { get; set; }
        public string VmName { get; set; }

        public ICollection<VmDocumentUser> DocumentUser { get; set; }
        public ICollection<VmUserRoles> UserRoles { get; set; }

    }
}
