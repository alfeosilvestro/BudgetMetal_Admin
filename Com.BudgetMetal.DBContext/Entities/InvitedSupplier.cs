using System;
using System.Collections.Generic;

namespace Com.BudgetMetal.DB.Entities
{
    public class InvitedSupplier : GenericEntity
    {
        
        public uint RfqId { get; set; }
        public uint CompanyId { get; set; }
        

        public Company Company { get; set; }
        public Rfq Rfq { get; set; }
    }
}
