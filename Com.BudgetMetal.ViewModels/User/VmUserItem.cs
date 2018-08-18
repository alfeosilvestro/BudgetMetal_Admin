using Com.BudgetMetal.ViewModels.CodeTable;
using Com.BudgetMetal.ViewModels.Company;
using Com.BudgetMetal.ViewModels.Role;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.ViewModels.User
{
    public class VmUserItem : ViewModelItemBase
    {
        //public string EmailAddress { get; set; }

        //public string Password { get; set; }

        //public string Title { get; set; }

        //public string UserName { get; set; }

        //public int RoleId { get; set; }

        //public string SiteAdmin { get; set; }

        //public bool Status { get; set; }

        //public bool Confirmed { get; set; }

        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string ContactName { get; set; }
        public string JobTitle { get; set; }
        public string ContactNumber { get; set; }
        public bool IsConfirmed { get; set; }
        public int Company_Id { get; set; }
        public virtual VmCompanyItem Company {get;set;}
        public List<VmCompanyItem> CompanyList { get; set; }
        public int UserType { get; set; }
        public virtual VmCodeTableItem CodeTable { get; set; }
        public List<VmCodeTableItem> CodeTableList { get; set; }
        public List<VmRoleItem> RoleList { get; set; }
    }
}
