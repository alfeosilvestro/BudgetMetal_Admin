using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Com.BudgetMetal.DBEntities
{
    public class DocumentActivity : GenericEntity
    {
        public string Action { get; set; }

        public int Document_Id { get; set; }

        public bool IsRfq { get; set; }

        [ForeignKey("Document_Id")]
        public virtual Document Document { get; set; }

        public int User_Id { get; set; }

        [ForeignKey("User_Id")]
        public virtual User User { get; set; }
    }
}
