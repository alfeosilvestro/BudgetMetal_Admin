using Com.BudgetMetal.ViewModels.CodeTable;
using Com.BudgetMetal.ViewModels.Company;
using Com.BudgetMetal.ViewModels.Role;
using Com.BudgetMetal.ViewModels.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.BudgetMetal.ViewModels.UserRoles
{
    public class VmUserRolesItem : ViewModelItemBase
    {

       
        public int Role_Id { get; set; }
        public virtual VmRoleItem Role { get; set; }

       
        public int User_Id { get; set; }
        public virtual VmUserItem User { get; set; }
    }
}
