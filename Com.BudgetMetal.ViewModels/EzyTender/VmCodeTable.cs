using System;
using System.Collections.Generic;

namespace Com.BudgetMetal.ViewModels.EzyTender
{
    public class VmCodeTable : ViewModelItemBase
    {
        //public CodeTable()
        //{
        //    DocumentDocumentStatus = new HashSet<Document>();
        //    DocumentDocumentType = new HashSet<Document>();
        //    User = new HashSet<User>();
        //}

       
        public uint CodeCategoryId { get; set; }
        public string Name { get; set; }

        public VmCodeCategory CodeCategory { get; set; }
        public ICollection<VmDocument> DocumentDocumentStatus { get; set; }
        public ICollection<VmDocument> DocumentDocumentType { get; set; }
        public ICollection<VmUser> User { get; set; }
    }
}
