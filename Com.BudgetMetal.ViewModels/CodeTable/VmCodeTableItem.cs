using System;
using System.Collections.Generic;
using System.Text;
using Com.BudgetMetal.ViewModels.CodeCategory;

namespace Com.BudgetMetal.ViewModels.CodeTable
{
    public class VmCodeTableItem : ViewModelItemBase
    {        
        public uint CodeCategory_Id { get; set; }
        public string Name { get; set; }
        public VmCodeCategoryItem CodeCategory { get; set; }
    }
}
