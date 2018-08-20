using System;
using System.Collections.Generic;
using Com.BudgetMetal.ViewModels.Document;

namespace Com.BudgetMetal.ViewModels.Attachment
{
    public class VmAttachmentItem : ViewModelItemBase
    {
        
        public int Document_Id { get; set; }
        public string FileName { get; set; }
        public string FileBinary { get; set; }
        public string Description { get; set; }
        public long? FileSize { get; set; }
        

        public virtual VmDocumentItem Document { get; set; }
    }
}
