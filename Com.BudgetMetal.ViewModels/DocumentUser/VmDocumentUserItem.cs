using Com.BudgetMetal.ViewModels.Document;
using Com.BudgetMetal.ViewModels.Role;
using Com.BudgetMetal.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.ViewModels.DocumentUser
{
    public class VmDocumentUserItem : ViewModelItemBase
    {
       
        public int Document_Id { get; set; }
        public virtual VmDocumentItem Document { get; set; }

        
        public int Role_Id { get; set; }
        public virtual VmRoleItem Role { get; set; }

       
        public int User_Id { get; set; }
        public virtual VmUserItem User { get; set; }

       
    }
}
