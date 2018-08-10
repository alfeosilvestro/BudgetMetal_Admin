using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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

        [ForeignKey("Rfq")]
        public int Rfq_Id { get; set; }
        public virtual Rfq Rfq { get; set; }
    }
}
