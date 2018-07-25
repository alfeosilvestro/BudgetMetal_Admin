using System;
using System.Collections.Generic;

namespace Com.BudgetMetal.DB.Entities
{
    public class Attachment : GenericEntity
    {
        
        public uint DocumentId { get; set; }
        public string FileName { get; set; }
        public byte[] FileBinary { get; set; }
        public string Description { get; set; }
        public long? FileSize { get; set; }
        

        public Document Document { get; set; }
    }
}
