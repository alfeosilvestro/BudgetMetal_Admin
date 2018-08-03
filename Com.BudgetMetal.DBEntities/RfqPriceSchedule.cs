using System;
using System.Collections.Generic;

namespace Com.BudgetMetal.DBEntities
{
    public class RfqPriceSchedule
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public string InternalRefrenceCode { get; set; }
        public string QuantityRequired { get; set; }
        public string Version { get; set; }
        public uint RfqId { get; set; }

        public Rfq Rfq { get; set; }
    }
}
