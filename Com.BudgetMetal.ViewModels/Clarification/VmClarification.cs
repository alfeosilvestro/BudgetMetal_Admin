using System;
using System.Collections.Generic;
using Com.BudgetMetal.ViewModels.Document;
using Com.BudgetMetal.ViewModels.User;

namespace Com.BudgetMetal.ViewModels.Clarification
{
    public class VmClarificationItem : ViewModelItemBase
    {
        
        public int DocumentId { get; set; }
        public int UserId { get; set; }
        public string ClarificationQuestion { get; set; }
        public string ClarificationAnswer { get; set; }
        public bool? AnswerType { get; set; }
        public int Clarification_Id { get; set; }

        public virtual VmDocumentItem Document { get; set; }
        public virtual VmUserItem User { get; set; }
    }
}
