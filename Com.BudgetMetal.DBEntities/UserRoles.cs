using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.BudgetMetal.DBEntities
{
    public class UserRoles : GenericEntity
    {
        [ForeignKey("Role")]
        public int Role_Id { get; set; }
        public virtual Role Role { get; set; }

        [ForeignKey("User")]
        public int User_Id { get; set; }
        public virtual User User { get; set; }
    }
}
