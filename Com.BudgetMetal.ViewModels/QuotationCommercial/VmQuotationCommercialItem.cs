using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.BudgetMetal.ViewModels.QuotationCommercial
{
    public class VmQuotationCommercialItem : ViewModelItemBase
    {
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public string Compliance { get; set; }
        public string SupplierDescription { get; set; }

        public int Quotation_Id { get; set; }
    }
}
