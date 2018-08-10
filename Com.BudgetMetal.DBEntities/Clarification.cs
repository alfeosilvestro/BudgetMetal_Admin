using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.BudgetMetal.DBEntities
{
    public class Clarification : GenericEntity
    {
        
        public string ClarificationQuestion { get; set; }
        public string ClarificationAnswer { get; set; }
        public bool? AnswerType { get; set; }

        [ForeignKey("Document")]
        public int Document_Id { get; set; }
        public Document Document { get; set; }

        [ForeignKey("User")]
        public int User_Id { get; set; }
        public User User { get; set; }
    }
}
