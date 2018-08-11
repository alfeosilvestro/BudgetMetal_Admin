using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.BudgetMetal.DBEntities
{
    [Table("SLA")]
    public class Sla : GenericEntity
    {
        public string Requirement { get; set; }
        public string Description { get; set; }

       
        public int Rfq_Id { get; set; }

        [ForeignKey("Rfq_Id")]
        public virtual Rfq Rfq { get; set; }
    }
}
