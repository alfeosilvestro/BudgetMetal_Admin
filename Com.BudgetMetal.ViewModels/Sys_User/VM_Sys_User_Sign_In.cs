using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
namespace Com.BudgetMetal.ViewModels.Sys_User
{
    public class VM_Sys_User_Sign_In
    {
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
