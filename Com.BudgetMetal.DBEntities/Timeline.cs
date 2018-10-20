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

        
        public int Company_Id { get; set; }
        
        public int User_Id { get; set; }    


        public string Message { get; set; }
        
        public int Document_Id { get; set; }

       
        public int MessageType { get; set; }

        public bool IsRead { get; set; }

    }
}
