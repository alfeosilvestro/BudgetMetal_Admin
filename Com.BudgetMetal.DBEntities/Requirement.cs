using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.BudgetMetal.DBEntities
{
    public class Requirement : GenericEntity
    {
       
       
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public int? Quantity { get; set; }

       
        public int Rfq_Id { get; set; }

        [ForeignKey("Rfq_Id")]
        public virtual Rfq Rfq { get; set; }
    }
}
