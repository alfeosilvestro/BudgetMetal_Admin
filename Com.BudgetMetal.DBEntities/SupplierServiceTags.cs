using System;
using System.Collections.Generic;

namespace Com.BudgetMetal.DBEntities
{
    public class SupplierServiceTags : GenericEntity
    {
        
        public int ServiceTags_Id { get; set; }
        public int Company_Id { get; set; }

        public Company Company { get; set; }
        public ServiceTags ServiceTags { get; set; }
    }
}
