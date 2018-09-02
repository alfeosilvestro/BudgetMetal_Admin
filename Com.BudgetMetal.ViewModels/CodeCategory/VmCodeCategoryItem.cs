using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.BudgetMetal.ViewModels.CodeCategory
{
    public class VmCodeCategoryItem : ViewModelItemBase
    {
        [Required(ErrorMessage ="Name is required!")]
        public string Name { get; set; }
    }
}
