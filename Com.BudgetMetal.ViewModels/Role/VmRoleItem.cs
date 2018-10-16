using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.BudgetMetal.ViewModels.Role
{
    public class VmRoleItem : ViewModelItemBase
    {
        
        [Required(ErrorMessage = "Name is required!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Code is required!")]
        public string Code { get; set; }
    }
}
