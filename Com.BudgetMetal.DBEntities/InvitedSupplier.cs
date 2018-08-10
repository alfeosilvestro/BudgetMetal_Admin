using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.BudgetMetal.DBEntities
{
    public class InvitedSupplier : GenericEntity
    {
        [ForeignKey("Rfq")]
        public int Rfq_Id { get; set; }
        public virtual Rfq Rfq { get; set; }

        [ForeignKey("Company")]
        public int Company_Id { get; set; }
        public virtual Company Company { get; set; }


        
    }
}
