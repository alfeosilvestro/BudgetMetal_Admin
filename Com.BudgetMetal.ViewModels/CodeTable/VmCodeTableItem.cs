using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Com.BudgetMetal.ViewModels.CodeCategory;

namespace Com.BudgetMetal.ViewModels.CodeTable
{
    public class VmCodeTableItem : ViewModelItemBase
    {
        [Required(ErrorMessage = "Code category is required!")]
        public int CodeCategory_Id { get; set; }
        [Required(ErrorMessage = "Name is required!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Value is required!")]
        public string Value { get; set; }
        public VmCodeCategoryItem CodeCategory { get; set; }
        public List<VmCodeCategoryItem> CodeCategoryList { get; set; }
    }
}
