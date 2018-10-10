using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Com.BudgetMetal.DBEntities
{
    public class TimeLine : GenericEntity
    {
        public TimeLine()
        {
            //Clarification = new HashSet<Clarification>();
            //DocumentUser = new HashSet<DocumentUser>();
            //Rating = new HashSet<Rating>();
            //UserRoles = new HashSet<UserRoles>();
        }


        [ForeignKey("Company")]
        public int Company_Id { get; set; }
        public virtual Company Company { get; set; }

        [ForeignKey("User")]
        public int User_Id { get; set; }    
        public virtual User User { get; set; }


        public string Message { get; set; }

        [ForeignKey("Document")]
        public int Document_Id { get; set; }
        public virtual Document Document { get; set; }

        [ForeignKey("CodeTable")]
        public int MessageType { get; set; }
        public virtual CodeTable MType { get; set; }

        public bool IsRead { get; set; }

    }
}
