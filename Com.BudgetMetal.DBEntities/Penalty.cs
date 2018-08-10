using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.BudgetMetal.DBEntities
{
    public class Penalty : GenericEntity
    {
       
        public string BreachOfServiceDefinition { get; set; }
        public string PenaltyAmount { get; set; }
        public string Description { get; set; }

        [ForeignKey("Rfq")]
        public int Rfq_Id { get; set; }
        public virtual Rfq Rfq { get; set; }
    }
}
