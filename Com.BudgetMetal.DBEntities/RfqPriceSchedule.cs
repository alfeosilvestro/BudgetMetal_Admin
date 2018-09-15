using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.BudgetMetal.DBEntities
{
    public class RfqPriceSchedule : GenericEntity
    {
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public string InternalRefrenceCode { get; set; }
        public string QuantityRequired { get; set; }
      
        public int Rfq_Id { get; set; }

        [ForeignKey("Rfq_Id")]
        public virtual Rfq Rfq { get; set; }
    }
}
