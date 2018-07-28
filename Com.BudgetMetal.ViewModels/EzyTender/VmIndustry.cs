using System;
using System.Collections.Generic;

namespace Com.BudgetMetal.ViewModels.EzyTender
{
    public class VmIndustry : ViewModelItemBase
    {
        //public Industry()
        //{
        //    ServiceTags = new HashSet<ServiceTags>();
        //}

        
        public string Name { get; set; }

        public ICollection<VmServiceTags> ServiceTags { get; set; }
    }
}
