using System;
using System.Collections.Generic;

namespace Com.BudgetMetal.DBEntities
{
    public class ServiceTags : GenericEntity
    {
        public ServiceTags()
        {
            SupplierServiceTags = new HashSet<SupplierServiceTags>();
        }

       
        public uint IndustryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Industry Industry { get; set; }
        public ICollection<SupplierServiceTags> SupplierServiceTags { get; set; }
    }
}
