using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.BudgetMetal.DBEntities
{
    public class QuotationRequirement : GenericEntity
    {
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public string Compliance { get; set; }
        public string SupplierDescription { get; set; }
        
        public int Quotation_Id { get; set; }

        [ForeignKey("Quotation_Id")]
        public virtual Quotation Quotation { get; set; }
    }
}
