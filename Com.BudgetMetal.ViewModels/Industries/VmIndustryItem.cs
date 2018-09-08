using System;
using System.ComponentModel.DataAnnotations;

namespace Com.BudgetMetal.ViewModels.Industries
{
    public class VmIndustryItem : ViewModelItemBase
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
    }
}
