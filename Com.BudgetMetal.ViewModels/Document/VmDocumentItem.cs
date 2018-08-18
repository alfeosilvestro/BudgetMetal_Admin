using Com.BudgetMetal.ViewModels.CodeTable;
using Com.BudgetMetal.ViewModels.Company;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.ViewModels.Document
{
    public class VmDocumentItem : ViewModelItemBase
    {
        public string Title { get; set; }

        public string ContactPersonName { get; set; }
        public string DocumentNo { get; set; }
        public DateTime? SubmissionDate { get; set; }

        public int Company_Id { get; set; }

        public int DocumentStatus_Id { get; set; }

        public int DocumentType_Id { get; set; }    

        //[ForeignKey("Company_Id")]
        //public virtual Company Company { get; set; }


        //[ForeignKey("DocumentStatus_Id")]
        //public virtual CodeTable DocumentStatus { get; set; }
        
            
        //[ForeignKey("DocumentType_Id")]
        //public virtual CodeTable DocumentType { get; set; }
        public VmCodeTableItem DocumentStatus { get; set; }
        public List<VmCodeTableItem> DocumentStatusList { get; set; }

        
        public VmCodeTableItem DocumentType { get; set; }
        public List<VmCodeTableItem> DocumentTypeList { get; set; }

        public VmCompanyItem Company { get; set; }
        public List<VmCompanyItem> CompanyList { get; set; }

        
    }
}
