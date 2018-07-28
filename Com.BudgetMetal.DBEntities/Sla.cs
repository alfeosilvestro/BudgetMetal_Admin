using System;
using System.Collections.Generic;

namespace Com.BudgetMetal.DBEntities
{
    public class Sla : GenericEntity
    {
      
        public uint RfqId { get; set; }
        public string Requirement { get; set; }
        public string Description { get; set; }

        public Rfq Rfq { get; set; }
    }
}
