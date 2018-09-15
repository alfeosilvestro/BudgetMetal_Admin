using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.ViewModels.QuotationRequirement
{
    public class VmQuotationRequirementItem : ViewModelItemBase
    {
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public string Compliance { get; set; }
        public string SupplierDescription { get; set; }

        public int Quotation_Id { get; set; }
    }
}
