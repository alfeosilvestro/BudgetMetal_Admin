using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.ViewModels.EmailLog
{
    public class VmEmailLogItem : ViewModelItemBase
    {
        public string ToEmailAddress { get; set; }
        public string Message { get; set; }
        public int? MailStatus { get; set; }
        public DateTime? SentDate { get; set; }
        public int? SentByUserId { get; set; }
    }
}
