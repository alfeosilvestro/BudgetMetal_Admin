﻿using System;
using System.Collections.Generic;

namespace Com.BudgetMetal.ViewModels.EzyTender
{
    public class VmEmailLog
    {
        public uint Id { get; set; }
        public string ToEmailAddress { get; set; }
        public string Message { get; set; }
        public int? MailStatus { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? SentDate { get; set; }
        public int? SentByUserId { get; set; }
    }
}
