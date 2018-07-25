using System;
using System.Collections.Generic;

namespace Com.BudgetMetal.DB.Entities
{
    public class UserRoles : GenericEntity
    {
        
        public uint RoleId { get; set; }
        public uint UserId { get; set; }

        //public Role Role { get; set; }
        //public User User { get; set; }
    }
}
