using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.BudgetMetal.DBEntities
{
    public class SupplierServiceTags : GenericEntity
    {
        [ForeignKey("ServiceTags")]
        public int ServiceTags_Id { get; set; }
        public virtual ServiceTags ServiceTags { get; set; }

        [ForeignKey("Company")]
        public int Company_Id { get; set; }
        public virtual Company Company { get; set; }
    }
}
