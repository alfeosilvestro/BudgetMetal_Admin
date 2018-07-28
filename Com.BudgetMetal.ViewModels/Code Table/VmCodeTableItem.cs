using Com.BudgetMetal.DB.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.ViewModels.Code_Table
{
    public class VmCodeTableItem : ViewModelItemBase
    {
        
        public uint CodeCategory_Id { get; set; }
        public string Name { get; set; }
        public CodeCategory CodeCategory { get; set; }

        public List<Code MyProperty { get; set; }
    }
}
