using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.BudgetMetal.DBEntities
{
    public class InvitedSupplier : GenericEntity
    {
       
        public int Rfq_Id { get; set; }

        [ForeignKey("Rfq_Id")]
        public virtual Rfq Rfq { get; set; }

       
        public int Company_Id { get; set; }

        public bool NotRelevant { get; set; }

        [ForeignKey("Company_Id")]
        public virtual Company Company { get; set; }


        
    }
}
