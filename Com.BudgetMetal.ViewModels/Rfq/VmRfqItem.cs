using Com.BudgetMetal.ViewModels.Document;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.ViewModels.Rfq
{
    public class VmRfqItem : ViewModelItemBase
    {
        public string InternalRefrenceNo { get; set; }
        public string InternalProjectName { get; set; }
        public DateTime? StartRfqdate { get; set; }
        public DateTime? ValidRfqdate { get; set; }
        public DateTime? EstimatedProjectStartDate { get; set; }
        public DateTime? EstimatedProjectEndDate { get; set; }
        public bool SupplierProvideMaterial { get; set; }
        public bool SupplierProvideTransport { get; set; }
        public string MessageToSupplier { get; set; }
        public string IndustryOfRfq { get; set; }
        public string SelectedTags { get; set; }


        public int Document_Id { get; set; }

        public VmDocumentItem Document { get; set; }
        public List<VmDocumentItem> DocumentList { get; set; }
    }
}
