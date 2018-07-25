using System;
using System.Collections.Generic;

namespace Com.BudgetMetal.DB.Entities
{
    public class DocumentUser : GenericEntity
    {
       
        public uint DocumentId { get; set; }
        public uint RoleId { get; set; }
        public uint UserId { get; set; }
       

        public Document Document { get; set; }
        public Role Role { get; set; }
        public User User { get; set; }
    }
}
