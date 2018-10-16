using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.BudgetMetal.DBEntities
{
    public class CompanySupplier : GenericEntity
    {
        public int Company_Id { get; set; }

        [ForeignKey("Company_Id")]
        public virtual Company Company { get; set; }

        public int Supplier_Id { get; set; }

        [ForeignKey("Supplier_Id")]
        public virtual Company Supplier { get; set; }
    }
}
