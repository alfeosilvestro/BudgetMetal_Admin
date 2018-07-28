using System;
using System.Collections.Generic;

namespace Com.BudgetMetal.ViewModels.EzyTender
{
    public class VmClarification : ViewModelItemBase
    {
        
        public uint DocumentId { get; set; }
        public uint UserId { get; set; }
        public string ClarificationQuestion { get; set; }
        public string ClarificationAnswer { get; set; }
        public bool? AnswerType { get; set; }

        public VmDocument Document { get; set; }
        public VmUser User { get; set; }
    }
}
