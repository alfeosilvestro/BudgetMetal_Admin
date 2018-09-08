using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.BudgetMetal.DBEntities
{
    public class CompanySupplier : GenericEntity
    {
        [ForeignKey("Company_Id")]
        public int Company_Id { get; set; }
        public virtual Company Company { get; set; }

        [ForeignKey("Supplier_Id")]
        public int Supplier_Id { get; set; }
        public virtual Company Supplier { get; set; }
    }
}
