using Com.BudgetMetal.ViewModels.Industries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.ViewModels.ServiceTags
{
    public class VmServiceTagsItem : ViewModelItemBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Industry_Id { get; set; }
        public virtual VmIndustryItem Industry { get; set; }
        public List<VmIndustryItem> IndustryList { get; set; }
    }
}
