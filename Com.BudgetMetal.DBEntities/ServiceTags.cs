using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.BudgetMetal.DBEntities
{
    public class ServiceTags : GenericEntity
    {
        //public ServiceTags()
        //{
        //    SupplierServiceTags = new HashSet<SupplierServiceTags>();
        //}
        
        public string Name { get; set; }
        public string Description { get; set; }

        //[ForeignKey("Industry")]
        public uint Industry_Id { get; set; }
        //public Industry Industry { get; set; }
        //public ICollection<SupplierServiceTags> SupplierServiceTags { get; set; }
    }
}
