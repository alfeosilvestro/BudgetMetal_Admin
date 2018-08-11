using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.BudgetMetal.DBEntities
{
    public class Role : GenericEntity
    {       
        //public string RoleCode { get; set; }

        //public string Role{ get; set; }

        //public Role()
        //{
        //    DocumentUser = new HashSet<DocumentUser>();
        //    UserRoles = new HashSet<UserRoles>();
        //}

        public string Code { get; set; }
        public string Name { get; set; }

        //public ICollection<DocumentUser> DocumentUser { get; set; }
        public ICollection<UserRoles> UserRoles { get; } = new List<UserRoles>();

    }
}
