using System;
using System.Collections.Generic;

namespace Com.BudgetMetal.DB.Entities
{
    public class Role_2 : GenericEntity
    {
        public Role_2()
        {
            DocumentUser = new HashSet<DocumentUser>();
            UserRoles = new HashSet<UserRoles>();
        }
        
        public string Code { get; set; }
        public string Name { get; set; }

        public ICollection<DocumentUser> DocumentUser { get; set; }
        public ICollection<UserRoles> UserRoles { get; set; }
    }
}
