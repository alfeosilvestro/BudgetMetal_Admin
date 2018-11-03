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

        
        public int Document_Id { get; set; }

        [ForeignKey("Document_Id")]
        public Document Document { get; set; }

       
        public int User_Id { get; set; }

        [ForeignKey("User_Id")]
        public User User { get; set; }

        public int Clarification_Id { get; set; }
    }
}
