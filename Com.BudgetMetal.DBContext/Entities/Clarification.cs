using System;
using System.Collections.Generic;

namespace Com.BudgetMetal.DB.Entities
{
    public class Clarification : GenericEntity
    {
        
        public uint DocumentId { get; set; }
        public uint UserId { get; set; }
        public string ClarificationQuestion { get; set; }
        public string ClarificationAnswer { get; set; }
        public bool? AnswerType { get; set; }

        public Document Document { get; set; }
        public User User { get; set; }
    }
}
