﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.BudgetMetal.DBEntities
{
    public class Attachment : GenericEntity
    {
        
        
        public string FileName { get; set; }
        public string FileBinary { get; set; }
        public string Description { get; set; }
        public long? FileSize { get; set; }

        [ForeignKey("Document")]
        public int Document_Id { get; set; }
        public virtual Document Document { get; set; }
    }
}
