using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.BudgetMetal.DBEntities
{
    public class SupplierIndustryCertification : GenericEntity
    {
        [ForeignKey("Company")]
        public int Company_Id { get; set; }
        public virtual Company Company { get; set; }


        public string Certification { get; set; }

        public DateTime ExpiredDate { get; set; }
    }
}
