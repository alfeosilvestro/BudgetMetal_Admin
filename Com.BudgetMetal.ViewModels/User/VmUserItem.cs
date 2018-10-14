using Com.BudgetMetal.ViewModels.CodeTable;
using Com.BudgetMetal.ViewModels.Company;
using Com.BudgetMetal.ViewModels.Role;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required(ErrorMessage = "Email is required!")]        
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email is not valid")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "Password is required!")]
        public string Password { get; set; }
        [Required(ErrorMessage = "User name is required!")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Contact name is required!")]
        public string ContactName { get; set; }
        [Required(ErrorMessage = "Job title is required!")]
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
        public List<int> SelectedRoleId { get; set; }
        public List<VmRoleItem> SelectedRoles { get; set; }
    }
}
