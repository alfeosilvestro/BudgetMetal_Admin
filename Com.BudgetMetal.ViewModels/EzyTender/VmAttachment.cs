using System;
using System.Collections.Generic;

namespace Com.BudgetMetal.ViewModels.EzyTender
{
    public class VmAttachment : ViewModelItemBase
    {
        
        public uint DocumentId { get; set; }
        public string FileName { get; set; }
        public byte[] FileBinary { get; set; }
        public string Description { get; set; }
        public long? FileSize { get; set; }
        

        public VmDocument Document { get; set; }
    }
}
