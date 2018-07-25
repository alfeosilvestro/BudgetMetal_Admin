using System;
using System.Collections.Generic;

namespace Com.BudgetMetal.DB.Entities
{
    public class Penalty : GenericEntity
    {
       
        public uint RfqId { get; set; }
        public string BreachOfServiceDefinition { get; set; }
        public string PenaltyAmount { get; set; }
        public string Description { get; set; }

        public Rfq Rfq { get; set; }
    }
}
