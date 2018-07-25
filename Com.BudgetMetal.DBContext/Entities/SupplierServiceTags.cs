using System;
using System.Collections.Generic;

namespace Com.BudgetMetal.DB.Entities
{
    public class SupplierServiceTags : GenericEntity
    {
        
        public uint ServiceTagsId { get; set; }
        public uint CompanyId { get; set; }

        public Company Company { get; set; }
        public ServiceTags ServiceTags { get; set; }
    }
}
