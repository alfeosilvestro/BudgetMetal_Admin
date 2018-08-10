﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.ViewModels.EmailLog
{
    public class VmEmailLogItem
    {
        public int Id { get; set; }
        public string ToEmailAddress { get; set; }
        public string Message { get; set; }
        public int? MailStatus { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? SentDate { get; set; }
        public int? SentByUserId { get; set; }
    }
}
