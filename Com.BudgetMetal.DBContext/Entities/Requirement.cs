using System;
using System.Collections.Generic;

namespace Com.BudgetMetal.DB.Entities
{
    public class Requirement : GenericEntity
    {
       
        public uint RfqId { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public int? Quantity { get; set; }

        public Rfq Rfq { get; set; }
    }
}
