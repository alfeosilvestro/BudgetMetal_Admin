using System;
using System.Collections.Generic;

namespace Com.BudgetMetal.DB.Entities
{
    public class Industry : GenericEntity
    {
        public Industry()
        {
            ServiceTags = new HashSet<ServiceTags>();
        }

        
        public string Name { get; set; }

        public ICollection<ServiceTags> ServiceTags { get; set; }
    }
}
