using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Com.BudgetMetal.DBEntities
{
    public class Industry : GenericEntity
    {
        //public Industry()
        //{
        //    ServiceTags = new HashSet<ServiceTags>();
        //}

        public string Name { get; set; }

        //public ICollection<ServiceTags> ServiceTags { get; set; }
    }
}
