using System;
using System.Collections.Generic;

namespace Com.BudgetMetal.ViewModels.EzyTender
{
    public class VmSupplierServiceTags : ViewModelItemBase
    {
        
        public uint ServiceTagsId { get; set; }
        public uint CompanyId { get; set; }

        public VmCompany Company { get; set; }
        public VmServiceTags ServiceTags { get; set; }
    }
}
