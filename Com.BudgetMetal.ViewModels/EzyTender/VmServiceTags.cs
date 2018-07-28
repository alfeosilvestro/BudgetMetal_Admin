using System;
using System.Collections.Generic;

namespace Com.BudgetMetal.ViewModels.EzyTender
{
    public class VmServiceTags : ViewModelItemBase
    {
        //public ServiceTags()
        //{
        //    SupplierServiceTags = new HashSet<SupplierServiceTags>();
        //}

       
        public uint IndustryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public VmIndustry Industry { get; set; }
        public ICollection<VmSupplierServiceTags> SupplierServiceTags { get; set; }
    }
}
