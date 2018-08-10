using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.BudgetMetal.DBEntities
{
    public class DocumentUser : GenericEntity
    {
        [ForeignKey("CodeCategory")]
        public int DocumentId { get; set; }
public virtual Document Document { get; set; }

        [ForeignKey("CodeCategory")]
        public int RoleId { get; set; }
 public virtual Role Role { get; set; }

        [ForeignKey("CodeCategory")]
        public int UserId { get; set; }
       public virtual User User { get; set; }
    }
}
